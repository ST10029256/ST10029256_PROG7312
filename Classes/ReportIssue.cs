using System.Collections.Generic;

namespace ST10029256_PROG7312
{
    // Class representing the details of a report submitted by a user
    public class ReportIssue
    {
        // Property to store the location of the reported issue
        public string Location { get; set; }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Property to store the category of the issue (e.g., Sanitation, Roads)
        public string Category { get; set; }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Property to store the description of the issue provided by the user
        public string IssueDescription { get; set; }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // List to store the file paths of attached images or documents related to the issue
        public List<string> Attachments { get; set; }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Constructor initializes the Attachments list
        public ReportIssue()
        {
            Attachments = new List<string>(); // Initialize the list of attachments when a report is created
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
