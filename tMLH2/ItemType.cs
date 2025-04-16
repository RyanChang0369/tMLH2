using System.Windows;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Controls;
using System.IO;
using System.Threading;
using System.ComponentModel;

namespace tMLH2
{
    public partial class MainWindow : Window
    {
        public enum ItemType
        {
            [Description("Unknown object that is treated as an item.")]
            Unknown = -100,
            [Description("Used for the background sprites. Do not use.")]
            Static = 0,
            [Description("Any type of armor. This program does not currently differentiate between different types of armor.")]
            Armor = 10,
            [Description("Armor that goes in the legs slot.")]
            Legs = 11,
            [Description("Armor that goes in the body slot for male characters.")]
            MaleBody = 12,
            [Description("Armor that goes in the body slot for female characters.")]
            FemaleBody = 13,
            [Description("Armor that goes over the arms.")]
            Arms = 14,
            [Description("Armor that goes in the helmet slot.")]
            Head = 15,
            [Description("Any type of accessory")]
            Accessory = 20,
            [Description("Any type of item. This program does not currently differentiate between different types of items.")]
            Item = 100,
            [Description("Most swords, except for short swords, and placeable items.")]
            Swing = 110,    // Most swords, placeable items
            [Description("Healing and food items.")]
            Eat = 120,      // Healing items
            [Description("Short swords.")]
            Stab = 130,     // Shortswords
            [Description("Life crystals, mana crystals, and summoning items.")]
            HoldUp = 140,   // Life crystals, mana crystals, summoning
            [Description("Guns, spellbooks, and spears.")]
            HoldOut = 150   // Guns, spellbooks, spears
        }

        /// <summary>
        /// Gets the group that this item type is in (armor, weapon, etc)
        /// </summary>
        /// <param name="itemType"></param>
        public static int GetGrouping(ItemType itemType)
        {
            return ((int)itemType / 10) * 10;
        }

        public static bool IsArmorLike(MainWindow.ItemType itemType)
        {
            return GetGrouping(itemType) == (int)ItemType.Armor
                || GetGrouping(itemType) == (int)ItemType.Static;
        }
        
        //Change to match call from BitmapLayer
        public static int GetNumberOfFrames(ItemType itemType, int sourceHeight, int frameHeight)
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
