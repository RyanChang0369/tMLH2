using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.IO;

namespace tMLH2
{
    public class BitmapLayer
    {
        /// <summary>
        /// The BitmapImage that represents this BitmapLayer
        /// </summary>
        public Bitmap Source;
        
        public Bitmap[] BitmapFrames;
        
        public int FrameHeight { get => BitmapFrames[0].Height; }

        public int FrameWidth { get => BitmapFrames[0].Width; }

        public int NumberOfFrames { get => BitmapFrames.Length; }

        /// <summary>
        /// The ItemType associated with this BitmapLayer
        /// </summary>
        public int ItemType;

        public string FullPath;

        private LayeredImage ParentLayeredImage;

        public Bitmap CurrentFrame { get => BitmapFrames[ParentLayeredImage.CurrentFrame]; }
        
        private FileSystemWatcher watcher = new FileSystemWatcher();
        
        /// <summary>
        /// Creates a new BitmapLayer from a static resource
        /// </summary>
        public BitmapLayer(Bitmap source, LayeredImage parentLayeredImage)
        {
            Source = source;
            ItemType = (int)MainWindow.ItemType.Static;
            FullPath = null;
            ParentLayeredImage = parentLayeredImage;

            BitmapFrames = new Bitmap[MainWindow.GetNumberOfFrames(ItemType, source.Height, 56)];

            DivideBitmap();
        }
        
        public BitmapLayer(Bitmap source, LayeredImage parentLayeredImage, int itemType,
            int frameHeight, string fullPath, bool useWatcher)
        {
            if (source.Height == 1118)
                Source = ImageHandler.FixBitmap(source);
            else
                Source = source;

            ItemType = itemType;
            FullPath = fullPath;
            ParentLayeredImage = parentLayeredImage;

            BitmapFrames = new Bitmap[MainWindow.GetNumberOfFrames(itemType, Source.Height, frameHeight)];
            
            DivideBitmap();

            if (useWatcher && fullPath != null)
                CheckForSourceUpdate(fullPath);
        }
        
        public Bitmap GetFrame(int index)
        {
            while (index >= BitmapFrames.Length)
                index -= BitmapFrames.Length;

            return BitmapFrames[index];
        }
        
        private void DivideBitmap()
        {
            for (int i = 0; i < BitmapFrames.Length; i++)
            {
                BitmapFrames[i] = ImageHandler.CropBitmap(Source,
                    ImageHandler.GetCropRect(BitmapFrames.Length,
                    i, Source));
            }
        }

        /// <summary>
        /// Use this method to update the source
        /// </summary>
        public void UpdateSource(Bitmap newSource)
        {
            Source = newSource;
            DivideBitmap();
        }
        
        private void CheckForSourceUpdate(string fullPath)
        {
            // Create a new FileSystemWatcher and set its properties.
            Console.WriteLine("BitmapLayer (1): fullPath: " + fullPath);
            watcher.Path = Path.GetDirectoryName(fullPath);
            watcher.Filter = Path.GetFileName(fullPath);

            // Add event handlers.
            watcher.Changed += SourceChanged;
            watcher.Created += SourceChanged;

            // Begin watching.

            watcher.EnableRaisingEvents = true;

            Console.WriteLine("BitmapLayer (2): watcher loaded!");
        }

        private void SourceChanged(object sender, FileSystemEventArgs e)
        {
            Console.WriteLine("BitmapLayer (3): SourceChanged");
            watcher.EnableRaisingEvents = false;
            while (!FileDialog.IsFileReady(e.FullPath)) { }

            UpdateSource(ImageHandler.ForceSilentlyReadBitmap(e.FullPath));
            
            watcher.EnableRaisingEvents = true;

            // Somehow call the draw method
            App.Current.Dispatcher.Invoke(() =>
            {
                ParentLayeredImage.Draw();
            });
        }
    }
}
