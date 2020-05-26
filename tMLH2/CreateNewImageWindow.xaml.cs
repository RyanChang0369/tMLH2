using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace tMLH2
{
    /// <summary>
    /// Interaction logic for CreateNewImageWindow.xaml
    /// </summary>
    public partial class CreateNewImageWindow : EditorInterface
    {
        public Bitmap CreatedBitmap { get; set; }

        private System.Drawing.Rectangle rectangle;

        public CreateNewImageWindow()
        {
            InitializeComponent();
            UpdateComponent();
            DataContext = rectangle;
        }

        protected override void SafelyExit()
        {
            if (rectangle.Width < 1)
                rectangle.Width = 1;

            if (rectangle.Height < 1)
                rectangle.Height = 1;

            CreatedBitmap = new Bitmap(rectangle.Width, rectangle.Height);
        }

        protected override void UpdateComponent()
        {
            if (rectangle == null || rectangle.Width == 0 || rectangle.Height == 0)
                rectangle = new System.Drawing.Rectangle(0, 0, 48, 48);
        }

        protected override void AddRequiredControls()
        {
            RequiredControls.Add(WidthTB);
            RequiredControls.Add(HeightTB);
        }

        protected override void ClearVariable()
        {
            throw new NotImplementedException();
        }
    }
}
