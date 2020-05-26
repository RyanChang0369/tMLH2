using System.Windows;
using System;
using System.Drawing;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog(FileDialog.DialogOptions.Open);

            if (dialog.OpenPaths == null)
                return;

            foreach (string path in dialog.OpenPaths)
                LayeredImage.Push(path);

            InitiateLayersStackPanel();
            LayersStackPanel.SelectedIndex = LayersStackPanel.Items.Count - 1;
            LayeredImage.Draw();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog(FileDialog.DialogOptions.Save);

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

        private void NewButton_Click(object sender, RoutedEventArgs e)
        {
            SelectItemTypeWindow itemTypeWindow = new SelectItemTypeWindow();
            itemTypeWindow.ShowDialog();
            ItemType itemType = itemTypeWindow.ItemType;

            Bitmap bitmap;

            if (IsArmorLike(itemType))
                bitmap = new Bitmap(40, 1120);
            else
            {
                CreateNewImageWindow imageWindow = new CreateNewImageWindow();
                imageWindow.ShowDialog();
                bitmap = imageWindow.CreatedBitmap;
            }

            FileDialog fileDialog = new FileDialog(FileDialog.DialogOptions.Save);

            if (fileDialog.SavePath == null)
                return;

            LayeredImage.Push(bitmap, itemType, fileDialog.SavePath, true);
            InitiateLayersStackPanel();
        }
    }
}
