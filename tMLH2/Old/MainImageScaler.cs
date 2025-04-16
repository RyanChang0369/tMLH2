using System.Windows;
using System.Windows.Controls;
using System.Windows.Media;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private void Window_SizeChanged(object sender, SizeChangedEventArgs e)
        {
            //UpdateMainImageScale();
        }

        //private void UpdateMainImageScale()
        //{
        //    double gridSizeX = (ImageContainer as Grid).ActualWidth;
        //    double gridSizeY = (ImageContainer as Grid).ActualHeight;

        //    for (int i = 0; i < ScaleTransforms.Count; i++)
        //    {
        //        double frameSizeX = (ImageElements[i] as Image).ActualWidth;
        //        double frameSizeY = (ImageElements[i] as Image).ActualHeight;

                
        //        (ScaleTransforms[i] as ScaleTransform).CenterX = gridSizeX / 2;
        //        (ScaleTransforms[i] as ScaleTransform).CenterY = gridSizeY / 2;

        //        (ScaleTransforms[i] as ScaleTransform).ScaleX = 2;
        //        (ScaleTransforms[i] as ScaleTransform).ScaleY = 2;
        //    }
        //}
    }
}
