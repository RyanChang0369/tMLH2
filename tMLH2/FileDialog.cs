using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;

namespace tMLH2
{
    public class FileDialog
    {
        public enum DialogOptions
        {
            Save = 1,
            Open = 2
        }
        public string SavePath;
        public string[] OpenPaths;

        public FileDialog(int dialogOption)
        {
            switch (dialogOption)
            {
                case (int)DialogOptions.Open:
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = true;
                    if (openFileDialog.ShowDialog() == true)
                    {
                        OpenPaths = openFileDialog.FileNames;
                    }
                    break;
                case (int)DialogOptions.Save:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SavePath = saveFileDialog.FileName;
                    }
                    break;
            }
            
        }

        public static bool IsFileReady(string filename)
        {
            Thread.Sleep(5);
            try
            {
                using (FileStream inputStream = File.Open(filename, FileMode.Open, FileAccess.Read, FileShare.ReadWrite))
                {
                    //int i = inputStream.ReadByte();
                    return true;
                }
            }
            catch (Exception)
            {
                Console.WriteLine("Not ready");
                return false;
            }
        }
    }
}