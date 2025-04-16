using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Controls;

namespace tMLH2
{
    public class NumberValidation : ValidationRule
    {
        //public int DefaultValue { get; set; }

        public NumberValidation()
        { }
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            int number = 0;

            try
            {
                if (((string)value).Length > 0)
                    number = int.Parse((string)value);
            }
            catch (Exception)
            {
                return new ValidationResult(false, "Value entered must be a number.");
            }

            return ValidationResult.ValidResult;
        }
    }
}
