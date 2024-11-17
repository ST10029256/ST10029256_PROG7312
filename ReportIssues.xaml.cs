using Microsoft.Win32;
using ST10029256_PROG7312.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media.Imaging;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// ReportIssues UserControl manages the creation and submission of issue reports,
    /// media attachment, and validation of form fields.
    /// </summary>
    public partial class ReportIssues : UserControl
    {
        private ObservableCollection<ReportIssue> reportIssues; // Stores the collection of reports
        private readonly ObservableCollection<string> attachments = new ObservableCollection<string>(); // Stores file paths of attachments
        private static int reportCounter = 1; // Counter to generate unique request IDs

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes the ReportIssues UserControl with a collection of reports.
        /// Adds sample data if the collection is empty.
        /// </summary>
        /// <param name="reports">Collection of existing reports.</param>
        public ReportIssues(ObservableCollection<ReportIssue> reports)
        {
            InitializeComponent(); // Initialize the component
            reportIssues = reports ?? new ObservableCollection<ReportIssue>(); // Assign the collection or create a new one

            // Add sample data if the collection is empty
            if (!reportIssues.Any())
            {
                AddSampleData();
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Adds sample data to the reportIssues collection for demonstration purposes.
        /// </summary>
        private void AddSampleData()
        {
            reportIssues.Add(new ReportIssue
            {
                RequestID = $"REQ{reportCounter++.ToString("D3")}",
                Location = "Main Street",
                Category = "Road Issue",
                IssueDescription = "Potholes on the road causing damage to vehicles.",
                DateSubmitted = DateTime.Now.AddDays(-3),
                DateOfIssue = DateTime.Now.AddDays(-5),
                Status = "Pending",
                Attachments = new ObservableCollection<string> { "pothole_image.jpg" },
                Priority = PriorityLevel.High
            });

            reportIssues.Add(new ReportIssue
            {
                RequestID = $"REQ{reportCounter++.ToString("D3")}",
                Location = "Central Park",
                Category = "Park Maintenance",
                IssueDescription = "Swing in the play area is broken and unsafe for children.",
                DateSubmitted = DateTime.Now.AddDays(-2),
                DateOfIssue = DateTime.Now.AddDays(-4),
                Status = "In Progress",
                Attachments = new ObservableCollection<string> { "swing_broken.jpg" },
                Priority = PriorityLevel.Medium
            });

            reportIssues.Add(new ReportIssue
            {
                RequestID = $"REQ{reportCounter++.ToString("D3")}",
                Location = "Broadway Avenue",
                Category = "Lighting",
                IssueDescription = "Streetlights are not functioning, making the area unsafe at night.",
                DateSubmitted = DateTime.Now.AddDays(-1),
                DateOfIssue = DateTime.Now.AddDays(-3),
                Status = "Completed",
                Attachments = new ObservableCollection<string> { "streetlights_out.jpg" },
                Priority = PriorityLevel.Low
            });
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Navigates back to the main page by clearing the frame content.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow; // Get the main window instance
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = null; // Clear the content of the main frame
                mainWindow.MainFrame.Visibility = Visibility.Collapsed; // Hide the main frame
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Navigates to the ReportIssuesDisplay page to view the list of reports.
        /// </summary>
        private void NavigateToDisplay_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow; // Get the main window instance
            if (mainWindow != null)
            {
                var reportIssuesDisplayPage = new ReportIssuesDisplay(reportIssues); // Create a new display page
                mainWindow.MainFrame.Content = reportIssuesDisplayPage; // Set the content to display page
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Opens a file dialog for attaching media files and adds them to the attachments list.
        /// </summary>
        private void AttachMedia_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image and Document files (*.jpg, *.jpeg, *.png, *.pdf, *.docx, *.txt) | *.jpg; *.jpeg; *.png; *.pdf; *.docx; *.txt", // Filter for allowed file types
                Multiselect = true // Enable selecting multiple files
            };

            if (openFileDialog.ShowDialog() == true) // Check if the dialog result is OK
            {
                foreach (var fileName in openFileDialog.FileNames) // Iterate through selected files
                {
                    if (File.Exists(fileName)) // Check if file exists
                    {
                        attachments.Add(fileName); // Add the file to the attachments list
                    }
                    else
                    {
                        MessageBox.Show($"File not found: {fileName}", "Attachment Error", MessageBoxButton.OK, MessageBoxImage.Warning); // Show error if file not found
                    }
                }
                DisplayAttachments(); // Refresh the attachment display
                UpdateProgress(); // Update progress bar
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Displays the list of attached files in the ImagePanel with appropriate icons.
        /// </summary>
        private void DisplayAttachments()
        {
            ImagePanel.Children.Clear(); // Clear existing attachment display
            foreach (string fileName in attachments) // Iterate through the attachments
            {
                var iconImage = GetIconForFileExtension(Path.GetExtension(fileName).ToLower()); // Get the icon based on file type

                var docPanel = new StackPanel
                {
                    Orientation = Orientation.Vertical, // Arrange items vertically
                    Margin = new Thickness(5),
                    HorizontalAlignment = HorizontalAlignment.Left
                };

                var documentIcon = new Image
                {
                    Width = 50, // Set icon size
                    Height = 50,
                    Source = iconImage, // Set the icon image source
                    Margin = new Thickness(5)
                };

                var docName = new TextBlock
                {
                    Text = Path.GetFileName(fileName), // Display the file name
                    Width = 100,
                    TextAlignment = TextAlignment.Center,
                    TextWrapping = TextWrapping.Wrap
                };

                docPanel.Children.Add(documentIcon); // Add icon to panel
                docPanel.Children.Add(docName); // Add file name to panel
                ImagePanel.Children.Add(docPanel); // Add panel to the ImagePanel
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves an icon image for the given file extension.
        /// </summary>
        /// <param name="fileExtension">The file extension.</param>
        /// <returns>BitmapImage of the file icon.</returns>
        private BitmapImage GetIconForFileExtension(string fileExtension)
        {
            string iconUri; // Icon URI based on file type
            switch (fileExtension)
            {
                case ".pdf":
                    iconUri = "pack://application:,,,/Images/pdf.png"; // Icon for PDF files
                    break;
                case ".docx":
                    iconUri = "pack://application:,,,/Images/wordDocx.png"; // Icon for Word documents
                    break;
                case ".txt":
                    iconUri = "pack://application:,,,/Images/txt.png"; // Icon for text files
                    break;
                default:
                    iconUri = "pack://application:,,,/Images/default.png"; // Default icon
                    break;
            }

            return new BitmapImage(new Uri(iconUri)); // Create and return the BitmapImage
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the submission of a new report after validating the input fields.
        /// </summary>
        private void SubmitBtn_Click(object sender, RoutedEventArgs e)
        {
            var location = LocationTextBox.Text.Trim(); // Retrieve location input
            var category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(); // Retrieve selected category
            var issueDescription = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text.Trim(); // Retrieve issue description
            var selectedPriority = (PriorityComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(); // Retrieve selected priority
            var dateOfIssue = DateOfIssuePicker.SelectedDate; // Retrieve selected date of issue

            PriorityLevel priorityLevel;
            bool isPriorityValid = Enum.TryParse(selectedPriority, out priorityLevel); // Validate priority
            PriorityError.Visibility = isPriorityValid ? Visibility.Collapsed : Visibility.Visible; // Show or hide priority error
            PriorityError.Text = isPriorityValid ? "" : "Please select a valid priority."; // Set priority error text

            // Validation logic for input fields
            var validationErrors = new List<(bool IsValid, TextBlock ErrorControl, string ErrorMessage)>
            {
                (!string.IsNullOrWhiteSpace(location), LocationError, "Location cannot be empty."),
                (CategoryComboBox.SelectedIndex >= 0, CategoryError, "Please select a valid category."),
                (!string.IsNullOrWhiteSpace(issueDescription), IssueDescriptionError, "Description cannot be empty."),
                (isPriorityValid, PriorityError, "Please select a valid priority."),
                (dateOfIssue.HasValue, DateOfIssueError, "Please select a valid date.")
            };

            var isValid = validationErrors.All(v => v.IsValid); // Check if all validations pass

            validationErrors.ForEach(v =>
            {
                v.ErrorControl.Text = v.IsValid ? "" : v.ErrorMessage; // Set error message for invalid fields
                v.ErrorControl.Visibility = v.IsValid ? Visibility.Collapsed : Visibility.Visible; // Show or hide error messages
            });

            if (!isValid) return; // Stop submission if validation fails

            var newReport = new ReportIssue
            {
                RequestID = $"REQ{reportCounter++.ToString("D3")}", // Generate unique RequestID
                Location = location,
                Category = category,
                IssueDescription = issueDescription,
                DateSubmitted = DateTime.Now,
                DateOfIssue = dateOfIssue.Value,
                Status = "Pending",
                Attachments = new ObservableCollection<string>(attachments),
                Priority = priorityLevel
            };

            reportIssues.Add(newReport); // Add the new report to the collection
            ClearForm(); // Clear the form inputs

            var celebrationWindow = new CelebrationWindow(); // Show celebration window
            celebrationWindow.ShowDialog();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Clears all input fields and resets the form.
        /// </summary>
        private void ClearForm()
        {
            LocationTextBox.Clear(); // Clear location input
            CategoryComboBox.SelectedIndex = -1; // Reset category selection
            PriorityComboBox.SelectedIndex = -1; // Reset priority selection
            IssueDescriptionRichTextBox.Document.Blocks.Clear(); // Clear issue description
            DateOfIssuePicker.SelectedDate = null; // Reset date of issue
            ImagePanel.Children.Clear(); // Clear attachment display
            attachments.Clear(); // Clear the attachments list

            ResetProgress(); // Reset progress bar
            ClearErrorMessages(); // Clear error messages
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Updates the progress bar based on completed form steps.
        /// </summary>
        private void UpdateProgress()
        {
            var completedSteps = new bool[]
            {
                !string.IsNullOrWhiteSpace(LocationTextBox.Text), // Check location input
                CategoryComboBox.SelectedIndex != -1, // Check category selection
                !string.IsNullOrWhiteSpace(new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text), // Check issue description
                PriorityComboBox.SelectedIndex != -1, // Check priority selection
                attachments.Count > 0, // Check attachments
                DateOfIssuePicker.SelectedDate.HasValue // Check date of issue selection
            }.Count(step => step);

            ProgressBar.Value = (completedSteps * 100) / 6; // Calculate progress percentage
            EngagementLabel.Text = ProgressBar.Value == 100 ? "You're all set! Just submit the report." : $"You've completed {ProgressBar.Value}% of the report."; // Update engagement label
            EngagementLabel.Visibility = Visibility.Visible; // Show engagement label
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Resets the progress bar and hides the engagement label.
        /// </summary>
        private void ResetProgress()
        {
            ProgressBar.Value = 0; // Reset progress value
            EngagementLabel.Visibility = Visibility.Collapsed; // Hide engagement label
            ClearErrorMessages(); // Clear all error messages
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Clears all error messages on the form.
        /// </summary>
        private void ClearErrorMessages()
        {
            LocationError.Visibility = Visibility.Collapsed; // Hide location error
            CategoryError.Visibility = Visibility.Collapsed; // Hide category error
            IssueDescriptionError.Visibility = Visibility.Collapsed; // Hide description error
            PriorityError.Visibility = Visibility.Collapsed; // Hide priority error
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateProgress(); // Update progress on location change

        //---------------------------------------------------------------------------------------------------------------------------------------------//
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e) => UpdateProgress(); // Update progress on category change

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void IssueDescriptionRichTextBox_TextChanged(object sender, TextChangedEventArgs e) => UpdateProgress(); // Update progress on description change

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void IssueDescriptionRichTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            var text = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text; // Retrieve current text
            if (string.IsNullOrWhiteSpace(text)) // Check if text is empty
            {
                PlaceholderTextBlock.Visibility = Visibility.Collapsed; // Hide placeholder
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void IssueDescriptionRichTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            var text = new TextRange(IssueDescriptionRichTextBox.Document.ContentStart, IssueDescriptionRichTextBox.Document.ContentEnd).Text; // Retrieve current text
            if (string.IsNullOrWhiteSpace(text)) // Check if text is empty
            {
                PlaceholderTextBlock.Visibility = Visibility.Visible; // Show placeholder
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void PriorityComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProgress(); // Update progress on priority change
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void DateOfIssuePicker_SelectedDateChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProgress(); // Update progress on date change
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
