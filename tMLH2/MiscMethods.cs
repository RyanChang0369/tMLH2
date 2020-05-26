using System;
using System.ComponentModel;
using System.Globalization;
using System.Linq;
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

        public static string GetDescription<T>(this T e) where T : IConvertible
        {
            if (e is Enum)
            {
                Type type = e.GetType();
                Array values = Enum.GetValues(type);

                foreach (int val in values)
                {
                    if (val == e.ToInt32(CultureInfo.InvariantCulture))
                    {
                        var memInfo = type.GetMember(type.GetEnumName(val));
                        var descriptionAttribute = memInfo[0]
                            .GetCustomAttributes(typeof(DescriptionAttribute), false)
                            .FirstOrDefault() as DescriptionAttribute;

                        if (descriptionAttribute != null)
                        {
                            return descriptionAttribute.Description;
                        }
                    }
                }
            }

            return "";
        }
    }
}
