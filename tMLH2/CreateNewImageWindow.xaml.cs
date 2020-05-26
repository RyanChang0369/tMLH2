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
        public static Bitmap CreatedBitmap { get; set; }
        public CreateNewImageWindow()
        {
            InitializeComponent();
            UpdateComponent();
            DataContext = CreatedBitmap;
            
        }

        protected override void UpdateComponent()
        {
            if (CreatedBitmap == null)
                CreatedBitmap = new Bitmap(48, 48);
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
