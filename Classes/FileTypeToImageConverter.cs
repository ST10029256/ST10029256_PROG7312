using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ST10029256_PROG7312.Converters
{
    /// <summary>
    /// Converter class that converts a file path to an appropriate icon or image based on the file type.
    /// Implements the IValueConverter interface to allow usage in XAML data bindings.
    /// </summary>
    public class FileTypeToImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts a file path to an icon or image based on the file extension.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert the value to a string (expecting a file path).
            string filePath = value as string;

            // If the file path is null or empty, return null.
            if (string.IsNullOrEmpty(filePath)) return null;

            // Extract the file extension and convert it to lowercase.
            string extension = Path.GetExtension(filePath).ToLower();
            string iconPath; // Holds the path for the corresponding icon.

            // Use a switch case to determine the correct icon or image based on the file extension.
            switch (extension)
            {
                case ".pdf":
                    iconPath = "pack://application:,,,/Images/pdf.png"; // PDF icon
                    break;
                case ".docx":
                    iconPath = "pack://application:,,,/Images/wordDocx.png"; // Word document icon
                    break;
                case ".txt":
                    iconPath = "pack://application:,,,/Images/txt.png"; // Text file icon
                    break;
                case ".jpg":
                case ".jpeg":
                case ".png":
                    try
                    {
                        // Attempt to return the actual image if it's a valid image file.
                        return new BitmapImage(new Uri(filePath));
                    }
                    catch
                    {
                        // If the image cannot be loaded, return a default image icon.
                        return new BitmapImage(new Uri("pack://application:,,,/Images/image.png"));
                    }
                default:
                    iconPath = "pack://application:,,,/Images/default.png"; // Default icon for unknown file types
                    break;
            }

            // Return the appropriate icon as a BitmapImage for non-image files.
            return new BitmapImage(new Uri(iconPath));
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// This method is not implemented as reverse conversion is not required.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // ConvertBack is not used in this scenario, so throw a NotImplementedException.
            throw new NotImplementedException();
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
