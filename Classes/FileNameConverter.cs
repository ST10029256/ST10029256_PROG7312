using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ST10029256_PROG7312.Converters
{
    /// <summary>
    /// Converter class that extracts and returns the file name from a full file path.
    /// Implements the IValueConverter interface to allow usage in XAML data bindings.
    /// </summary>
    public class FileNameConverter : IValueConverter
    {
        /// <summary>
        /// Converts a full file path to just the file name.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Convert the value to a string (expecting a file path).
            string filePath = value as string;

            // If the file path is null or empty, return an empty string.
            if (string.IsNullOrEmpty(filePath)) return string.Empty;

            // Return only the file name portion of the file path.
            return Path.GetFileName(filePath);
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
