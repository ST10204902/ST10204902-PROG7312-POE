using EventScraper;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace ST10204902_PROG7312_POE.Converters
{
    /// <summary>
    /// Converts a DateTime to a year and month string.
    /// </summary>
    public class YearMonthGroupConverter : IValueConverter
    {

        //------------------------------------------------------------------
        /// <summary>
        /// Converts a DateTime to a year and month string.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is DateTime date)
            {
                return date.ToString("yyyy MMMM", CultureInfo.InvariantCulture);
            }
            return "Unknown";
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Converts a year and month string to a DateTime.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
// ------------------------------EOF------------------------------------