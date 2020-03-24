using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;
using System.IO;
using System.Threading;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        public enum ItemType
        {
            Unknown = -100,
            Static = 0,
            Armor = 10,
            Legs = 11,
            Body = 12,
            FemaleBody = 13,
            Arms = 14,
            Head = 15,
            //Ballon = 14,
            Accessory = 20,
            Swing = 110,    // Most swords, placable item
            Eat = 120,      // Healing items
            Stab = 130,     // Shortswords
            HoldUp = 140,   // Life crystals, mana crystals, summoning
            HoldOut = 150   // Guns, spellbooks, spears
        }

        /// <summary>
        /// Gets the group that this item type is in (armor, weapon, etc)
        /// </summary>
        /// <param name="itemType"></param>
        public static int GetGrouping(int itemType)
        {
            return (itemType / 10) * 10;
        }

        public static bool IsArmorLike(int itemType)
        {
            return GetGrouping(itemType) == (int)ItemType.Armor
                || GetGrouping(itemType) == (int)ItemType.Static;
        }
        
        //Change to match call from BitmapLayer
        public static int GetNumberOfFrames(int itemType, int sourceHeight, int frameHeight)
        {
            if (IsArmorLike(itemType))
            {
                double frames = Math.Ceiling((double)sourceHeight / frameHeight);
                return (int)frames;
            }
            else
                return 1;
        }
    }
}
