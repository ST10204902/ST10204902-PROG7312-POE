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
    public class MediaAttachmentConverter : IValueConverter
    {
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

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

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