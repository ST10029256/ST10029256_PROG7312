using ST10029256_PROG7312.ViewModels;
using ST10029256_PROG7312.Classes;
using System.Windows;
using System.Windows.Controls;
using System.Collections.ObjectModel;
using System;
using System.Linq;

namespace ST10029256_PROG7312.UserControls
{
    public partial class EventsAndAnnouncementsDisplay : UserControl
    {
        private EventsViewModel _viewModel; // Reference to the ViewModel managing events

        private MainWindow _mainWindow; // Reference to the MainWindow for navigation

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Constructor that initializes the EventsAndAnnouncementsDisplay control and sets up data binding.
        /// </summary>
        public EventsAndAnnouncementsDisplay(MainWindow mainWindow, EventsViewModel viewModel)
        {
            InitializeComponent();
            _mainWindow = mainWindow; // Set reference to MainWindow
            _viewModel = viewModel; // Assign the passed ViewModel instance
            this.DataContext = _viewModel; // Set DataContext for data binding with ViewModel

            CategoriesSetup(); // Initialize the categories ComboBox

            // Display a message if no events are available using a conditional lambda expression
            NoEventsMessage.Visibility = _viewModel.AvailableEvents.Any() ? Visibility.Collapsed : Visibility.Visible;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Sets up the categories ComboBox by binding the categories from the ViewModel.
        /// </summary>
        private void CategoriesSetup()
        {
            // Set the ComboBox data source and select "Select a Category" as default
            CategoryCbx.ItemsSource = _viewModel.Categories;
            CategoryCbx.SelectedIndex = 0; // Set default selection to "Select a Category"
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the GotFocus event for the search box, clearing the placeholder text when the user focuses on it.
        /// </summary>
        private void SearchTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (SearchTextBox.Text == "Search for events...")
            {
                SearchTextBox.Text = string.Empty; // Clear the placeholder text
                SearchTextBox.Foreground = System.Windows.Media.Brushes.Black; // Change text color to black
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the LostFocus event for the search box, resetting the placeholder text if the field is empty.
        /// </summary>
        private void SearchTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(SearchTextBox.Text))
            {
                // If the search box is empty, set the placeholder text back
                SearchTextBox.Text = "Search for events...";
                SearchTextBox.Foreground = System.Windows.Media.Brushes.Gray; // Change text color to gray
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the KeyUp event for the search box to filter events as the user types.
        /// </summary>
        private void SearchTextBox_KeyUp(object sender, System.Windows.Input.KeyEventArgs e)
        {
            FilterEvents(); // Call FilterEvents whenever the user types in the search box
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the Category selection changed event to filter events based on the selected category.
        /// </summary>
        private void CategoryCbx_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            FilterEvents(); // Filter events when the user changes the category
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the DatePicker selection changed event to filter events based on the selected date.
        /// </summary>
        private void DateFilter_SelectedDateChanged(object sender, RoutedEventArgs e)
        {
            FilterEvents(); // Filter events when the user selects a date
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Filters the events based on the search text, selected category, and selected date.
        /// </summary>
        private void FilterEvents()
        {
            // Ensure that the ViewModel is not null
            if (_viewModel == null)
            {
                MessageBox.Show("Error: ViewModel is not initialized.");
                return;
            }

            // Extract search text, trim any leading/trailing whitespace
            string searchText = SearchTextBox.Text == "Search for events..." ? string.Empty : SearchTextBox.Text.Trim();

            // Retrieve the selected category, null if "Select a Category" is selected
            string selectedCategory = CategoryCbx.SelectedItem as string;
            selectedCategory = selectedCategory == "Select a Category" ? null : selectedCategory;

            // Retrieve the selected date from DatePicker
            DateTime? selectedDate = DateFilter.SelectedDate;

            // Use the ViewModel's FilterEvents method to apply filters (searchText, category, and date)
            _viewModel.FilterEvents(searchText, selectedCategory, selectedDate);

            // Update NoEventsMessage visibility based on whether there are any filtered events
            NoEventsMessage.Visibility = _viewModel.FilteredEvents.Any() ? Visibility.Collapsed : Visibility.Visible;

            // After filtering, generate event recommendations
            _viewModel.GenerateRecommendations();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the Back button, which navigates back to the EventsAndAnnouncements control.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Create a new instance of EventsAndAnnouncements and display it
            var eventsAndAnnouncementsControl = new EventsAndAnnouncements(_mainWindow, _viewModel);
            _mainWindow.MainFrame.Content = eventsAndAnnouncementsControl; // Set the new control as the content of MainFrame
            _mainWindow.MainFrame.Visibility = Visibility.Visible; // Ensure the frame is visible
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//