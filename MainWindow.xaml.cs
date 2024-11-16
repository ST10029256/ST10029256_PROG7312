using ST10029256_PROG7312.Classes; 
using ST10029256_PROG7312.Helpers;
using ST10029256_PROG7312.UserControls; 
using ST10029256_PROG7312.ViewModels; 
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Windows;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// The MainWindow class serves as the entry point for the application's UI.
    /// It manages interactions between various components, including user controls,
    /// ViewModels, and global data like reports and events.
    /// </summary>
    public partial class MainWindow : Window
    {
        /// <summary>
        /// Collection of all stored report issues.
        /// </summary>
        public ObservableCollection<ReportIssue> StoredReports { get; private set; }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// ViewModel for managing event-related data.
        /// </summary>
        private EventsViewModel _viewModel;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Global Red-Black Tree structure for managing request data.
        /// </summary>
        public RedBlackTree GlobalRequestTree { get; private set; } = new RedBlackTree();

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Global collection of sorted reports for use across the application.
        /// </summary>
        public ObservableCollection<ReportIssue> GlobalSortedReports { get; private set; } = new ObservableCollection<ReportIssue>();

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes the MainWindow instance, setting up ViewModels, loading categories and reports,
        /// and configuring the main frame visibility.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent(); // Initialize UI components
            _viewModel = new EventsViewModel(); // Instantiate the ViewModel
            StoredReports = new ObservableCollection<ReportIssue>(); // Initialize the collection of reports

            LoadCategories(); // Load predefined categories into the ViewModel
            LoadReports(); // Initialize the display of reports
            MainFrame.Visibility = Visibility.Collapsed; // Set the main frame to be hidden initially
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Loads predefined categories into the ViewModel.
        /// </summary>
        private void LoadCategories()
        {
            _viewModel.Categories.Clear(); // Clear any existing categories in the ViewModel
            _viewModel.Categories.AddRange(new[] // Add a predefined set of categories
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
        /// Refreshes the displayed reports by setting the content of the main frame to the ReportIssuesDisplay control.
        /// </summary>
        private void LoadReports()
        {
            // Create a new ReportIssuesDisplay user control and assign it to the main frame
            var reportIssuesDisplay = new ReportIssuesDisplay(StoredReports);
            MainFrame.Content = reportIssuesDisplay; // Set the content to the ReportIssuesDisplay control
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Adds a new event to the ViewModel.
        /// </summary>
        /// <param name="newEvent">The event to add.</param>
        public void AddEvent(LocalEvent newEvent)
        {
            if (newEvent != null)
            {
                _viewModel.AddEvent(newEvent); // Add the event to the ViewModel
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves all available events from the ViewModel.
        /// </summary>
        /// <returns>A list of available events.</returns>
        public List<LocalEvent> GetEvents()
        {
            return _viewModel.AvailableEvents.ToList(); // Return a list of all available events
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves a list of unique categories from the ViewModel.
        /// </summary>
        /// <returns>A list of unique categories.</returns>
        public List<string> GetUniqueCategories()
        {
            return _viewModel.Categories.Distinct().ToList(); // Return a distinct list of categories
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the "Events and Announcements" button click.
        /// Navigates to the Events and Announcements page.
        /// </summary>
        private void Events_Announcements_Click(object sender, RoutedEventArgs e)
        {
            // Create and navigate to the EventsAndAnnouncements user control
            var eventsAndAnnouncements = new EventsAndAnnouncements(this, _viewModel);
            MainFrame.Content = eventsAndAnnouncements; // Set the content to the EventsAndAnnouncements control
            MainFrame.Visibility = Visibility.Visible; // Make the main frame visible
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the "Report Issues" button click.
        /// Navigates to the Report Issues page.
        /// </summary>
        private void ReportIssues_Click(object sender, RoutedEventArgs e)
        {
            // Create and navigate to the ReportIssues user control
            var reportIssuesPage = new ReportIssues(StoredReports);
            MainFrame.Content = reportIssuesPage; // Set the content to the ReportIssues control
            MainFrame.Visibility = Visibility.Visible; // Make the main frame visible
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the "Service Status" button click.
        /// Navigates to the Service Request Status page.
        /// </summary>
        private void ServiceStatus_Click(object sender, RoutedEventArgs e)
        {
            // Create and navigate to the ServiceRequestStatusDisplay user control
            var serviceStatusPage = new ServiceRequestStatusDisplay(StoredReports);
            MainFrame.Content = serviceStatusPage; // Set the content to the ServiceRequestStatusDisplay control
            MainFrame.Visibility = Visibility.Visible; // Make the main frame visible
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Opens the Edit Request page for the selected report.
        /// </summary>
        /// <param name="report">The report to edit.</param>
        private void OpenEditRequest(ReportIssue report)
        {
            // Create and navigate to the EditRequest user control
            var editRequestPage = new EditRequest(report);
            editRequestPage.ReportUpdated += OnReportUpdated; // Subscribe to the ReportUpdated event
            MainFrame.Content = editRequestPage; // Set the content to the EditRequest control
            MainFrame.Visibility = Visibility.Visible; // Make the main frame visible
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Updates the report in the StoredReports collection and refreshes the reports display.
        /// </summary>
        /// <param name="updatedReport">The updated report details.</param>
        private void OnReportUpdated(ReportIssue updatedReport)
        {
            // Find the report in the collection with the matching RequestID
            var report = StoredReports.FirstOrDefault(r => r.RequestID == updatedReport.RequestID);
            if (report != null)
            {
                report.Status = updatedReport.Status; // Update the status of the report
                report.IssueDescription = updatedReport.IssueDescription; // Update the issue description
            }

            LoadReports(); // Refresh the reports display
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
