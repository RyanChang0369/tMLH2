using System.Windows;
using System;
using System.Drawing;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog(FileDialog.DialogOptions.OpenMultiple);

            if (dialog.MutliplePaths == null)
                return;

            foreach (string path in dialog.MutliplePaths)
                LayeredImage.Push(path);

            InitiateLayersStackPanel();
            LayersStackPanel.SelectedIndex = LayersStackPanel.Items.Count - 1;
            LayeredImage.Draw();
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog(FileDialog.DialogOptions.Save);

            if (dialog.SinglePath == null)
                return;

            try
            {
                ImageHandler.WriteBitmap(dialog.SinglePath, LayeredImage.SelectedBitmapLayer.Source);
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

            if (itemType == ItemType.Static)
            {
                MessageBox.Show("Cannot set type of item as static.");
                return;
            }

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

            if (fileDialog.SinglePath == null)
                return;

            LayeredImage.Push(bitmap, itemType, fileDialog.SinglePath, true);
            InitiateLayersStackPanel();

            SaveButton.IsEnabled = true;
        }
    }
}
