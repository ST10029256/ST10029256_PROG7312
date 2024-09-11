using System.Collections.Generic;
using System.Windows.Controls;

namespace ST10029256_PROG7312
{
    /// <summary>
    /// This class displays the list of submitted reports.
    /// It receives a list of stored reports and populates a ListView with them.
    /// </summary>
    public partial class ReportIssuesDisplay : UserControl
    {
        // This list holds the reports passed to this control.
        private List<ReportIssue> reportIssues;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Constructor for the ReportIssuesDisplay.
        /// Initializes the component and loads the passed-in reports.
        /// </summary>
        public ReportIssuesDisplay(List<ReportIssue> storedReports)
        {
            InitializeComponent(); // Initialize the UserControl components
            reportIssues = storedReports; // Assign the list of reports to the local field
            LoadReports(); // Load the reports into the ListView
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// This method binds the report list to the ListView to display all submitted reports.
        /// </summary>
        private void LoadReports()
        {
            // Bind the report list to the ListView to show the reports in the UI
            ReportsListView.ItemsSource = reportIssues;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the back button click.
        /// This hides the current form and returns the user to the previous screen.
        /// </summary>
        private void BackBtn_Click(object sender, System.Windows.RoutedEventArgs e)
        {
            // Hide this UserControl and return to the previous view
            this.Visibility = System.Windows.Visibility.Collapsed;
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
