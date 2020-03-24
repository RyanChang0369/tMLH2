using System.Windows;
using System;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private void UploadButton_Click(object sender, RoutedEventArgs e)
        {
            FileDialog dialog = new FileDialog();
            Console.WriteLine("ButtonActions.xaml.cs (1): dialog.Path: " + dialog.Path);

            if (dialog.Path == null)
                return;

            LayeredImage.Push(dialog.Path);

            InitiateLayersStackPanel();
        }
    }
}
