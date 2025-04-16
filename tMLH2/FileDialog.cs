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
            OpenSingle = 2,
            OpenMultiple = 3
        }
        public string SinglePath;
        public string[] MutliplePaths;

        public FileDialog(DialogOptions dialogOption)
        {
            switch (dialogOption)
            {
                case DialogOptions.OpenMultiple:
                    OpenFileDialog openFileDialog = new OpenFileDialog();
                    openFileDialog.Multiselect = true;
                    if (openFileDialog.ShowDialog() == true)
                    {
                        MutliplePaths = openFileDialog.FileNames;
                    }
                    break;
                case DialogOptions.Save:
                    SaveFileDialog saveFileDialog = new SaveFileDialog();
                    if (saveFileDialog.ShowDialog() == true)
                    {
                        SinglePath = saveFileDialog.FileName;
                    }
                    break;
                case DialogOptions.OpenSingle:
                    OpenFileDialog openFileDialog1 = new OpenFileDialog();
                    openFileDialog1.Multiselect = false;
                    if (openFileDialog1.ShowDialog() == true)
                    {
                        SinglePath = openFileDialog1.FileName;
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