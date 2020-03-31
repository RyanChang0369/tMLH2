using System.Windows;
using System;
using System.Windows.Controls;
using System.Drawing;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        int BrushSize = 1;
        Color BrushColor = Color.Black;
        Pen BrushPen { get => new Pen(BrushColor, BrushSize); }
        SolidBrush SolidBrushBrush { get => new SolidBrush(BrushColor); }
        readonly SolidBrush TransparentBrush = new SolidBrush(Color.Transparent);

        private bool IsPainting { get => (bool)SelectPaintRadioBtn.IsChecked; }
        private bool IsEydropping { get => (bool)EyedropperCheckBox.IsChecked; }

        private void Slider_ValueChanged(object sender, RoutedPropertyChangedEventArgs<double> e)
        {
            Slider slider = sender as Slider;
            BrushSizeLabel.Content = slider.Value + " Pixel(s)";
            BrushSize = (int)slider.Value;
        }

        private void EyedropperCheckBox_Checked(object sender, RoutedEventArgs e)
        {

        }
    }
}
