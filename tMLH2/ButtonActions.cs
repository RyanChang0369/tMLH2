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

        private void UploadWeaponRadio_Checked(object sender, RoutedEventArgs e)
        {
            LayeredImage.SetBaseSource(Properties.Resources.playerArms, (int)LayeredImage.BaseImageIndexes.ArmModel);
        }

        private void UploadArmorRadio_Checked(object sender, RoutedEventArgs e)
        {
            try
            {
                LayeredImage.SetBaseSource(Properties.Resources.blank, (int)LayeredImage.BaseImageIndexes.ArmModel);
            }
            catch (NullReferenceException eeeee)
            {
                Console.WriteLine(eeeee);
            }
        }
    }
}
