using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ST10029256_PROG7312.Converters
{
    /// <summary>
    /// Converts a file path into its file name for display purposes.
    /// Implements the IValueConverter interface for use in WPF data binding.
    /// </summary>
    public class FileNameConverter : IValueConverter
    {
        /// <summary>
        /// Converts a file path to its file name.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Check if the value is a valid string and not null or empty
            if (value is string filePath && !string.IsNullOrEmpty(filePath))
            {
                // Use Path.GetFileName to extract the file name from the file path
                return Path.GetFileName(filePath);
            }
            // Return an empty string if the value is null or invalid
            return string.Empty;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// This method is not implemented because conversion back from a file name to a file path is not required.
        /// </summary>
        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion back is not supported, so throw an exception
            throw new NotImplementedException();
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
