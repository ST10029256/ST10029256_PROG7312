using System.Collections.ObjectModel;
using System.Windows.Controls;
using System.Windows;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// EditRequest UserControl allows users to update the status and description
    /// of a specific report. It raises an event to notify listeners about the update.
    /// </summary>
    public partial class EditRequest : UserControl
    {
        private ReportIssue _report; // The report being edited

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Delegate for the ReportUpdated event, used to notify listeners about report updates.
        /// </summary>
        public delegate void ReportUpdatedEventHandler(ReportIssue updatedReport);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event triggered when a report is updated.
        /// </summary>
        public event ReportUpdatedEventHandler ReportUpdated;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes the EditRequest UserControl with the given report.
        /// </summary>
        public EditRequest(ReportIssue report)
        {
            InitializeComponent(); // Initialize UI components
            _report = report; // Assign the report to the local field

            // Populate fields with the current values of the report
            StatusComboBox.SelectedItem = new ComboBoxItem { Content = _report.Status }; // Set current status
            CompletionDescriptionTextBox.Text = _report.IssueDescription; // Set current description
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Saves the changes made to the report and navigates back to the ServiceRequestStatusDisplay.
        /// </summary>
        private void SaveChanges_Click(object sender, RoutedEventArgs e)
        {
            // Update the report's status and description with user input
            _report.Status = (StatusComboBox.SelectedItem as ComboBoxItem)?.Content.ToString(); // Update status
            _report.IssueDescription = CompletionDescriptionTextBox.Text; // Update description

            // Raise the ReportUpdated event
            ReportUpdated?.Invoke(_report);

            // Display success message to the user
            MessageBox.Show("Status updated successfully", "Update", MessageBoxButton.OK, MessageBoxImage.Information);

            // Navigate back to the ServiceRequestStatusDisplay
            var mainWindow = Application.Current.MainWindow as MainWindow; // Get the main window instance
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new ServiceRequestStatusDisplay(
                    new ObservableCollection<ReportIssue>(mainWindow.StoredReports)); // Refresh data and navigate back
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the Back button click event to navigate back to the ServiceRequestStatusDisplay.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            // Navigate back to the ServiceRequestStatusDisplay
            var mainWindow = Application.Current.MainWindow as MainWindow; // Get the main window instance
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new ServiceRequestStatusDisplay(
                    new ObservableCollection<ReportIssue>(mainWindow.StoredReports)); // Refresh data and navigate back
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//