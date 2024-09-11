using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;
using System.Windows.Media.Imaging;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// This class converts a file path into an appropriate icon based on the file's extension.
    /// It implements the IValueConverter interface to convert file names to images.
    /// </summary>
    public class FileIconConverter : IValueConverter
    {
        /// <summary>
        /// Converts a file path to a corresponding icon image based on the file extension.
        /// Supported extensions: .pdf, .docx, .txt. Default icon for other file types.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Cast the value to a string (file name)
            var fileName = value as string;

            // If the file name is empty, return null (no icon)
            if (string.IsNullOrEmpty(fileName))
                return null;

            // Get the file extension and convert it to lowercase
            var extension = Path.GetExtension(fileName)?.ToLower();

            // Default icon if the file type isn't matched
            string iconPath = "pack://application:,,,/Images/default.png";

            // Match the file extension to the correct icon path
            switch (extension)
            {
                case ".pdf":
                    iconPath = "pack://application:,,,/Images/pdf.png";
                    break;
                case ".docx":
                    iconPath = "pack://application:,,,/Images/wordDocx.png";
                    break;
                case ".txt":
                    iconPath = "pack://application:,,,/Images/txt.png";
                    break;
            }

            // Return the corresponding BitmapImage based on the icon path
            return new BitmapImage(new Uri(iconPath));
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
