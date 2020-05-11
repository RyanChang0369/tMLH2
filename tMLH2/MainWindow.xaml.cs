using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace tMLH2
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public static double zoom = 1;

        public enum UnderlayColor
        {
            White,
            Black,
            Green,
            Blue,
            Red
        }

        public enum UnderlayGender
        {
            Male = 10,
            Female = 20
        }

        LayeredImage LayeredImage;

        public MainWindow()
        {
            InitializeComponent();
            
            ImageHandler.InitiateImageHandler();
            InitiateImage();
            InitiateLayersStackPanel();
            InitializeShortcuts();
        }

        public void InitiateImage()
        {
            LayeredImage = new LayeredImage(Properties.Resources.player10, ImageControl, NextPaneButton, PrevPaneButton, 40, 56);
        }

        private void CheckColorAndGenderOptions(object sender, RoutedEventArgs e)
        {
            int options = 0;
            try
            {
                foreach (RadioButton radioColor in ColorOptionsStackPanel.Children)
                {
                    if ((bool)radioColor.IsChecked)
                    {
                        options += Int32.Parse(radioColor.Name.Substring(radioColor.Name.Length - 1));
                        break;
                    }
                }
            }
            catch (NullReferenceException) { return; }

            try
            {
                foreach (RadioButton radioGender in GenderOptionsStackPanel.Children)
                {
                    if ((bool)radioGender.IsChecked)
                    {
                        options += Int32.Parse(radioGender.Name.Substring(radioGender.Name.Length - 2));
                        break;
                    }
                }
            }
            catch (NullReferenceException) { return; }

            try
            {
                LayeredImage.SetBaseSource(Properties.Resources.ResourceManager.
                GetObject("player" + options, Properties.Resources.Culture) as System.Drawing.Bitmap,
                (int)LayeredImage.BaseImageIndexes.PlayerModel);
            }
            catch (NullReferenceException) { return; }
        }

        private void ColorPicker_SelectedColorChanged(object sender, RoutedPropertyChangedEventArgs<Color?> e)
        {
            //Test
            Color tempColor = (Color)ColorPicker.SelectedColor;
            BrushColor = tempColor.ToDrawingColor();
        }

        private void ImageOptions_Checked(object sender, RoutedEventArgs e)
        {
            RadioButton radioButton = (RadioButton)sender;

            try
            {
                switch (radioButton.Name)
                {
                    case "ImageOptions_RadioButtonArmor":
                        LayeredImage.Mode = LayeredImage.DrawingMode.Armor;
                        break;
                    case "ImageOptions_RadioButtonItem":
                        LayeredImage.Mode = LayeredImage.DrawingMode.Item;
                        ImageControl.Width = 500;
                        break;
                }

                LayeredImage.Draw();
            }
            catch (NullReferenceException)
            {

            }

        }

        private void ZoomSlider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = (Slider)sender;

            double zoomValue = slider.Value / 100;
            zoom = zoomValue;

            ImageControl.RenderTransform = new ScaleTransform(zoomValue, zoomValue);
            try
            {
                //LayeredImage.Draw();
            }
            catch (NullReferenceException)
            {

            }
        }
    }
}
