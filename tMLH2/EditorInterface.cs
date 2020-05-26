using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;

namespace tMLH2
{
    public abstract partial class EditorInterface : Window
    {
        protected List<Control> RequiredControls { get; set; }

        protected void Save(object sender, RoutedEventArgs e)
        {
            RequiredControls = new List<Control>();
            AddRequiredControls();

            if (!FormIsValid())
            {
                MessageBox.Show("Please fill out all required forms", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }
            SafelyExit();
            Close();
        }

        protected void Quit(object sender, RoutedEventArgs e)
        {
            ClearVariable();
            Close();
        }

        /// <summary>
        /// Anything the program needs to do before navigating to CreateMaPrompt can be done here.
        /// </summary>
        protected virtual void SafelyExit()
        {

        }

        protected abstract void AddRequiredControls();

        protected abstract void UpdateComponent();

        protected bool FormIsValid()
        {
            foreach (Control control in RequiredControls)
            {
                if (!control.IsValid())
                    return false;
            }
            return true;
        }

        protected abstract void ClearVariable();

        protected void ComboBox_Initialize(object sender, RoutedEventArgs e)
        {
            ComboBox comboBox = (ComboBox)sender;

            List<ComboBoxItem> comboBoxItems = new List<ComboBoxItem>();

            foreach (Enum enumValue in comboBox.Items)
            {
                string tooltip = enumValue.GetDescription();

                ComboBoxItem comboBoxItem = new ComboBoxItem
                {
                    Content = enumValue,
                    ToolTip = tooltip
                };

                comboBoxItems.Add(comboBoxItem);
            }


            comboBox.ItemsSource = null;

            foreach (ComboBoxItem item in comboBoxItems)
            {
                comboBox.Items.Add(item);
            }
        }


        protected void InfoLoaded_EnumDescription(object sender, RoutedEventArgs e)
        {
            Image imgElement = (Image)sender;
            imgElement.ToolTip = "Click to open description for the below Combo Box.";

            imgElement.MouseUp += InfoMouseUp_EnumDescription;
        }

        private void InfoMouseUp_EnumDescription(object sender, MouseEventArgs e)
        {
            Image imgElement = (Image)sender;

            if (e.LeftButton == MouseButtonState.Released)
            {
                EnumDescriptor descriptor = new EnumDescriptor((Array)imgElement.Tag);
                descriptor.Show();
            }
        }
    }
}
