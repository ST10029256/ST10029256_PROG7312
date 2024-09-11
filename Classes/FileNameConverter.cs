using System;
using System.Globalization;
using System.IO;
using System.Windows.Data;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// This class is responsible for converting a file path to just the file name.
    /// Implements the IValueConverter interface for data binding in WPF.
    /// </summary>
    public class FileNameConverter : IValueConverter
    {
        /// <summary>
        /// Converts a full file path into just the file name.
        /// </summary>
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Cast the incoming value to a string representing the file path
            var filePath = value as string;

            // If the file path is null or empty, return null
            if (string.IsNullOrEmpty(filePath))
                return null;

            // Return the file name extracted from the file path
            return Path.GetFileName(filePath);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            // Conversion back is not implemented for this converter
            throw new NotImplementedException();
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
