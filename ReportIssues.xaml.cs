using Microsoft.Win32; 
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq; 
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging; 

namespace ST10029256_PROG7312
{
    public partial class ReportIssues : UserControl
    {
        /// <summary>
        /// Stores the list of submitted reports.
        /// </summary>
        private readonly List<ReportIssue> reportIssues;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Stores file attachments temporarily before submitting a report.
        /// </summary>
        private readonly List<string> attachments = new List<string>();

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes a new instance of the <see cref="ReportIssues"/> class and sets the list of stored reports.
        /// </summary>
        /// <param name="storedReports">A list of reports that have been submitted previously.</param>
        public ReportIssues(List<ReportIssue> storedReports)
        {
            InitializeComponent(); // Initialize UI components
            reportIssues = storedReports; // Set the list of stored reports
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the back button to navigate back to the main menu.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Reference the main window to control its navigation
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                // Set the MainFrame content to null (clears the frame)
                mainWindow.MainFrame.Content = null;
                // Hide the MainFrame
                mainWindow.MainFrame.Visibility = Visibility.Collapsed;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for navigating to the display screen that shows submitted reports.
        /// </summary>
        private void NavigateToDisplay_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                // Navigate to the report display screen, passing the stored reports list
                mainWindow.MainFrame.Content = new ReportIssuesDisplay(reportIssues);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Opens a file dialog for attaching images or documents to a report.
        /// </summary>
        private void AttachMedia_Click(object sender, RoutedEventArgs e)
        {
            // Configure the OpenFileDialog for image and document files
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image and Document files (*.jpg, *.jpeg, *.png, *.pdf, *.docx, *.txt) | *.jpg; *.jpeg; *.png; *.pdf; *.docx; *.txt", // Supported file types
                Multiselect = true // Allows multiple files to be selected
            };

            // Open the file dialog and check if the user selected files
            if (openFileDialog.ShowDialog() == true)
            {
                // Add the selected file paths to the attachments list
                attachments.AddRange(openFileDialog.FileNames);

                // Loop through the selected files
                foreach (string fileName in openFileDialog.FileNames)
                {
                    // Determine the icon to use based on the file extension
                    var iconImage = GetIconForFileExtension(Path.GetExtension(fileName).ToLower());

                    // Create a StackPanel to display the icon and file name
                    var docPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical, // Arrange items vertically
                        Margin = new Thickness(5), // Add some margin
                        HorizontalAlignment = HorizontalAlignment.Left // Align to the left
                    };

                    // Create an Image control for the file icon
                    var documentIcon = new Image
                    {
                        Width = 50, // Set icon width
                        Height = 50, // Set icon height
                        Source = iconImage, // Set the image source to the file icon
                        Margin = new Thickness(5) // Add some margin around the icon
                    };

                    // Create a TextBlock for the file name
                    var docName = new TextBlock
                    {
                        Text = Path.GetFileName(fileName), // Display the file name only
                        Width = 100, // Set a fixed width
                        TextAlignment = TextAlignment.Center, // Center-align the text
                        TextWrapping = TextWrapping.Wrap // Wrap long file names
                    };

                    // Add the icon and file name to the StackPanel
                    docPanel.Children.Add(documentIcon);
                    docPanel.Children.Add(docName);

                    // Add the StackPanel to the form's image panel for display
                    ImagePanel.Children.Add(docPanel);
                }

                // Update the progress bar and engagement label
                UpdateProgress();
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Returns an icon based on the file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension (e.g., ".pdf").</param>
        /// <returns>A <see cref="BitmapImage"/> representing the file type icon.</returns>
        private BitmapImage GetIconForFileExtension(string fileExtension)
        {
            // Determine the correct icon based on the file extension
            string iconUri;
            switch (fileExtension)
            {
                case ".pdf":
                    iconUri = "pack://application:,,,/Images/pdf.png"; // PDF icon
                    break;
                case ".docx":
                    iconUri = "pack://application:,,,/Images/wordDocx.png"; // Word document icon
                    break;
                case ".txt":
                    iconUri = "pack://application:,,,/Images/txt.png"; // Text file icon
                    break;
                default:
                    iconUri = "pack://application:,,,/Images/default.png"; // Default icon for unsupported file types
                    break;
            }

            // Return the BitmapImage created from the icon URI
            return new BitmapImage(new Uri(iconUri));
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Validates the form fields and submits the report if all fields are valid.
        /// </summary>
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation(); // Initialize the validation class

            // Retrieve input values from the form
            var location = LocationTextBox.Text.Trim();
            var category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var categoryIndex = CategoryComboBox.SelectedIndex;
            var issueDescription = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text.Trim();

            // Clear any previous error messages
            ClearErrorMessages();

            // Validate the input fields
            var validationResults = new List<(bool, TextBlock, string)>
            {
                (validation.ValidateLocation(location), LocationError, "Location cannot be empty."), // Validate location
                (validation.ValidateCategory(categoryIndex), CategoryError, "Please select a category."), // Validate category
                (validation.ValidateIssueDescription(issueDescription), IssueDescriptionError, "Issue description must be less than 50 words.") // Validate description length
            };

            // Check if any validation failed and display corresponding error messages
            if (validationResults.Any(result => !result.Item1))
            {
                validationResults.Where(result => !result.Item1).ToList()
                                 .ForEach(result => { result.Item2.Text = result.Item3; result.Item2.Visibility = Visibility.Visible; });
                return;
            }

            // Create a new report and add it to the report list if validation passed
            var newReport = new ReportIssue
            {
                Location = location,
                Category = category,
                IssueDescription = issueDescription,
                Attachments = new List<string>(attachments) // Copy the attachments
            };

            // Add the new report to the stored reports
            reportIssues.Add(newReport);

            // Clear the form after submission
            ClearForm();

            // Show a success window (celebration)
            new CelebrationWindow().ShowDialog();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Clears the form fields and resets the progress bar.
        /// </summary>
        private void ClearForm()
        {
            // Clear input fields
            LocationTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;
            IssueDescriptionRichTextBox.Document.Blocks.Clear();
            ImagePanel.Children.Clear();
            attachments.Clear(); // Clear the attachments list

            // Reset the progress bar
            ResetProgress();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Updates the progress bar based on the number of completed fields.
        /// </summary>
        private void UpdateProgress()
        {
            // Calculate the number of completed fields (location, category, description, and attachments)
            var completedSteps = new[]
            {
                !string.IsNullOrWhiteSpace(LocationTextBox.Text), // Location entered
                CategoryComboBox.SelectedIndex != -1, // Category selected
                !string.IsNullOrWhiteSpace(new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text), // Description entered
                attachments.Any() // Attachments added
            }.Count(step => step); // Count how many steps are completed

            // Calculate the progress percentage
            var progressPercentage = (completedSteps * 100) / 4;
            ProgressBar.Value = progressPercentage; // Set progress bar value

            // Update engagement label based on progress
            EngagementLabel.Text = progressPercentage >= 100
                ? "You're all set! Just submit the report."
                : $"You've completed {progressPercentage}% of the report.";

            EngagementLabel.Visibility = Visibility.Visible; // Show the engagement label
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles text change events and hides the error message.
        /// </summary>
        private void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e) => HandleTextBoxChanged(LocationError);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles selection change in the category ComboBox and hides the error message.
        /// </summary>
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => HandleSelectionChanged(CategoryError);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles text change in the issue description and hides the error message.
        /// </summary>
        private void IssueDescriptionRichTextBox_TextChanged(object sender, TextChangedEventArgs e) => HandleTextBoxChanged(IssueDescriptionError);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Hides error labels when the user corrects input in a text box.
        /// </summary>
        private void HandleTextBoxChanged(TextBlock errorLabel)
        {
            UpdateProgress(); // Update the progress bar
            errorLabel.Visibility = Visibility.Collapsed; // Hide the error message
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Hides error labels when the user corrects input in a selection field.
        /// </summary>
        private void HandleSelectionChanged(TextBlock errorLabel)
        {
            UpdateProgress(); // Update the progress bar
            errorLabel.Visibility = Visibility.Collapsed; // Hide the error message
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Clears all error messages on the form.
        /// </summary>
        private void ClearErrorMessages()
        {
            LocationError.Visibility = Visibility.Collapsed;
            CategoryError.Visibility = Visibility.Collapsed;
            IssueDescriptionError.Visibility = Visibility.Collapsed;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Resets the progress bar and hides the engagement label.
        /// </summary>
        private void ResetProgress()
        {
            ProgressBar.Value = 0; // Reset the progress bar value
            EngagementLabel.Visibility = Visibility.Collapsed; // Hide the engagement label
            ClearErrorMessages(); // Clear all error messages
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the GotFocus event for the issue description RichTextBox.
        /// </summary>
        private void IssueDescriptionRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            // Check if the text box is empty
            var text = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text;

            // Hide placeholder text when the user starts typing
            if (string.IsNullOrWhiteSpace(text))
            {
                PlaceholderTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the LostFocus event for the issue description RichTextBox.
        /// </summary>
        private void IssueDescriptionRichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            // Check if the text box is empty after losing focus
            var text = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text;

            // Show placeholder text if the text box is empty
            if (string.IsNullOrWhiteSpace(text))
            {
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
