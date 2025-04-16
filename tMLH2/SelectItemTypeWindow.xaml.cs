using System;
using System.Collections.Generic;
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
    /// Interaction logic for SelectItemTypeWindow.xaml
    /// </summary>
    public partial class SelectItemTypeWindow : EditorInterface
    {
        public MainWindow.ItemType ItemType { get; set; }

        public SelectItemTypeWindow()
        {
            InitializeComponent();
            UpdateComponent();
            DataContext = this;
        }

        protected override void AddRequiredControls()
        {
            
        }

        protected override void ClearVariable()
        {
            throw new NotImplementedException();
        }

        protected override void UpdateComponent()
        {
            
        }
    }
}
