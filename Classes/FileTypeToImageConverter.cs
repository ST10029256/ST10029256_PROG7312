using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// Converts file paths into appropriate images or icons based on the file type.
    /// Implements IValueConverter for use in WPF data binding.
    /// </summary>
    public class FileTypeToImageConverter : IValueConverter
    {
        /// <summary>
        /// Converts the file path to an image or icon representing the file type.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var filePath = value as string; // Cast the value to a string representing the file path

            if (!string.IsNullOrEmpty(filePath)) // Ensure the file path is not empty or null
            {
                var fileExtension = Path.GetExtension(filePath)?.ToLower(); // Get the file extension

                switch (fileExtension)
                {
                    case ".jpg":
                    case ".jpeg":
                    case ".png":
                        try
                        {
                            // Return the actual image file
                            return new BitmapImage(new Uri(filePath, UriKind.Absolute));
                        }
                        catch
                        {
                            // If loading the image fails, return a default icon
                            return new BitmapImage(new Uri("pack://application:,,,/Images/default.png"));
                        }

                    case ".pdf":
                        // Return a PDF icon
                        return new BitmapImage(new Uri("pack://application:,,,/Images/pdf.png"));

                    case ".docx":
                        // Return a Word document icon
                        return new BitmapImage(new Uri("pack://application:,,,/Images/wordDocx.png"));

                    case ".txt":
                        // Return a text file icon
                        return new BitmapImage(new Uri("pack://application:,,,/Images/txt.png"));

                    default:
                        // Return a default icon for unsupported files
                        return new BitmapImage(new Uri("pack://application:,,,/Images/default.png"));
                }
            }

            // Return a default icon if the file path is null or empty
            return new BitmapImage(new Uri("pack://application:,,,/Images/default.png"));
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
