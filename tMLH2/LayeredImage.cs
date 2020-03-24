using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Imaging;

namespace tMLH2
{
    public class LayeredImage
    {
        private System.Windows.Controls.Image ImageControl;

        public static int SelectedIndex = -1;

        /// <summary>
        /// Represents the base image for this ImageLayer
        /// </summary>
        public BitmapLayer[] BaseSourceImages = new BitmapLayer[3];

        public enum BaseImageIndexes
        {
            PlayerModel, ArmModel, Reserved
        }

        /// <summary>
        /// A list of all the layers in this image
        /// </summary>
        public List<BitmapLayer> BitmapLayers;

        private Bitmap FinalImage;

        /// <summary>
        /// The button that increments the current pane that is associated with this object
        /// </summary>
        public Button NextPaneButton;

        /// <summary>
        /// The button that decrements the current pane that is associated with this object
        /// </summary>
        public Button PreviousPaneButton;
        
        public int CurrentFrame;
        

        public LayeredImage(Bitmap baseSourceImage, System.Windows.Controls.Image imageControl,
            Button nextPaneButton, Button prevPaneButton, int frameWidth, int frameHeight)
        {
            BitmapLayers = new List<BitmapLayer>();

            for (int i = 0; i < BaseSourceImages.Length; i++)
            {
                BaseSourceImages[i] = new BitmapLayer(Properties.Resources.blank, this);
            }

            BaseSourceImages[(int)BaseImageIndexes.PlayerModel] =
                new BitmapLayer(baseSourceImage, this);

            ImageControl = imageControl;
            
            CurrentFrame = 0;
            
            NextPaneButton = nextPaneButton;
            PreviousPaneButton = prevPaneButton;

            nextPaneButton.Click += NextPaneButton_Click;
            prevPaneButton.Click += PrevPaneButton_Click;
            FinalImage = new Bitmap(frameWidth, frameHeight);

            Draw();

            TogglePaneButtons();
        }

        public void OnWindowResize()
        {
            FinalImage = new Bitmap((int)ImageControl.ActualWidth, (int)ImageControl.ActualHeight);
            Draw();
        }

        /// <summary>
        /// Updates a base source with the specified index. See enum BaseSourceImageTypes.
        /// </summary>
        public void SetBaseSource(Bitmap baseSource, int index)
        {
            BaseSourceImages[index] = new BitmapLayer(baseSource, this);
            Draw();
        }

        /// <summary>
        /// Push to BitmapLayers. Uses SmartDetection.
        /// </summary>
        /// <param name="fullPath"></param>
        public void Push(string fullPath)
        {
            Bitmap tempBmp = ImageHandler.SilentlyReadBitmap(fullPath);

            int itemType = SmartDetection.DetectType(fullPath, tempBmp.ToBitmapImage());

            Push(tempBmp, itemType,
                SmartDetection.DetectFrameHeight(tempBmp.Height, itemType), fullPath, true);
        }

        /// <summary>
        /// Push to BitmapLayers
        /// </summary>
        /// <param name="bitmapImage"></param>
        /// <param name="itemType"></param>
        /// <param name="frameHeight"></param>
        /// <param name="fullPath"></param>
        /// <param name="useWatcher"></param>
        public void Push(Bitmap bitmapImage, int itemType, int frameHeight, string fullPath, bool useWatcher)
        {
            BitmapLayers.Insert(BitmapLayers.Count,
                new BitmapLayer(bitmapImage, this, itemType, frameHeight, fullPath, useWatcher));
            Draw();
        }

        public void Pop()
        {
            Remove(BitmapLayers.Count);
            Draw();
        }

        public void Remove(int index)
        {
            BitmapLayers.RemoveAt(index);
            Draw();
        }

        public bool Contains(BitmapLayer bitmapLayer)
        {
            return BitmapLayers.Contains(bitmapLayer);
        }

        public int IndexOf(BitmapLayer bitmapLayer)
        {
            return BitmapLayers.IndexOf(bitmapLayer);
        }

        public void Replace(BitmapLayer oldLayer, BitmapLayer newLayer)
        {
            BitmapLayers[BitmapLayers.FindIndex(ind => ind.Equals(oldLayer))] = newLayer;
            Draw();
        }

        public void Replace(int index, BitmapLayer newLayer)
        {
            BitmapLayers[index] = newLayer;
            Draw();
        }

        public void Clear()
        {
            BitmapLayers.Clear();
            Draw();
        }

        public void Draw()
        {
            using (Graphics g = Graphics.FromImage(FinalImage))
            {
                g.Clear(Color.White);

                try
                {
                    for (int i = 0; i < BaseSourceImages.Length; i++)
                    {
                        g.DrawImage(BaseSourceImages[i].GetFrame(CurrentFrame),
                            ImageHandler.GetDrawRect(BaseSourceImages[i].GetFrame(CurrentFrame)));
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e);
                }

                try
                {
                    for (int i = 0; i < BitmapLayers.Count; i++)
                    {
                        g.DrawImage(BitmapLayers[i].GetFrame(CurrentFrame),
                            ImageHandler.GetDrawRect(BitmapLayers[i].GetFrame(CurrentFrame)));
                    }
                }
                catch (ArgumentOutOfRangeException e)
                {
                    Console.WriteLine(e);
                }
            }

            try
            {
                ImageControl.Source = null;
                ImageControl.Source = FinalImage.ToBitmapImage();
            }
            catch (InvalidOperationException e)
            {
                //Freeze?
                App.Current.Dispatcher.Invoke(() =>
                {
                    ImageControl.Source = FinalImage.ToBitmapImage();
                });

                Console.WriteLine(e);
            }
        }

        private void PrevPaneButton_Click(object sender, RoutedEventArgs e)
        {
            DecrementFrame();
        }

        private void NextPaneButton_Click(object sender, RoutedEventArgs e)
        {
            IncrementFrame();
        }

        public void IncrementFrame()
        {
            CurrentFrame++;
            TogglePaneButtons();
            Draw();
        }

        public void DecrementFrame()
        {
            CurrentFrame--;
            TogglePaneButtons();
            Draw();
        }
        
        public void TogglePaneButtons()
        {
            if (CurrentFrame == 0)
                PreviousPaneButton.IsEnabled = false;
            else
                PreviousPaneButton.IsEnabled = true;
            if (CurrentFrame >= 19 /*Or however many frames there are - 1*/)
                NextPaneButton.IsEnabled = false;
            else
                NextPaneButton.IsEnabled = true;
        }
    }
}