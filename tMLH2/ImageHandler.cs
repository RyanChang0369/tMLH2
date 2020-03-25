using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Threading;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace tMLH2
{
    public static class ImageHandler
    {
        enum ImageOverlayIndexes
        {
            Pants, Torso, Helm, Weapon
        }

        //public static BitmapImage[] BitmapImageOverlays = new BitmapImage[4];
        public static BitmapImage BitmapImageUnderlay;
        public static BitmapImage BitmapImageOverlay;
        public static BitmapImage BitmapImageArm;

        public static void InitiateImageHandler()
        {
            BitmapImageUnderlay = Properties.Resources.player10.ToBitmapImage();
            BitmapImageOverlay = Properties.Resources.blank.ToBitmapImage();
            BitmapImageArm = Properties.Resources.blank.ToBitmapImage();

            /*for (int i = 0; i < BitmapImageOverlays.Length; i++)
                BitmapImageOverlays[i] = Properties.Resources.blank.ToBitmapImage();*/
        }

        public static BitmapImage ToBitmapImage(this Bitmap bitmap)
        {
            using (var memory = new MemoryStream())
            {
                bitmap.Save(memory, ImageFormat.Png);
                memory.Position = 0;

                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.StreamSource = memory;
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.EndInit();
                bitmapImage.Freeze();

                return bitmapImage;
            }
        }

        public static Bitmap ToBitmap(this BitmapImage bitmapImage)
        {
            using (MemoryStream outStream = new MemoryStream())
            {
                BitmapEncoder enc = new BmpBitmapEncoder();
                enc.Frames.Add(BitmapFrame.Create(bitmapImage));
                enc.Save(outStream);
                System.Drawing.Bitmap bitmap = new System.Drawing.Bitmap(outStream);

                return new Bitmap(bitmap);
            }
        }

        public static Int32Rect GetCropRect(int frames, int startFrame, BitmapImage bmpImg)
        {
            int width = bmpImg.PixelWidth;
            int height = bmpImg.PixelHeight;

            try
            {
                if (height % frames != 0)
                {
                    Console.WriteLine("ImageHandler (1): (height % frames != 0)");
                }

                int croppedHeight = height / frames;

                //Console.WriteLine("ImageHandler (2): croppedHeight: " + croppedHeight);

                return new Int32Rect(0, startFrame * croppedHeight, width, croppedHeight);
            }
            catch (DivideByZeroException)
            {
                return new Int32Rect(0, 0, width, height);
            }
        }

        public static Rectangle GetCropRect(int frames, int startFrame, Bitmap bmp)
        {
            int width = bmp.Width;
            int height = bmp.Height;

            try
            {
                if (height % frames != 0)
                {
                    Console.WriteLine("ImageHandler (3): (height % frames != 0) " + (height / frames));
                }

                int croppedHeight = (height / frames);

                //Console.WriteLine("ImageHandler (4): croppedHeight: " + croppedHeight);

                return new Rectangle(0, startFrame * croppedHeight, width, croppedHeight);
            }
            catch (DivideByZeroException)
            {
                return new Rectangle(0, 0, width, height);
            }
        }

        public static Int32Rect GetCropRectWithHeight(int croppedHeight, int startFrame, BitmapImage bmpImg)
        {
            int width = bmpImg.PixelWidth;
            int height = bmpImg.PixelHeight;

            try
            {
                return new Int32Rect(0, startFrame * croppedHeight, width, croppedHeight);
            }
            catch (DivideByZeroException)
            {
                return new Int32Rect(0, 0, width, height);
            }
        }

        public static Rectangle GetCropRect(BitmapLayer bmpLayer, int currentFrame)
        {
            return GetCropRect(bmpLayer.NumberOfFrames, currentFrame, bmpLayer.Source);
        }

        public static Rectangle GetDrawRect(Bitmap bitmap)
        {
            return new Rectangle(0, 0, bitmap.Width, bitmap.Height);
        }

        public static byte[] BitmapImageToByteArray(this BitmapImage bitmapImage)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            encoder.Frames.Add(BitmapFrame.Create(bitmapImage));

            using (MemoryStream ms = new MemoryStream())
            {
                encoder.Save(ms);
                return ms.ToArray();
            }
        }

        /// <summary>
        /// Tries to read a BitmapImage without denying access from other programs
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static BitmapImage SilentlyReadBitmapImage(string fullPath)
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

        /// <summary>
        /// Tries to read a Bitmap without denying access from other programs
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static Bitmap SilentlyReadBitmap(string fullPath)
        {
            using (FileStream fileStream = new FileStream(
                fullPath, FileMode.Open, FileAccess.Read, FileShare.Read))
            {
                return new Bitmap(fileStream);
            }
        }

        /// <summary>
        /// Tries to silently read a Bitmap even if the bitmap is locked by another program
        /// </summary>
        /// <param name="fullPath"></param>
        /// <returns></returns>
        public static Bitmap ForceSilentlyReadBitmap(string fullPath)
        {
            try
            {
                return SilentlyReadBitmap(fullPath);
            }
            catch (Exception)
            {
                Thread.Sleep(10);
                return ForceSilentlyReadBitmap(fullPath);
            }
        }

        public static void WriteBitmap(string fullPath, Bitmap bitmap)
        {
            try
            {
                if (File.Exists(fullPath))
                {
                    MessageBoxResult result =MessageBox.Show(fullPath + " exists. Overwrite?",
                        "File Exists", MessageBoxButton.YesNoCancel, MessageBoxImage.Warning);
                    if (result != MessageBoxResult.Yes)
                        return;
                }
                bitmap.Save(fullPath);
            }
            catch (ArgumentNullException)
            {
                MessageBox.Show("Please select a file.");
            }
        }
        
        public static Bitmap CropBitmap(Bitmap bitmap, Rectangle cropArea)
        {
            return bitmap.Clone(cropArea, bitmap.PixelFormat);
        }

        /// <summary>
        /// Makes the Bitmap 1120 pixels in height
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static Bitmap FixBitmap(Bitmap bitmap)
        {
            Bitmap underlay = new Bitmap(bitmap.Width, 1120);
            //underlay.SetResolution(bitmap.HorizontalResolution, bitmap.VerticalResolution);

            using (Graphics g = Graphics.FromImage(underlay))
            {
                g.Clear(System.Drawing.Color.Transparent);
                g.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
                g.DrawImage(bitmap, new Rectangle(0, 0, bitmap.Width, bitmap.Height));
                //g.DrawImage(bitmap, new Rectangle(0, 0, underlay.Width, underlay.Height),
                //new Rectangle(0, 0, bitmap.Width, bitmap.Height), GraphicsUnit.Pixel);
            }

            return underlay;
        }
    }
}