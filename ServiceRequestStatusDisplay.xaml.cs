using ST10029256_PROG7312.Classes; 
using System;
using System.Collections.ObjectModel; 
using System.Linq; 
using System.Windows; 
using System.Windows.Controls; 

namespace ST10029256_PROG7312
{
    /// <summary>
    /// ServiceRequestStatusDisplay UserControl displays service request statuses,
    /// allows filtering by urgency, and provides options to edit, delete, or track requests.
    /// </summary>
    public partial class ServiceRequestStatusDisplay : UserControl
    {
        private readonly RedBlackTree requestTree; // Red-Black Tree for efficient request management
        private ObservableCollection<ReportIssue> sortedReports; // ObservableCollection for displaying sorted reports

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes the ServiceRequestStatusDisplay with stored reports.
        /// </summary>
        /// <param name="storedReports">The collection of stored reports to display and manage.</param>
        public ServiceRequestStatusDisplay(ObservableCollection<ReportIssue> storedReports)
        {
            InitializeComponent(); // Initialize UI components
            requestTree = new RedBlackTree(); // Initialize the Red-Black Tree

            if (storedReports == null || !storedReports.Any()) // Check if stored reports are empty or null
            {
                sortedReports = new ObservableCollection<ReportIssue>(); // Initialize an empty collection
                serviceRequestsListView.ItemsSource = sortedReports; // Bind the empty collection to the ListView
                return;
            }

            foreach (var report in storedReports) // Insert all stored reports into the Red-Black Tree
            {
                requestTree.Insert(report);
            }

            sortedReports = new ObservableCollection<ReportIssue>(requestTree.InOrderTraversal()); // Get sorted reports
            serviceRequestsListView.ItemsSource = sortedReports; // Bind the sorted reports to the ListView
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Reloads all service requests and updates the ListView.
        /// </summary>
        private void LoadServiceRequests_Click(object sender, RoutedEventArgs e)
        {
            LoadServiceRequests();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void LoadServiceRequests()
        {
            var allReports = requestTree.InOrderTraversal(); // Retrieve all reports in order
            Console.WriteLine("Reloading service requests...");
            foreach (var report in allReports) // Log each report
            {
                Console.WriteLine($"Report ID: {report.RequestID}");
            }
            serviceRequestsListView.ItemsSource = new ObservableCollection<ReportIssue>(allReports); // Update the ListView
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Filters and displays only high-priority (urgent) requests.
        /// </summary>
        private void DisplayUrgentRequests_Click(object sender, RoutedEventArgs e)
        {
            var urgentReports = requestTree.InOrderTraversal()
                                           .Where(report => report.Priority == PriorityLevel.High); // Filter high-priority reports

            if (!urgentReports.Any()) // Check if there are no urgent requests
            {
                MessageBox.Show("No high-priority urgent requests found.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            serviceRequestsListView.ItemsSource = new ObservableCollection<ReportIssue>(urgentReports); // Display urgent reports
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Displays related requests based on the entered Request ID.
        /// </summary>
        private void DisplayRelatedRequestsButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(trackRequestIdTextBox.Text)) // Validate input
            {
                MessageBox.Show("Please enter a valid Request ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var trackingId = trackRequestIdTextBox.Text.Trim(); // Get the entered Request ID
            var startReport = requestTree.Search(trackingId); // Search for the report in the tree

            if (startReport != null) // If the report is found
            {
                var relatedReports = requestTree.InOrderTraversal()
                    .Where(report =>
                        (report.Category == startReport.Category || report.DateOfIssue.Date == startReport.DateOfIssue.Date)
                        && report.RequestID != startReport.RequestID); // Filter related reports by category or date

                if (relatedReports.Any()) // Check if related reports exist
                {
                    serviceRequestsListView.ItemsSource = new ObservableCollection<ReportIssue>(relatedReports); // Display related reports
                }
                else
                {
                    MessageBox.Show("No related requests found.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
                }
            }
            else
            {
                MessageBox.Show("Request ID not found. Please enter a valid ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Tracks a specific request by Request ID and displays it in the ListView.
        /// </summary>
        private void TrackRequest_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(trackRequestIdTextBox.Text)) // Validate input
            {
                MessageBox.Show("Please enter a valid Request ID.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return;
            }

            var trackingId = trackRequestIdTextBox.Text.Trim(); // Get the entered Request ID
            var report = requestTree.Search(trackingId); // Search for the report in the tree

            if (report != null) // If the report is found
            {
                serviceRequestsListView.ItemsSource = new ObservableCollection<ReportIssue> { report }; // Display the report
            }
            else
            {
                MessageBox.Show("Request not found.", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Navigates to the EditRequest page for the selected report.
        /// </summary>
        private void EditReport_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ReportIssue reportIssue) // Check if the sender is a button
            {
                var mainWindow = Application.Current.MainWindow as MainWindow; // Get the main window instance
                if (mainWindow != null)
                {
                    var editRequestPage = new EditRequest(reportIssue); // Create the EditRequest page
                    mainWindow.MainFrame.Content = editRequestPage; // Navigate to the EditRequest page
                }
            }
            else
            {
                MessageBox.Show("Error: Unable to load the report for editing.", "Edit Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Deletes a report from the collection and updates the ListView.
        /// </summary>
        private void DeleteReport_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.DataContext is ReportIssue report) // Check if the sender is a button
            {
                Console.WriteLine($"Request to delete: {report.RequestID}");

                requestTree.Delete(report.RequestID); // Delete the report from the Red-Black Tree

                var reportToRemove = sortedReports.FirstOrDefault(r => r.RequestID == report.RequestID); // Find the report in the ObservableCollection
                if (reportToRemove != null)
                {
                    sortedReports.Remove(reportToRemove); // Remove the report from the collection
                    Console.WriteLine($"Removed from ObservableCollection: {report.RequestID}");
                }

                var mainWindow = Application.Current.MainWindow as MainWindow; // Sync with main window's stored reports
                if (mainWindow != null)
                {
                    var storedReportToRemove = mainWindow.StoredReports.FirstOrDefault(r => r.RequestID == report.RequestID);
                    if (storedReportToRemove != null)
                    {
                        mainWindow.StoredReports.Remove(storedReportToRemove); // Remove the report from main window's collection
                    }
                }

                serviceRequestsListView.ItemsSource = null; // Refresh the ListView
                serviceRequestsListView.ItemsSource = sortedReports;

                MessageBox.Show("Report deleted successfully.", "Delete", MessageBoxButton.OK, MessageBoxImage.Information);
            }
            else
            {
                MessageBox.Show("Error: Unable to identify the report to delete.", "Delete Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void trackRequestIdTextBox_GotFocus(object sender, RoutedEventArgs e)
        {
            if (trackRequestIdTextBox.Text == "Enter Request ID") // Clear placeholder text
            {
                trackRequestIdTextBox.Text = string.Empty;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private void trackRequestIdTextBox_LostFocus(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(trackRequestIdTextBox.Text)) // Restore placeholder text if input is empty
            {
                trackRequestIdTextBox.Text = "Enter Request ID";
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Navigates back to the main page and refreshes the data.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow; // Get the main window instance
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = null; // Navigate back to the main screen
                mainWindow.MainFrame.Visibility = Visibility.Collapsed; // Hide the main frame

                sortedReports = new ObservableCollection<ReportIssue>(mainWindow.StoredReports); // Refresh the reports
                serviceRequestsListView.ItemsSource = sortedReports; // Update the ListView
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
