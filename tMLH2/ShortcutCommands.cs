using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;
using System.IO;
using System.Threading;
using System.Drawing;
using System.Collections.Generic;
using System.Windows.Input;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private static readonly int MaxItemsInHistory = 50;
        private List<Bitmap> UndoHistory = new List<Bitmap>(MaxItemsInHistory);
        private List<Bitmap> RedoHistory = new List<Bitmap>(MaxItemsInHistory);

        public static RoutedCommand Undo = new RoutedCommand();
        public static RoutedCommand Redo = new RoutedCommand();
        public static RoutedCommand Save = new RoutedCommand();
        public static RoutedCommand Upload = new RoutedCommand();

        private void InitializeShortcuts()
        {
            Undo.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            Redo.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));
            Save.InputGestures.Add(new KeyGesture(Key.S, ModifierKeys.Control));
            Upload.InputGestures.Add(new KeyGesture(Key.U, ModifierKeys.Control));

            CommandBindings.Add(new CommandBinding(Undo, ExecuteUndo));
            CommandBindings.Add(new CommandBinding(Redo, ExecuteRedo));
            CommandBindings.Add(new CommandBinding(Save, ExecuteSave));
            CommandBindings.Add(new CommandBinding(Upload, ExecuteUpload));
        }

        private void ExecuteUndo(object sender, ExecutedRoutedEventArgs e)
        {
            while (UndoHistory.Count > MaxItemsInHistory)
            {
                UndoHistory.RemoveAt(0);
            }

            if (UndoHistory.Count == 0)
                return;

            try
            {
                RedoHistory.Add(LayeredImage.SelectedBitmapLayer.Source);
                Bitmap bitmap = UndoHistory[UndoHistory.Count - 1];
                LayeredImage.SelectedBitmapLayer.UpdateSource(bitmap);
                UndoHistory.RemoveAt(UndoHistory.Count - 1);
                LayeredImage.Draw();
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        private void ExecuteRedo(object sender, ExecutedRoutedEventArgs e)
        {
            while (RedoHistory.Count > MaxItemsInHistory)
            {
                RedoHistory.RemoveAt(0);
            }

            if (RedoHistory.Count == 0)
                return;

            try
            {
                UndoHistory.Add(LayeredImage.SelectedBitmapLayer.Source);
                Bitmap bitmap = RedoHistory[RedoHistory.Count - 1];
                LayeredImage.SelectedBitmapLayer.UpdateSource(bitmap);
                RedoHistory.RemoveAt(RedoHistory.Count - 1);
                LayeredImage.Draw();
            }
            catch (ArgumentOutOfRangeException)
            {
                return;
            }
        }

        private void ExecuteUpload(object sender, ExecutedRoutedEventArgs e)
        {
            UploadButton_Click(null, null);
        }

        private void ExecuteSave(object sender, ExecutedRoutedEventArgs e)
        {
            SaveButton_Click(null, null);
        }


        private void Window_KeyDown(object sender, KeyEventArgs e)
        {
            if (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.P)
                ExecuteSelectPaint();
            else if (Keyboard.Modifiers == ModifierKeys.None && e.Key == Key.E)
                ExecuteSelectErase();
        }

        private void ExecuteSelectPaint()
        {
            SelectPaintRadioBtn.IsChecked = true;
            SelectEraseRadioBtn.IsChecked = false;
        }

        private void ExecuteSelectErase()
        {
            SelectPaintRadioBtn.IsChecked = false;
            SelectEraseRadioBtn.IsChecked = true;
        }
    }
}
