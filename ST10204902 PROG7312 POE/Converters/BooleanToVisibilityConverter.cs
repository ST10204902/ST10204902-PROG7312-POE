using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace ST10204902_PROG7312_POE.Converters
{
    //------------------------------------------------------------------
    /// <summary>
    /// Converts a boolean value to a visibility value.
    /// </summary>
    public class BooleanToVisibilityConverter : IValueConverter
    {
        //------------------------------------------------------------------
        /// <summary>
        /// Converts a boolean value to a visibility value.
        /// </summary>
        /// <param name="value">The boolean value to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is bool booleanValue)
            {
                return booleanValue ? Visibility.Visible : Visibility.Collapsed;
            }
            return Visibility.Collapsed;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Converts a visibility value to a boolean value.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is Visibility visibility)
            {
                return visibility == Visibility.Visible;
            }
            return false;
        }
    }
}
// ------------------------------EOF------------------------------------