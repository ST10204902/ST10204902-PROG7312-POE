using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ST10204902_PROG7312_POE.Converters
{
    //------------------------------------------------------------------
    /// <summary>
    /// Converts an integer priority to a string representation.
    /// </summary>
    public class PriorityToStringConverter : IValueConverter
    {
        //------------------------------------------------------------------
        /// <summary>
        /// Converts an integer priority to a string representation.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is int priority)
            {
                return priority switch
                {
                    1 => "High",
                    2 => "Medium",
                    3 => "Low",
                    _ => "Unknown"
                };
            }
            return "Unknown";
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Converts a string representation of a priority to an integer.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object ConvertBack(object value, Type targetType, object parameter, System.Globalization.CultureInfo culture)
        {
            if (value is string priorityString)
            {
                return priorityString switch
                {
                    "High" => 1,
                    "Medium" => 2,
                    "Low" => 3,
                    _ => 3
                };
            }
            return 3;
        }
    }
}
// ------------------------------EOF------------------------------------
