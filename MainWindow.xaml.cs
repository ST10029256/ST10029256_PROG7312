using ST10029256_PROG7312.Classes;
using ST10029256_PROG7312.Helpers;
using ST10029256_PROG7312.UserControls;
using ST10029256_PROG7312.ViewModels; 
using System.Collections.Generic;
using System.Linq;
using System.Windows;

namespace ST10029256_PROG7312
{
    public partial class MainWindow : Window
    {
        // Property to hold stored reports
        public List<ReportIssue> StoredReports { get; private set; }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // ViewModel to manage events and categories
        private EventsViewModel _viewModel;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        public MainWindow()
        {
            InitializeComponent();

            // Initialize the ViewModel and stored reports
            _viewModel = new EventsViewModel();
            StoredReports = new List<ReportIssue>();

            LoadCategories(); // Load categories into ViewModel

            // Hide the main frame initially
            MainFrame.Visibility = Visibility.Collapsed;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Loads predefined categories into the ViewModel.
        /// </summary>
        private void LoadCategories()
        {
            // Clear existing categories and populate new ones in the ViewModel
            _viewModel.Categories.Clear();
            _viewModel.Categories.AddRange(new[]
            {
                "Select a Category",
                "Concert",
                "Festival",
                "Workshop",
                "Conference",
                "Community Meeting",
                "Sports Event",
                "Other"
            });
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Adds a new event to the ViewModel.
        /// </summary>
        /// <param name="newEvent">The event to add.</param>
        public void AddEvent(LocalEvent newEvent)
        {
            newEvent?.Let(e => _viewModel.AddEvent(e)); // Using null-coalescing and lambda expression
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves available events from the ViewModel.
        /// </summary>
        public List<LocalEvent> GetEvents()
        {
            return _viewModel.AvailableEvents.ToList(); // Convert the observable collection to a List
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves unique categories from the ViewModel.
        /// </summary>
        public List<string> GetUniqueCategories()
        {
            // Return distinct categories
            return _viewModel.Categories.Distinct().ToList();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the "Events & Announcements" button click event and displays the EventsAndAnnouncements page.
        /// </summary>
        private void Events_Announcements_Click(object sender, RoutedEventArgs e)
        {
            var eventsAndAnnouncements = new EventsAndAnnouncements(this, _viewModel); // Pass the same ViewModel
            MainFrame.Content = eventsAndAnnouncements; // Set the content of MainFrame to the events page
            MainFrame.Visibility = Visibility.Visible; // Make the MainFrame visible
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the "Report Issues" button click event and displays the ReportIssues page.
        /// </summary>
        private void ReportIssues_Click(object sender, RoutedEventArgs e)
        {
            var reportIssuesPage = new ReportIssues(StoredReports); // Create a new ReportIssues page passing stored reports
            MainFrame.Content = reportIssuesPage; // Set the content of MainFrame to the report issues page
            MainFrame.Visibility = Visibility.Visible; // Make the MainFrame visible
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Placeholder event handler for the "Service Status" button click.
        /// </summary>
        private void ServiceStatus_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Service Status page is under construction."); // Display message that the page is under construction
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------//

    // Extension method to simplify null checks and lambda usage
    public static class ExtensionMethods
    {
        /// <summary>
        /// Executes the specified action if the object is not null.
        /// </summary>
        public static void Let<T>(this T obj, System.Action<T> action)
        {
            if (obj != null)
            {
                action(obj);
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
