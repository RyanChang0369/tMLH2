using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Input;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private Bitmap CloneBitmap(Bitmap img)
        {
            Bitmap img2 = new Bitmap(img.Width, img.Height);
            using (Graphics g = Graphics.FromImage(img2))
                g.DrawImageUnscaled(img, 0, 0);
            return img2;
        }

        private void ImageControl_MouseDown(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            try
            {
                Bitmap bitmap = CloneBitmap(LayeredImage.SelectedBitmapLayer.Source); //Needed to avoid OutOfMemoryException
                System.Windows.Point mouseLocation = Mouse.GetPosition(ImageControl);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.DrawRectangle(new Pen(Color.Black, 10), (float)mouseLocation.X, (float)mouseLocation.Y, 10, 10);
                }

                LayeredImage.SelectedBitmapLayer.Source = bitmap;
                LayeredImage.Draw();
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        private void ImageControl_MouseMove(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }

        private void ImageControl_MouseLeave(object sender, System.Windows.Input.MouseEventArgs e)
        {

        }
    }
}
