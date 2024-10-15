using System;
using System.Collections.Generic;
using System.Linq; 
using System.Windows;
using System.Windows.Controls;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// Interaction logic for ReportIssuesDisplay.xaml
    /// </summary>
    public partial class ReportIssuesDisplay : UserControl
    {

        private List<ReportIssue> reportIssues; // List to hold the reports

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes the ReportIssuesDisplay and loads the stored reports.
        /// </summary>
        public ReportIssuesDisplay(List<ReportIssue> storedReports)
        {
            InitializeComponent();
            reportIssues = storedReports; // Assign the list of reports to the local field
            LoadReports(); // Load the reports into the ItemsControl
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Loads and binds the reports to the ItemsControl, or displays a "No Reports" message if none exist.
        /// </summary>
        private void LoadReports()
        {
            // Check if there are any reports
            var hasReports = reportIssues?.Any() ?? false;

            if (hasReports)
            {
                // If there are reports, bind the list to the ItemsControl
                ReportsListView.ItemsSource = reportIssues;
                NoReportsMessage.Visibility = Visibility.Collapsed; // Hide "No Reports" message
            }
            else
            {
                // If no reports, clear the ItemsControl and show the "No Reports" message
                ReportsListView.ItemsSource = null; // Clear the ItemsSource
                NoReportsMessage.Visibility = Visibility.Visible; // Show "No Reports" message
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the "Back to Report" button to navigate back to the ReportIssues page.
        /// </summary>
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            var mainWindow = Application.Current.MainWindow as MainWindow;
            if (mainWindow != null)
            {
                mainWindow.MainFrame.Content = new ReportIssues(reportIssues); // Navigate back
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
