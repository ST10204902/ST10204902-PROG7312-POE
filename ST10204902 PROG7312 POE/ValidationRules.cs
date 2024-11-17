using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using System.Windows.Controls;

namespace ST10204902_PROG7312_POE
{
    //------------------------------------------------------------------
    public class NotEmptyValidation : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null)
            {
                return new ValidationResult(false, "Field cannot be empty.");
            }

            return ValidationResult.ValidResult;
        }
    }

    //------------------------------------------------------------------
    public class CategoryValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            if (value == null || (value is ComboBoxItem item && string.IsNullOrEmpty(item.Content as string)))
            {
                return new ValidationResult(false, "Please select a category.");
            }
            return ValidationResult.ValidResult;
        }
    }
}
// ------------------------------EOF------------------------------------
