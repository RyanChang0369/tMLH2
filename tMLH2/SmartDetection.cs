﻿using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace tMLH2
{
    public static class SmartDetection
    {
        /// <summary>
        /// Tries to detect the type of object from its file and directory name.
        /// </summary>
        /// <returns>A type from the ItemType enum.</returns>
        public static int DetectType(string fullPath, BitmapImage bmpImg)
        {
            int selectedOption;

            string imageFileName = Path.GetFileName(fullPath);
            string beforeUnderscore;
            string afterUnderscore;

            string directoryName = Path.GetDirectoryName(fullPath);
            string localFolderName = directoryName.Substring(directoryName.LastIndexOf("\\") + 1);

            if (imageFileName.Contains('_') && imageFileName.LastIndexOf('_') != imageFileName.Length)
            {
                afterUnderscore = imageFileName.Substring(imageFileName.LastIndexOf("_") + 1,
                    imageFileName.LastIndexOf('.') - (imageFileName.LastIndexOf("_") + 1));
                beforeUnderscore = imageFileName.Substring(0, imageFileName.LastIndexOf("_"));
            }
            else
            {
                beforeUnderscore = imageFileName.Substring(0, imageFileName.LastIndexOf('.'));
                afterUnderscore = "";
            }
                try
                {
                    string fileContents = RemoveComments(
                        FileMethods.SilentlyReadAllLines(
                            directoryName + "\\" + beforeUnderscore + ".cs"));

                    // Try to get ItemType from C# file associated with the image.
                    selectedOption = DetectByMethodNames(fileContents, bmpImg.PixelHeight);
                    if (selectedOption > 0)
                        return selectedOption;

                    // Try to get item type from name of image
                }
                catch (Exception e)
                {
                    Console.WriteLine(e);
                }

                switch (afterUnderscore)
                {
                    case ("Head"):
                        return (int)MainWindow.ItemType.Head;
                    case ("Arms"):
                        return (int)MainWindow.ItemType.Arms;
                    case ("Body"):
                        return (int)MainWindow.ItemType.Body;
                    case ("FemaleBody"):
                        return (int)MainWindow.ItemType.FemaleBody;
                    case ("Legs"):
                        return (int)MainWindow.ItemType.Legs;
                }

            return (int)MainWindow.ItemType.Unknown;
        }

        private static int DetectByMethodNames(string fileContents, int pixelHeight)
        {
            // Find UseStyle Methods
            int selectedOption = DetectByUseStyle(fileContents);
            if (selectedOption > 0)
                return selectedOption;

            // Find Armor Methods
            selectedOption = DetectByMethodNames_Armor(fileContents, pixelHeight);
            if (selectedOption > 0)
                return selectedOption;

            // Find Accessory Methods
            selectedOption = DetectByMethodNames_Accessory(fileContents);
            if (selectedOption > 0)
                return selectedOption;

            return (int)MainWindow.ItemType.Unknown;
        }

        private static int DetectByUseStyle(string fileContents)
        {
            fileContents = fileContents.RemoveSpaces();
            if (fileContents.Contains("item.useStyle=1"))
                return (int)MainWindow.ItemType.Swing;
            else if (fileContents.Contains("item.useStyle=2"))
                return (int)MainWindow.ItemType.Eat;
            else if (fileContents.Contains("item.useStyle=3"))
                return (int)MainWindow.ItemType.Stab;
            else if (fileContents.Contains("item.useStyle=4"))
                return (int)MainWindow.ItemType.HoldUp;
            else if (fileContents.Contains("item.useStyle=5"))
                return (int)MainWindow.ItemType.HoldOut;
            else
                return (int)MainWindow.ItemType.Unknown;
        }

        /// <summary>
        /// Attempts to determine ItemType with C# file and pixelHeight
        /// </summary>
        /// <param name="fileContents"></param>
        /// <param name="pixelHeight"></param>
        /// <returns></returns>
        private static int DetectByMethodNames_Armor(string fileContents, int pixelHeight)
        {
            // Find Armor Methods
            if (pixelHeight == 1120 || pixelHeight == 1118)
            {
                if (fileContents.Contains("[AutoloadEquip(EquipType.Head)]"))
                    return (int)MainWindow.ItemType.Head;
                else if (fileContents.Contains("[AutoloadEquip(EquipType.Body)]"))
                    return (int)MainWindow.ItemType.Body;
                else if (fileContents.Contains("[AutoloadEquip(EquipType.Legs)]"))
                    return (int)MainWindow.ItemType.Legs;
                else
                    return (int)MainWindow.ItemType.Armor;
            }
            else
                return (int)MainWindow.ItemType.Unknown;
        }

        /// <summary>
        /// Attempts to determine ItemType with C# file
        /// </summary>
        /// <param name="fileContents"></param>
        /// <returns></returns>
        private static int DetectByMethodNames_Accessory(string fileContents)
        {
            if (fileContents.Contains("public override void UpdateAccessory"))
                return (int)MainWindow.ItemType.Accessory;
            else
                return (int)MainWindow.ItemType.Unknown;
        }

        private static string RemoveComments(string fileContents)
        {
            while (fileContents.Contains("/*") && fileContents.Contains("*/"))
            {
                fileContents = fileContents.Remove(fileContents.IndexOf("/*"), fileContents.IndexOf("*/") - fileContents.IndexOf("/*") + 2);
            }

            fileContents = Regex.Replace(fileContents, "//.*", "");

            return fileContents;
        }

        private static string RemoveSpaces(this string fileContents)
        {
            return Regex.Replace(fileContents, " ", "");
        }

        public static int DetectFrameHeight(int sourceHeight, int itemType)
        {
            if (MainWindow.IsArmorLike(itemType))
            {
                if (sourceHeight == 1120)
                    return 56;
                else if (sourceHeight == 1118)
                    return 56;
            }

            return sourceHeight;
        }
    }
}