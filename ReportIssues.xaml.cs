using Microsoft.Win32;
using System;
using System.Collections.Generic;
using System.IO;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ST10029256_PROG7312
{
    public partial class ReportIssues : UserControl
    {
        /// <summary>
        /// Reference to the list of all submitted reports.
        /// </summary>
        private List<ReportIssue> reportIssues;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Temporary list to store file attachments before submission.
        /// </summary>
        private List<string> attachments = new List<string>();

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Tracks the progress of form completion.
        /// </summary>
        private int progress = 0;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Constructor for the ReportIssues UserControl.
        /// Initializes the form and takes in a list of stored reports to be updated with new submissions.
        /// </summary>
        public ReportIssues(List<ReportIssue> storedReports)
        {
            InitializeComponent();
            reportIssues = storedReports;  // Initialize the list of reports
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the back button.
        /// Closes the current form and returns to the previous screen.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            this.Visibility = Visibility.Collapsed; // Hide the form
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Opens a file dialog to allow the user to attach images or documents to the report.
        /// </summary>
        private void AttachMedia_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image and Document files (*.jpg, *.jpeg, *.png, *.pdf, *.docx, *.txt) | *.jpg; *.jpeg; *.png; *.pdf; *.docx; *.txt",
                Multiselect = true  // Allow multiple file selections
            };

            if (openFileDialog.ShowDialog() == true)
            {
                foreach (string fileName in openFileDialog.FileNames)
                {
                    string fileExtension = Path.GetExtension(fileName).ToLower();
                    attachments.Add(fileName);  // Add the file path to the attachments list

                    BitmapImage iconImage = null;

                    // Set the appropriate icon based on the file extension
                    switch (fileExtension)
                    {
                        case ".pdf":
                            iconImage = new BitmapImage(new Uri("pack://application:,,,/Images/pdf.png"));  // PDF icon
                            break;
                        case ".docx":
                            iconImage = new BitmapImage(new Uri("pack://application:,,,/Images/wordDocx.png"));  // DOCX icon
                            break;
                        case ".txt":
                            iconImage = new BitmapImage(new Uri("pack://application:,,,/Images/txt.png"));  // TXT icon
                            break;
                        default:
                            iconImage = new BitmapImage(new Uri("pack://application:,,,/Images/default.png"));  // Default icon
                            break;
                    }

                    // Create a StackPanel to hold the icon and file name
                    StackPanel docPanel = new StackPanel
                    {
                        Orientation = Orientation.Vertical,
                        Margin = new Thickness(5),
                        HorizontalAlignment = HorizontalAlignment.Left
                    };

                    // Create an Image to display the file icon
                    Image documentIcon = new Image
                    {
                        Width = 50,
                        Height = 50,
                        Source = iconImage,
                        Margin = new Thickness(5)
                    };

                    // Create a TextBlock to show the document's file name
                    TextBlock docName = new TextBlock
                    {
                        Text = Path.GetFileName(fileName),
                        Width = 100,
                        TextAlignment = TextAlignment.Center,
                        TextWrapping = TextWrapping.Wrap
                    };

                    // Add the icon and file name to the StackPanel
                    docPanel.Children.Add(documentIcon);
                    docPanel.Children.Add(docName);

                    // Add the StackPanel to the WrapPanel on the form
                    ImagePanel.Children.Add(docPanel);
                }

                // Updates progress and show engagement message
                UpdateProgress();
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Validates the form fields, then submits the report if validation passes.
        /// Clears the form and opens a celebration window upon successful submission.
        /// </summary>
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation();

            // Retrieve data from the form
            string location = LocationTextBox.Text;
            string category = CategoryComboBox.Text;
            int categoryIndex = CategoryComboBox.SelectedIndex;
            string issueDescription = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text;

            // Validate the input fields (attachments are optional)
            string errorMessage;
            bool isValid = validation.ValidateAllFields(location, categoryIndex, issueDescription, out errorMessage);

            if (!isValid)
            {
                // Show validation error message
                MessageBox.Show(errorMessage, "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            // Create a new report if validation succeeds
            ReportIssue newReport = new ReportIssue
            {
                Location = location,
                Category = category,
                IssueDescription = issueDescription,
                Attachments = new List<string>(attachments)  // Copy the attachments
            };

            // Add the new report to the list
            reportIssues.Add(newReport);

            // Clear the form
            LocationTextBox.Clear();
            CategoryComboBox.SelectedIndex = -1;
            IssueDescriptionRichTextBox.Document.Blocks.Clear();
            ImagePanel.Children.Clear();
            attachments.Clear();  // Clear attachments

            progress = 0;  // Reset progress
            ProgressBar.Value = 0;
            EngagementLabel.Visibility = Visibility.Collapsed;

            // Show celebration window upon successful submission
            CelebrationWindow celebration = new CelebrationWindow();
            celebration.ShowDialog();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Updates the progress bar based on the number of completed fields.
        /// </summary>
        private void UpdateProgress()
        {
            int completedSteps = 0;

            // Check if the location field is filled
            if (!string.IsNullOrWhiteSpace(LocationTextBox.Text))
            {
                completedSteps += 1;
            }

            // Check if a category is selected
            if (CategoryComboBox.SelectedIndex != -1)
            {
                completedSteps += 1;
            }

            // Check if the issue description has text
            var issueDescriptionText = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text;
            if (!string.IsNullOrWhiteSpace(issueDescriptionText))
            {
                completedSteps += 1;
            }

            // Check if there are any attachments
            if (attachments.Count > 0)
            {
                completedSteps += 1;
            }

            // Calculates the percentage of completed steps
            int progressPercentage = (completedSteps * 100) / 4;
            ProgressBar.Value = progressPercentage;

            // Updates the engagement label based on the progress
            if (progressPercentage >= 100)
            {
                EngagementLabel.Text = "You're all set! Just submit the report.";
            }
            else
            {
                EngagementLabel.Text = $"You've completed {progressPercentage}% of the report.";
            }

            EngagementLabel.Visibility = Visibility.Visible;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Updates the progress bar whenever the location field changes.
        /// </summary>
        private void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgress();  // Update progress
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Updates the progress bar whenever a category is selected.
        /// </summary>
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProgress();  // Update progress
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Updates the progress bar whenever the issue description changes.
        /// </summary>
        private void IssueDescriptionRichTextBox_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgress();  // Update progress
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles focus event for the issue description RichTextBox.
        /// Hides the placeholder if the RichTextBox contains text.
        /// </summary>
        private void IssueDescriptionRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var text = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                PlaceholderTextBlock.Visibility = Visibility.Collapsed;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the loss of focus event for the issue description RichTextBox.
        /// Shows the placeholder if the RichTextBox is empty.
        /// </summary>
        private void IssueDescriptionRichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var text = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text;

            if (string.IsNullOrWhiteSpace(text))
            {
                PlaceholderTextBlock.Visibility = Visibility.Visible;
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
