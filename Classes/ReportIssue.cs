using ST10029256_PROG7312.Classes; // Namespace reference for accessing related classes
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ST10029256_PROG7312
{
    // Represents a report issue submitted by a user
    public class ReportIssue
    {
        public string RequestID { get; set; } // Unique identifier for the issue
        public string Location { get; set; } // Location where the issue occurred
        public string Category { get; set; } // Category of the issue (e.g., maintenance, utilities)
        public string IssueDescription { get; set; } // Detailed description of the issue
        public DateTime DateSubmitted { get; set; } // Date when the issue was submitted
        public DateTime DateOfIssue { get; set; } // Date when the issue occurred
        public ObservableCollection<string> Attachments { get; set; } = new ObservableCollection<string>(); // Collection of file paths for attachments
        public string Status { get; set; } // Current status of the issue (e.g., Open, In Progress, Resolved)
        public PriorityLevel Priority { get; set; } // Priority level of the issue (e.g., High, Medium, Low)
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
