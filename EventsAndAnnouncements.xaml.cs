using ST10029256_PROG7312.Classes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using Microsoft.Win32;
using System.Windows.Media.Imaging;
using ST10029256_PROG7312.ViewModels;
using System.Collections.ObjectModel;
using System.Net.Mail;

namespace ST10029256_PROG7312.UserControls
{
    public partial class EventsAndAnnouncements : UserControl
    {
        // ViewModel for managing events
        private EventsViewModel _viewModel;

        // Reference to the main window to access shared properties and methods
        private MainWindow _mainWindow;

        // List to store file attachments (paths) for the event
        private ObservableCollection<string> _attachments = new ObservableCollection<string>();

        // Queue for managing multiple event submissions
        private Queue<LocalEvent> _eventQueue;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Constructor that initializes the control and sets up the ViewModel and event queue.
        /// </summary>
        public EventsAndAnnouncements(MainWindow parentWindow, EventsViewModel viewModel)
        {
            InitializeComponent(); // Initialize the UI components
            _mainWindow = parentWindow; // Store reference to parent window
            _viewModel = viewModel; // Assign the ViewModel
            this.DataContext = _viewModel; // Bind the ViewModel to the control for data binding

            _eventQueue = new Queue<LocalEvent>(); // Initialize the event queue for submissions
            UpdateCategories(); // Populate the categories ComboBox
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Updates the ComboBox with categories from MainWindow, ensuring "Select a Category" is at the top.
        /// </summary>
        private void UpdateCategories()
        {
            // Fetch unique categories from MainWindow, ensuring "Select a Category" is always first
            var categories = _mainWindow.GetUniqueCategories()
                                        .Prepend("Select a Category") // Add "Select a Category" at the beginning
                                        .ToList();

            // Bind the categories to the ComboBox
            CategoryComboBox.ItemsSource = categories;
            CategoryComboBox.SelectedIndex = 0; // Set default selection to "Select a Category"
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the submission of the event form. Validates the inputs and processes the event if valid.
        /// </summary>
        private void SubmitButton_Click(object sender, RoutedEventArgs e)
        {
            Validation validation = new Validation(); // Create a validation object

            // Retrieve values from form fields
            string eventName = EventNameTextBox.Text.Trim(); // Event name
            string category = CategoryComboBox.SelectedItem as string; // Selected category
            DateTime? eventDate = EventDatePicker.SelectedDate; // Selected event date
            string location = LocationTextBox.Text.Trim(); // Event location
            string description = new TextRange(DescriptionRichTextBox.Document.ContentStart, DescriptionRichTextBox.Document.ContentEnd).Text.Trim(); // Event description

            // Validation logic using a list of validation rules
            var validationErrors = new List<(bool IsValid, TextBlock ErrorControl, string ErrorMessage)>
            {
                (validation.ValidateTextField(eventName), EventNameError, "Event name cannot be empty."),
                (CategoryComboBox.SelectedIndex > 0, CategoryError, "Please select a valid category."),
                (validation.ValidateEventDate(eventDate), EventDateError, "Please select a valid future date."),
                (validation.ValidateTextField(location), LocationError, "Location cannot be empty."),
                (validation.ValidateDescription(description), DescriptionError, "Description must be less than 50 words.")
            };

            // Check if all validations passed
            var isValid = validationErrors.All(v => v.IsValid);

            // If there are validation errors, display the appropriate messages
            if (!isValid)
            {
                validationErrors.Where(v => !v.IsValid) // Filter out invalid entries
                                .ToList()
                                .ForEach(v => // Display error messages
                                {
                                    v.ErrorControl.Text = v.ErrorMessage;
                                    v.ErrorControl.Visibility = Visibility.Visible;
                                });
                return; // Exit if validation fails
            }

            // Create a new event if validation passed
            LocalEvent newEvent = new LocalEvent
            {
                Name = eventName,
                Category = category,
                Date = eventDate.Value, // Set event date (validated to not be null)
                Location = location,
                Description = description,
                Attachments = new ObservableCollection<string>(_attachments)
            };

            // Add the event to the submission queue
            _eventQueue.Enqueue(newEvent);
            ProcessEventQueue(); // Process the event queue (submit all events in the queue)

            ClearForm(); // Clear the form after submission

            // Show a success window after successful event submission
            new CelebrationWindow2().ShowDialog();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Processes the event queue and submits each event to the main window.
        /// </summary>
        private void ProcessEventQueue()
        {
            // Process events from the queue as long as it contains events
            while (_eventQueue.Any()) // Check if there are events in the queue
            {
                var currentEvent = _eventQueue.Dequeue(); // Get the next event in the queue
                _mainWindow.AddEvent(currentEvent); // Add the event to the main window's event list
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Clears all error messages from the form.
        /// </summary>
        private void ClearErrorMessages() =>
            new[] { EventNameError, CategoryError, EventDateError, LocationError, DescriptionError } // List of error message controls
                .ToList() // Convert array to list
                .ForEach(error => error.Visibility = Visibility.Collapsed); // Hide each error message

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Clears all fields in the form and resets the attachment list and error messages.
        /// </summary>
        private void ClearForm()
        {
            // Clear all input fields
            EventNameTextBox.Text = string.Empty;
            CategoryComboBox.SelectedIndex = 0; // Reset the category to default ("Select a Category")
            EventDatePicker.SelectedDate = null; // Clear selected date
            LocationTextBox.Text = string.Empty;
            DescriptionRichTextBox.Document.Blocks.Clear(); // Clear the description field
            _attachments.Clear(); // Clear the list of attached files
            ImagePanel.Children.Clear(); // Clear the panel displaying attached files
            ClearErrorMessages(); // Hide all error messages
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the event where the user clicks the Attach Media button to attach files.
        /// </summary>
        private void AttachMedia_Click(object sender, RoutedEventArgs e)
        {
            // Configure the file dialog to allow selecting images and document files
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image and Document files (*.jpg, *.jpeg, *.png, *.pdf, *.docx, *.txt)|*.jpg;*.jpeg;*.png;*.pdf;*.docx;*.txt",
                Multiselect = true // Allow multiple files to be selected
            };

            // If the user selected files, add them to the attachment list and display them
            if (openFileDialog.ShowDialog() == true)
            {
                // Add each selected file to the attachment list and display it
                openFileDialog.FileNames.ToList()
                                        .ForEach(fileName =>
                                        {
                                            _attachments.Add(fileName); // Add file to attachment list
                                            AddFileToImagePanel(fileName); // Display the file in the image panel
                                        });
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Adds a media file (with an icon and file name) to the ImagePanel for display.
        /// </summary>
        private void AddFileToImagePanel(string fileName)
        {
            string fileExtension = Path.GetExtension(fileName).ToLower(); // Get the file extension in lowercase
            BitmapImage iconImage = GetFileIcon(fileExtension); // Get the corresponding icon based on the file extension

            // Create a StackPanel to hold the file icon and name
            StackPanel docPanel = new StackPanel
            {
                Orientation = Orientation.Vertical, // Arrange items vertically
                Margin = new Thickness(5), // Add some margin
                HorizontalAlignment = HorizontalAlignment.Left // Align to the left
            };

            // Create an Image control to display the file icon
            Image documentIcon = new Image
            {
                Width = 50, // Set the width of the icon
                Height = 50, // Set the height of the icon
                Source = iconImage, // Set the image source to the file icon
                Margin = new Thickness(5) // Add margin around the icon
            };

            // Create a TextBlock to display the file name
            TextBlock docName = new TextBlock
            {
                Text = Path.GetFileName(fileName), // Display only the file name (without the path)
                Width = 100, // Set a fixed width
                TextAlignment = TextAlignment.Center, // Center-align the text
                TextWrapping = TextWrapping.Wrap // Wrap long file names
            };

            // Add the icon and file name to the StackPanel
            docPanel.Children.Add(documentIcon);
            docPanel.Children.Add(docName);

            // Add the StackPanel to the ImagePanel on the form
            ImagePanel.Children.Add(docPanel);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves the appropriate icon for a file based on its extension.
        /// </summary>
        private BitmapImage GetFileIcon(string fileExtension)
        {
            // Return the correct icon based on the file extension
            switch (fileExtension)
            {
                case ".pdf": return new BitmapImage(new Uri("pack://application:,,,/Images/pdf.png")); // PDF icon
                case ".docx": return new BitmapImage(new Uri("pack://application:,,,/Images/wordDocx.png")); // Word document icon
                case ".txt": return new BitmapImage(new Uri("pack://application:,,,/Images/txt.png")); // Text file icon
                case ".jpg":
                case ".jpeg":
                case ".png": return new BitmapImage(new Uri("pack://application:,,,/Images/image.png")); // Image icon
                default: return new BitmapImage(new Uri("pack://application:,,,/Images/default.png")); // Default icon for unknown file types
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the Back button click event. Hides the current page and returns to the previous screen.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            _mainWindow.MainFrame.Content = null; // Clear the content of MainFrame
            _mainWindow.MainFrame.Visibility = Visibility.Collapsed; // Hide the MainFrame
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Navigates to the event display page and generates event recommendations.
        /// </summary>
        private void NavigateToDisplay_Click(object sender, RoutedEventArgs e)
        {
            _viewModel.GenerateRecommendations(); // Generate event recommendations before navigating

            // Create a new display control and navigate to it
            var displayControl = new EventsAndAnnouncementsDisplay(_mainWindow, _viewModel); // Pass the ViewModel and MainWindow
            _mainWindow.MainFrame.Content = displayControl; // Set the content of MainFrame to the new display control
            _mainWindow.MainFrame.Visibility = Visibility.Visible; // Show the MainFrame
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Event handlers to provide real-time validation feedback

        /// <summary>
        /// Hides the event name error message when the text changes.
        /// </summary>
        private void EventNameTextBox_TextChanged(object sender, TextChangedEventArgs e) => HideError(EventNameError);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Hides the location error message when the text changes.
        /// </summary>
        private void LocationTextBox_TextChanged(object sender, TextChangedEventArgs e) => HideError(LocationError);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Hides the event date error message when a valid date is selected.
        /// </summary>
        private void EventDatePicker_SelectedDateChanged(object sender, RoutedEventArgs e) => HideError(EventDateError);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Hides the description error message when the text changes.
        /// </summary>
        private void DescriptionRichTextBox_TextChanged(object sender, TextChangedEventArgs e) => HideError(DescriptionError);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Hides the category error message when a valid category is selected.
        /// </summary>
        private void CategoryComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            // Hide error message if a valid category is selected
            if (CategoryComboBox.SelectedIndex > 0)
            {
                HideError(CategoryError);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Hides the specified error message.
        /// </summary>
        private void HideError(TextBlock errorLabel) => errorLabel.Visibility = Visibility.Collapsed;
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
