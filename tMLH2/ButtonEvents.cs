using System.Windows;
using System;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog();

            if (dialog.Path == null)
                return;

            LayeredImage.Push(dialog.Path);

            InitiateLayersStackPanel();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog();

            if (dialog.Path == null)
                return;

            try
            {
                ImageHandler.WriteBitmap(dialog.Path, LayeredImage.SelectedBitmapLayer.Source);
                MessageBox.Show("Image saved!");
            }
            catch (ArgumentOutOfRangeException)
            {
                
            }
        }
    }
}
