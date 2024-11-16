using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ST10029256_PROG7312.Classes
{
    /// <summary>
    /// Represents a node in the Red-Black Tree.
    /// </summary>
    public class RedBlackTreeNode
    {
        public ReportIssue Data { get; set; } // Holds the report data for the node
        public RedBlackTreeNode Left { get; set; } // Left child node
        public RedBlackTreeNode Right { get; set; } // Right child node
        public bool IsRed { get; set; } // Indicates whether the node is red or black

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes a new Red-Black Tree node with the given report data.
        /// </summary>
        public RedBlackTreeNode(ReportIssue data)
        {
            Data = data;
            IsRed = true; // New nodes are red by default
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
