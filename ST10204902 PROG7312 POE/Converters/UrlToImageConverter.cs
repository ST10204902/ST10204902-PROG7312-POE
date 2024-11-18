using System.Globalization;
using System.Windows.Media.Imaging;
using System;
using System.Windows.Data;
using System.Windows.Controls;
namespace ST10204902_PROG7312_POE.Converters
{
    /// <summary>
    /// Converts a URL to an image.
    /// </summary>
    public class UrlToImageConverter : IValueConverter
    {
        //------------------------------------------------------------------
        /// <summary>
        /// Converts a URL to an image.
        /// </summary>
        /// <param name="value"></param>
        /// <param name="targetType"></param>
        /// <param name="parameter"></param>
        /// <param name="culture"></param>
        /// <returns></returns>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is string url && !string.IsNullOrEmpty(url))
            {
                try
                {
                    var bitmap = new BitmapImage();
                    bitmap.BeginInit();
                    bitmap.UriSource = new Uri(url, UriKind.Absolute);
                    bitmap.CacheOption = BitmapCacheOption.OnLoad;
                    bitmap.EndInit();
                    return bitmap;
                }
                catch (Exception)
                {
                    return null;
                }
            }
            return null;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Converts an image to a URL.
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