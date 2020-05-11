using System.Windows;
using System;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog((int)FileDialog.DialogOptions.Open);

            if (dialog.OpenPaths == null)
                return;

            foreach (string path in dialog.OpenPaths)
                LayeredImage.Push(path);

            InitiateLayersStackPanel();
            LayersStackPanel.SelectedIndex = LayersStackPanel.Items.Count - 1;
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog((int)FileDialog.DialogOptions.Save);

            if (dialog.SavePath == null)
                return;

            try
            {
                ImageHandler.WriteBitmap(dialog.SavePath, LayeredImage.SelectedBitmapLayer.Source);
                MessageBox.Show("Image saved!");
            }
            catch (ArgumentOutOfRangeException)
            {
                
            }
        }
    }
}
