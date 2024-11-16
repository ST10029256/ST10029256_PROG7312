using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Windows;
using System.Windows.Controls;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// ReportIssuesDisplay UserControl displays a collection of reports in a list view.
    /// It provides navigation back to the ReportIssues page.
    /// </summary>
    public partial class ReportIssuesDisplay : UserControl
    {
        private ObservableCollection<ReportIssue> reportCollection; // Collection of reports to be displayed

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes the ReportIssuesDisplay with a collection of reports.
        /// </summary>
        /// <param name="reports">The collection of reports to display.</param>
        public ReportIssuesDisplay(ObservableCollection<ReportIssue> reports)
        {
            InitializeComponent(); // Initialize UI components
            reportCollection = reports; // Assign the collection to the local field
            LoadReports(); // Load the reports into the UI
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Loads the reports into the ListView. Displays a message if no reports are available.
        /// </summary>
        private void LoadReports()
        {
            if (reportCollection.Count > 0) // Check if there are reports to display
            {
                ReportsListView.ItemsSource = reportCollection; // Bind the collection to the ListView
                NoReportsMessage.Visibility = Visibility.Collapsed; // Hide the "No Reports" message
            }
            else
            {
                NoReportsMessage.Visibility = Visibility.Visible; // Show the "No Reports" message
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Handles the Back button click event to navigate back to the ReportIssues page.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow; // Get the main window instance
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new ReportIssues(mainWindow.StoredReports); // Navigate back to the ReportIssues page
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//