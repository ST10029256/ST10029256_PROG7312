using System;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// This class provides validation methods for report issue forms.
    /// It checks if the location, category, and issue description fields are properly filled.
    /// </summary>
    public class Validation
    {
        /// <summary>
        /// Validates if the location field is not empty or consists only of white space.
        /// </summary>
        public bool ValidateLocation(string location)
        {
            // Return true if the location is not null or empty (after trimming white spaces)
            return !string.IsNullOrWhiteSpace(location);
        }
        
        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Validates if a category has been selected from the list.
        /// </summary>
        public bool ValidateCategory(int selectedIndex)
        {
            // Return true if a valid category is selected (index should not be -1)
            return selectedIndex != -1;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Validates if the issue description is not empty and contains fewer than 50 words.
        /// </summary>
        public bool ValidateIssueDescription(string issueDescription)
        {
            // Split the issue description into words, ignoring empty entries (newlines, spaces)
            var words = issueDescription.Split(new[] { ' ', '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

            // Return true if the description is not empty and contains 50 words or fewer
            return !string.IsNullOrWhiteSpace(issueDescription) && words.Length <= 50;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Validates all fields of the report form. 
        /// This method checks location, category, and issue description, and provides an error message if validation fails.
        /// </summary>
        public bool ValidateAllFields(string location, int categoryIndex, string issueDescription, out string errorMessage)
        {
            // Initialize the error message as empty
            errorMessage = string.Empty;

            // Validate location, return an error if invalid
            if (!ValidateLocation(location))
            {
                errorMessage = "Location cannot be empty.";
                return false;
            }

            // Validate category, return an error if invalid
            if (!ValidateCategory(categoryIndex))
            {
                errorMessage = "Please select a category.";
                return false;
            }

            // Validate issue description, return an error if it exceeds 50 words or is empty
            if (!ValidateIssueDescription(issueDescription))
            {
                errorMessage = "Issue description must be less than 50 words.";
                return false;
            }

            // If all validations pass, return true
            return true;
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
