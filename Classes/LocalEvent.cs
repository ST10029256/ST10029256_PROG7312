using System;
using System.Collections.Generic;

namespace ST10029256_PROG7312.Classes
{
    // Represents a local event that can be displayed or managed within the application
    public class LocalEvent
    {
        // The name of the event
        public string Name { get; set; }

        // The category of the event (e.g., Concert, Workshop, etc.)
        public string Category { get; set; }

        // The date of the event
        public DateTime Date { get; set; }

        // The location where the event will be held
        public string Location { get; set; }

        // A brief description of the event, outlining its details
        public string Description { get; set; }

        // A list of file attachments related to the event, such as images or documents
        public List<string> Attachments { get; set; }

        // Constructor that initializes the Attachments list
        public LocalEvent()
        {
            // Initialize the Attachments list to prevent null reference issues when adding files
            Attachments = new List<string>();
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
