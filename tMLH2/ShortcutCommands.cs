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

        private void InitializeShortcuts()
        {
            Undo.InputGestures.Add(new KeyGesture(Key.Z, ModifierKeys.Control));
            Redo.InputGestures.Add(new KeyGesture(Key.Y, ModifierKeys.Control));

            CommandBindings.Add(new CommandBinding(Undo, ExecuteUndo));
            CommandBindings.Add(new CommandBinding(Redo, ExecuteRedo));
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
    }
}
