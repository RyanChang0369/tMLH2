using System;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace tMLH2
{
    /// <summary>
    /// A bitmap image that is divided into 1 or more panes
    /// </summary>
    public class PanedImage
    {
        /// <summary>
        /// Total number of pixels per frame in this paned image
        /// </summary>
        public int PixelsPerFrame = -1;

        public int Frames
        {
            get
            {
                if (SourceImage != null && SourceImage.PixelHeight > 0 && PixelsPerFrame > 0)
                {
                    double Frames = Math.Ceiling((double)SourceImage.PixelHeight / PixelsPerFrame);
                    return (int)Frames;
                }
                else
                    throw new ArgumentException("Cannot determine correct number of frames. ");
            }
        }

        /// <summary>
        /// Current frame of this paned image. This equals x, where 0 &lt;= x &lt;= frames
        /// </summary>
        public int CurrentFrame;

        /// <summary>
        /// The bitmap image
        /// </summary>
        public BitmapImage SourceImage;

        /// <summary>
        /// The cropped bitmap associated with this object
        /// </summary>
        public CroppedBitmap CroppedBitmap;

        /// <summary>
        /// The System.Windows.Controls.Image associated with this object
        /// </summary>
        public Image ImageElement;

        /// <summary>
        /// The button that increments the current pane that is associated with this object
        /// </summary>
        public Button NextPaneButton;

        /// <summary>
        /// The button that decrements the current pane that is associated with this object
        /// </summary>
        public Button PreviousPaneButton;

        /// <summary>
        /// An integer that referes to the type of item this PanedImage represents. Refer to ItemType enum.
        /// </summary>
        public int ItemType;

        public string SourcePath;

        private FileSystemWatcher watcher = new FileSystemWatcher();

        /// <summary>
        /// Creates a paned image object. RUN INITSTATIC BEFORE RUNNING THIS.
        /// </summary>
        /// <param name="frames">Total number of frames in this paned image</param>
        /// <param name="startFrame">Current frame of this paned image. This equals x, where 0 &lt;= x &lt;= frames</param>
        /// <param name="bitmapImage">The bitmap associated with this object</param>
        /// <param name="imageElement">The System.Windows.Controls.Image associated with this object</param>
        /// <param name="nextPaneButton">The button that increments the current pane that is associated with this object</param>
        /// <param name="prevPaneButton">The button that decrements the current pane that is associated with this object</param>
        /// <param name="watchSource">If true, updates the file when it is changed</param>
        public PanedImage(int startFrame, int itemType, int pixelsPerFrame, BitmapImage bitmapImage, Image imageElement,
            Button nextPaneButton, Button prevPaneButton)
        {
            PixelsPerFrame = pixelsPerFrame;

            CurrentFrame = startFrame;
            SourceImage = bitmapImage;
            ImageElement = imageElement;
            NextPaneButton = nextPaneButton;
            PreviousPaneButton = prevPaneButton;

            nextPaneButton.Click += NextPaneButton_Click;
            prevPaneButton.Click += PrevPaneButton_Click;

            TogglePaneButtons();

            CroppedBitmap = new CroppedBitmap(SourceImage, ImageHandler.GetCropRect(Frames, CurrentFrame, SourceImage));

            imageElement.Source = CroppedBitmap;
        }

        /// <summary>
        /// Use to update an old source
        /// </summary>
        /// <param name="bitmap"></param>
        public void UpdateSource(System.Drawing.Bitmap bitmap)
        {
            UpdateSource(bitmap.ToBitmapImage());
        }

        /// <summary>
        /// Use to update an old source
        /// </summary>
        /// <param name="bitmapImage"></param>
        public void UpdateSource(BitmapImage bitmapImage)
        {
            SourceImage = bitmapImage;

            // is armor checked?
            CreateCroppedBitmap();
        }

        /// <summary>
        /// Use when a new source is required.
        /// </summary>
        /// <param name="bitmap">The new bitmap.</param>
        public void NewSource(string fullPath)
        {
            NewSource(new System.Drawing.Bitmap(fullPath).ToBitmapImage(), fullPath);
        }

        /// <summary>
        /// Use when a new source is required.
        /// </summary>
        public void NewSource(BitmapImage bitmapImage, string fullPath)
        {
            ItemType = SmartDetection.DetectType(fullPath, bitmapImage);
            UpdateSource(bitmapImage);
            DisposeWatcher();
            CheckForSourceUpdate(fullPath);
            PixelsPerFrame = SmartDetection.DetectFrameHeight(bitmapImage.PixelHeight, ItemType);
        }

        /// <summary>
        /// Use when a new source is needed to be loaded from a static resource
        /// </summary>
        /// <param name="bitmap"></param>
        public void NewSourceFromResource(System.Drawing.Bitmap bitmap)
        {
            UpdateSource(ImageHandler.ToBitmapImage(bitmap));
        }

        //[PermissionSet(SecurityAction.Demand, Name = "FullTrust")]
        public void CheckForSourceUpdate(string fullPath)
        {
            // Create a new FileSystemWatcher and set its properties.
            Console.WriteLine("PanedImage (1): fullPath: " + fullPath);
            watcher.Path = Path.GetDirectoryName(fullPath);
            watcher.Filter = Path.GetFileName(fullPath);

            // Add event handlers.
            watcher.Changed += SourceChanged;
            watcher.Created += SourceChanged;

            // Begin watching.

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("PanedImage (2): watcher loaded!");
        }


        /*private void ScaleImageElement()
        {
            ScaleTransform st = (ScaleTransform)ImageElement.RenderTransform;
            st.ScaleX = 2;
            st.ScaleY = 2;
        }*/

        /// <summary>
        /// Changes the ItemType to a new value.
        /// </summary>
        /// <param name="newType">The new type. Refer to MainWidow ItemType enum</param>
        public void ChangeItemType(int newType)
        {
            ItemType = newType;
        }

        /// <summary>
        /// Gets the BitmapImage of the SourceImage without locking file
        /// </summary>
        /// <param name="fullPath">The full path to the bitmap image</param>
        /// <returns></returns>
        public BitmapImage GetSourceBitmapImage(string fullPath)
        {
            using (FileStream fileStream = new FileStream(
                fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                BitmapImage bmpImg = new BitmapImage();
                bmpImg.BeginInit();
                bmpImg.StreamSource = fileStream;
                bmpImg.CacheOption = BitmapCacheOption.OnLoad;
                bmpImg.EndInit();
                bmpImg.Freeze();
                return bmpImg;
            }
        }

        private void SourceChanged(object sender, FileSystemEventArgs e)
        {
            watcher.EnableRaisingEvents = false;
            while (!FileDialog.IsFileReady(e.FullPath)) { }

            UpdateSource(GetSourceBitmapImage(e.FullPath));
            watcher.EnableRaisingEvents = true;
        }

        //Event Handlers
        private void PrevPaneButton_Click(object sender, RoutedEventArgs e)
        {
            DecrementFrame();
        }

        private void NextPaneButton_Click(object sender, RoutedEventArgs e)
        {
            IncrementFrame();
        }

        //Methods
        public void IncrementFrame()
        {
            CurrentFrame++;
            while (CurrentFrame > Frames)
                CurrentFrame -= Frames;

            ChangeFrame();
        }

        public void DecrementFrame()
        {
            CurrentFrame--;
            while (CurrentFrame < 0)
                CurrentFrame += Frames;

            ChangeFrame();
        }

        private void ChangeFrame()
        {
            TogglePaneButtons();

            // is armor checked?
            CreateCroppedBitmap();
        }

        /// <summary>
        /// Correctly creates a cropped bitmap
        /// </summary>
        private void CreateCroppedBitmap()
        {
            if (MainWindow.IsArmorLike(ItemType))
            {
                CroppedBitmap = new CroppedBitmap(SourceImage, ImageHandler.GetCropRect(Frames, CurrentFrame, SourceImage));
            }
            else
            {
                CroppedBitmap = new CroppedBitmap(SourceImage, new Int32Rect(0, 0, SourceImage.PixelWidth, SourceImage.PixelHeight));
            }

            try
            {
                ImageElement.Source = null;
                ImageElement.Source = CroppedBitmap;
            }
            catch (InvalidOperationException e)
            {
                CroppedBitmap tempBitmap = CroppedBitmap.Clone();
                tempBitmap.Freeze();

                App.Current.Dispatcher.Invoke(() =>
                {
                    ImageElement.Source = tempBitmap;
                });

                CroppedBitmap = tempBitmap;

                Console.WriteLine(e);
            }
        }

        public void TogglePaneButtons()
        {
            if (CurrentFrame == 0)
                PreviousPaneButton.IsEnabled = false;
            else
                PreviousPaneButton.IsEnabled = true;
            if (CurrentFrame >= Frames - 1)
                NextPaneButton.IsEnabled = false;
            else
                NextPaneButton.IsEnabled = true;
        }

        public void DisposeWatcher()
        {
            try
            {
                watcher.Dispose();
                watcher = new FileSystemWatcher();
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }
        }
    }
}