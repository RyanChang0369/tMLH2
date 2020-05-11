﻿using System.Windows;
using System;
using System.Windows.Controls;
using System.Windows.Shapes;
using System.Drawing;
using System.Windows.Input;
using System.Collections.Generic;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        private bool IsDrawing = false;
        
        private void ImageControl_MouseDown(object sender, MouseButtonEventArgs e)
        {
            if (IsEydropping)
            {
                UserEyedrops();
            }
            else
            {
                try
                {
                    UndoHistory.Add(new Bitmap(LayeredImage.SelectedBitmapLayer.Source));
                }
                catch (ArgumentOutOfRangeException)
                { }

                IsDrawing = true;
                UserDrawsOnBitmap();
                RedoHistory = new List<Bitmap>(MaxItemsInHistory);
            }
        }

        private void ImageControl_MouseMove(object sender, MouseEventArgs e)
        {
            if (IsEydropping)
            {

            }
            else
            {
                if (e.LeftButton == MouseButtonState.Pressed)
                {
                    UserDrawsOnBitmap();
                }
                else
                {
                    IsDrawing = false;
                }
            }
        }

        private void ImageControl_MouseLeave(object sender, MouseEventArgs e)
        {
            
        }

        /// <summary>
        /// Run when user uses the eyedropper tool and clicks on the image.
        /// </summary>
        private void UserEyedrops()
        {
            try
            {
                Bitmap bitmap = new Bitmap(LayeredImage.SelectedBitmapLayer.Source);

                System.Windows.Point mouseLocation = Mouse.GetPosition(ImageControl);

                double xScale = mouseLocation.X / (ImageControl.ActualWidth / LayeredImage.FramePixelWidth);
                double yScale = mouseLocation.Y / (ImageControl.ActualHeight / LayeredImage.FramePixelHeight);

                int x = (int)Math.Round(xScale);
                int y = (int)Math.Round(yScale) + LayeredImage.CurrentFrame * LayeredImage.FramePixelHeight;

                int xCentered = x - (int)Math.Round((double)BrushSize / 2);
                int yCentered = y - (int)Math.Round((double)BrushSize / 2);

                ColorPicker.SelectedColor = ImageHandler.ToMediaColor(bitmap.GetPixel(xCentered, yCentered));

                EyedropperCheckBox.IsChecked = false;
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Please select a layer.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
        }

        /// <summary>
        /// Run when the user draws on the ImageControl.
        /// </summary>
        private void UserDrawsOnBitmap()
        {
            if (!IsDrawing)
                return;

            try
            {
                Bitmap bitmap = new Bitmap(LayeredImage.SelectedBitmapLayer.Source);

                System.Windows.Point mouseLocation = Mouse.GetPosition(ImageControl);

                double xScale = 0;
                double yScale = 0;

                if (LayeredImage.Mode == LayeredImage.DrawingMode.Armor)
                {
                    xScale = mouseLocation.X / (ImageControl.ActualWidth / LayeredImage.FramePixelWidth);
                    yScale = mouseLocation.Y / (ImageControl.ActualHeight / LayeredImage.FramePixelHeight);
                }
                else
                {
                    xScale = mouseLocation.X / (ImageControl.ActualWidth / LayeredImage.BitmapLayers[LayeredImage.CurrentFrame].Source.Width);
                    yScale = mouseLocation.Y / (ImageControl.ActualHeight / LayeredImage.BitmapLayers[LayeredImage.CurrentFrame].Source.Height);
                }

                int x = (int)Math.Round(xScale);
                int y = (int)Math.Round(yScale) + LayeredImage.CurrentFrame * LayeredImage.FramePixelHeight;

                int xCentered = x - (int)Math.Round((double)BrushSize / 2);
                int yCentered = y - (int)Math.Round((double)BrushSize / 2);

                using (Graphics g = Graphics.FromImage(bitmap))
                {
                    g.CompositingMode = System.Drawing.Drawing2D.CompositingMode.SourceCopy;
                    if (IsPainting)
                        g.FillRectangle(SolidBrushBrush, xCentered, yCentered, BrushSize, BrushSize);
                    else
                        g.FillRectangle(TransparentBrush, xCentered, yCentered, BrushSize, BrushSize);
                }

                LayeredImage.SelectedBitmapLayer.UpdateSource(bitmap);

                LayeredImage.Draw();
            }
            catch (ArgumentOutOfRangeException)
            {
                MessageBox.Show("Please select a layer.", "Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }
            catch (IndexOutOfRangeException e)
            {
                Console.WriteLine(e);
            }
        }
    }
}
