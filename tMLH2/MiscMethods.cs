using System.Windows;
using System.Windows.Controls;
using static tMLH2.MainWindow;

namespace tMLH2
{
    public static class MiscMethods
    {
        public static bool IsValid(this DependencyObject control)
        {
            if (Validation.GetHasError(control))
                return false;
            else
                return true;
        }

        /// <summary>
        /// Returns true if the interger value of the ItemType is below 0.
        /// </summary>
        /// <returns></returns>
        public static bool ItemTypeIsUnknown(this ItemType itemType)
        {
            return itemType < 0;
        }
    }
}
