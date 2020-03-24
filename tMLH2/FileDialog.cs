using Microsoft.Win32;
using System;
using System.IO;
using System.Threading;

namespace tMLH2
{
    public class FileDialog
    {
        public string Path { get; set; }

        public FileDialog()
        {
            OpenFileDialog fileDialog = new OpenFileDialog();
            fileDialog.Multiselect = false;
            if (fileDialog.ShowDialog() == true)
            {
                Path = fileDialog.FileName;
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