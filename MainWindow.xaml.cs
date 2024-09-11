using System;
using System.Collections.Generic;
using System.Windows;
using System.Windows.Navigation;

namespace ST10029256_PROG7312
{
    public partial class MainWindow : Window
    {
        /// <summary>
        /// List to store all report submissions across the application. 
        /// This list will be passed to various user controls to handle report submission and display.
        /// </summary>
        private List<ReportIssue> storedReports;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Constructor for MainWindow.
        /// Initializes the list of reports and sets up the main window.
        /// </summary>
        public MainWindow()
        {
            InitializeComponent();
            storedReports = new List<ReportIssue>(); // Initialize the list to store submitted reports
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the "Report Issues" button.
        /// Displays the ReportIssues UserControl and passes the stored reports list to it.
        /// </summary>
        private void ReportIssues_Click(object sender, RoutedEventArgs e)
        {
            ReportIssues reportIssues = new ReportIssues(storedReports); // Create a new instance of ReportIssues UserControl and pass the reports list
            MainFrame.Content = reportIssues; // Display the ReportIssues control in the MainFrame
            MainFrame.Visibility = Visibility.Visible; // Make the MainFrame visible
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the "Report Issues Display" button.
        /// Displays the ReportIssuesDisplay UserControl to view previously submitted reports.
        /// </summary>
        private void ReportIssuesDisplay_Click(object sender, RoutedEventArgs e)
        {
            ReportIssuesDisplay reportIssuesDisplay = new ReportIssuesDisplay(storedReports); // Create a new instance of ReportIssuesDisplay UserControl and pass the reports list
            MainFrame.Content = reportIssuesDisplay; // Display the ReportIssuesDisplay control in the MainFrame
            MainFrame.Visibility = Visibility.Visible; // Make the MainFrame visible
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the "Service Status" button.
        /// </summary>
        private void ServiceStatus_Click(object sender, RoutedEventArgs e)
        {

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for the "Events & Announcements" button.
        /// </summary>
        private void Events_Announcements_Click(object sender, RoutedEventArgs e)
        {

        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Event handler for when the MainFrame navigates to a new page or user control.
        /// </summary>
        private void MainFrame_Navigated(object sender, NavigationEventArgs e)
        {

        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
