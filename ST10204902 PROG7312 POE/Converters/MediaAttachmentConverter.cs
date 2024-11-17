using ST10204902_PROG7312_POE.Models;
using System;
using System.Globalization;
using System.IO;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Media.Imaging;
using Patagames.Pdf.Net;
using Patagames.Pdf;

namespace ST10204902_PROG7312_POE.Converters
{
    //------------------------------------------------------------------
    /// <summary>
    /// Converts a media attachment to a UI element.
    /// </summary>
    public class MediaAttachmentConverter : IValueConverter
    {
        //------------------------------------------------------------------
        /// <summary>
        /// Converts a media attachment to a UI element.
        /// </summary>
        /// <param name="value">The media attachment to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (value is MediaAttachment mediaAttachment)
            {
                string extension = Path.GetExtension(mediaAttachment.FilePath).ToLower();
                switch (extension)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                        return CreateImageControl(mediaAttachment.FileData);
                    case ".pdf":
                        return CreatePdfPreviewImage(mediaAttachment.FileData);
                    case ".docx":
                    case ".txt":
                        return CreateTextPreview(mediaAttachment.FileData);
                    default:
                        return CreateUnsupportedFileTypeTextBlock();
                }
            }
            return null;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Converts back a UI element to a media attachment.
        /// </summary>
        /// <param name="value">The UI element to convert.</param>
        /// <param name="targetType">The target type.</param>
        /// <param name="parameter">The parameter.</param>
        /// <param name="culture">The culture.</param>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Creates an image control for image file previews.
        /// </summary>
        /// <param name="fileData">The byte array representing the file data.</param>
        /// <returns>An image control for image file previews.</returns>
        private Image CreateImageControl(byte[] fileData)
        {
            using (var stream = new MemoryStream(fileData))
            {
                var bitmap = new BitmapImage();
                bitmap.BeginInit();
                bitmap.StreamSource = stream;
                bitmap.CacheOption = BitmapCacheOption.OnLoad;
                bitmap.EndInit();
                return new Image
                {
                    Source = bitmap,
                    Width = 180,
                    Height = 180,
                    Stretch = System.Windows.Media.Stretch.Uniform
                };
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Creates an image control for PDF file previews.
        /// </summary>
        /// <param name="fileData">The byte array representing the file data.</param>
        /// <returns>An image control for PDF file previews.</returns>
        private Image CreatePdfPreviewImage(byte[] fileData)
        {
            using (var stream = new MemoryStream(fileData))
            {
                PdfCommon.Initialize();
                using (var document = PdfDocument.Load(stream))
                {
                    var page = document.Pages[0];
                    int width = page.Width > page.Height ? 180 : (int)(180 * page.Width / page.Height);
                    int height = page.Width > page.Height ? (int)(180 * page.Height / page.Width) : 180;

                    using (var bitmap = new PdfBitmap(width, height, true))
                    {
                        bitmap.FillRect(0, 0, width, height, FS_COLOR.White);
                        page.Render(bitmap, 0, 0, width, height, Patagames.Pdf.Enums.PageRotate.Normal, Patagames.Pdf.Enums.RenderFlags.FPDF_NONE);

                        using (var memoryStream = new MemoryStream())
                        {
                            bitmap.GetImage().Save(memoryStream, System.Drawing.Imaging.ImageFormat.Png);
                            memoryStream.Position = 0;

                            var bitmapImage = new BitmapImage();
                            bitmapImage.BeginInit();
                            bitmapImage.StreamSource = memoryStream;
                            bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                            bitmapImage.EndInit();

                            return new Image
                            {
                                Source = bitmapImage,
                                Width = 180,
                                Height = 180,
                                Stretch = System.Windows.Media.Stretch.Uniform
                            };
                        }
                    }
                }
            }
        }
        //------------------------------------------------------------------
        /// <summary>
        /// Creates a text block for text file previews.
        /// </summary>
        /// <param name="fileData">The byte array representing the file data.</param>
        /// <returns>A text block for text file previews.</returns> 
        private TextBlock CreateTextPreview(byte[] fileData)
        {
            string text = System.Text.Encoding.UTF8.GetString(fileData);
            return new TextBlock
            {
                Text = text.Length > 200 ? text.Substring(0, 200) + "..." : text,
                Width = 180,
                Height = 180,
                TextWrapping = System.Windows.TextWrapping.Wrap
            };
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Creates a text block for unsupported file types.
        /// </summary>
        /// <returns>A text block for unsupported file types.</returns>
        private TextBlock CreateUnsupportedFileTypeTextBlock()
        {
            return new TextBlock
            {
                Text = "Unsupported file type",
                Width = 180,
                Height = 180,
                TextAlignment = System.Windows.TextAlignment.Center,
                VerticalAlignment = System.Windows.VerticalAlignment.Center
            };
        }
    }
}
// ------------------------------EOF------------------------------------