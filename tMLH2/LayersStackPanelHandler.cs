using System;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private Button ButtonJustDeleted;

        public void InitiateLayersStackPanel()
        {
            LayersStackPanel.Items.Clear();

            // For base stack panel (arms not shown)
            //CreateLSPChildren(-1, LayeredImage.
            //    BaseSourceImages[(int)LayeredImage.BaseImageIndexes.PlayerModel].CurrentFrame.ToBitmapImage());

            // For other layers
            for (int i = 0; i < LayeredImage.BitmapLayers.Count; i++)
            {
                CreateLSPChildren(i, LayeredImage.BitmapLayers[i].CurrentFrame.ToBitmapImage());
            }
        }

        /// <summary>
        /// Select an index of -1 if this is the base layer
        /// </summary>
        /// <param name="index"></param>
        /// <param name="thumbnailSource"></param>
        private void CreateLSPChildren(int index, BitmapImage thumbnailSource)
        {           
            StackPanel container = new StackPanel
            {
                Name = "Layer" + index,
                Orientation = Orientation.Horizontal
            };
            Image thumbnail = new Image
            {
                Source = thumbnailSource,
                Width = 50,
                Height = 50,
                Margin = new Thickness(10.0)
            };
            Label label = new Label
            {
                Content = "Layer " + index,
                VerticalContentAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10.0),
                Width = 150,
                FontSize = 18
            };
            Image deleteIcon = new Image
            {
                Source = Properties.Resources.Delete.ToBitmapImage(),
                Cursor = Cursors.Hand
            };
            Button deleteButton = new Button
            {
                Width = 20,
                Height = 20,
                Padding = new Thickness(0),
                Content = deleteIcon,
                ToolTip = "Delete Layer",
                Name = "DeleteButton" + index
            };

            deleteButton.Click += DeleteButton_Click;

            container.Children.Add(thumbnail);
            container.Children.Add(label);
            container.Children.Add(deleteButton);

            LayersStackPanel.Items.Add(container);

            SaveButton.IsEnabled = LayersStackPanel.Items.Count > 1;
        }

        private void LayersStackPanel_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            int index = LayersStackPanel.SelectedIndex;
            LayeredImage.SelectedIndex = index;

            for (int i = 0; i < LayersStackPanel.Items.Count; i++)
            {
                ((StackPanel)LayersStackPanel.Items[i]).Background = new SolidColorBrush(Colors.Transparent);
            }

            try
            {
                ((StackPanel)LayersStackPanel.Items[index]).Background = new SolidColorBrush(Colors.LightSlateGray);
            }
            catch (ArgumentOutOfRangeException)
            {

            }

            LayeredImage.Draw();
        }

        private void DeleteButton_Click(object sender, RoutedEventArgs e)
        {
            Button btn = (Button)sender;
            StackPanel container = (StackPanel)VisualTreeHelper.GetParent(btn);
            ButtonJustDeleted = btn;
            int index = LayersStackPanel.Items.IndexOf(container);
            Console.WriteLine("LayersStackPanelHandler (1): btn " + btn.Name);

            if (index + 1 == LayeredImage.SelectedIndex)
            {
                LayeredImage.SelectedIndex = -1;
            }

            LayeredImage.Remove(index);
            InitiateLayersStackPanel();
        }
    }
}
