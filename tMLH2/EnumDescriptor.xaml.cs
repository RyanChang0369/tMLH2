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
    /// Interaction logic for EnumDescriptor.xaml
    /// </summary>
    public partial class EnumDescriptor : Window
    {
        public EnumDescriptor()
        {
            InitializeComponent();
        }

        public EnumDescriptor(Array enumValues)
        {
            InitializeComponent();

            for (int i = 0; i < enumValues.Length; i++)
            {
                Enum enumValue = (Enum)enumValues.GetValue(i);
                string description = enumValue.GetDescription();

                TextBlock enumHeader = new TextBlock
                {
                    Text = enumValue.ToString(),
                    FontSize = 28,
                    Margin = new Thickness(10)
                };
                TextBlock enumDescription = new TextBlock
                {
                    Text = description,
                    FontSize = 18,
                    TextWrapping = TextWrapping.Wrap,
                    Margin = new Thickness(20, 0, 0, 10)
                };


                StackPanel enumPanel = new StackPanel();
                enumPanel.Children.Add(enumHeader);
                enumPanel.Children.Add(enumDescription);

                InfoPanel.Children.Add(enumPanel);
            }
        }
    }
}
