using System;
using System.Collections.ObjectModel;

namespace ST10029256_PROG7312.Classes
{
    /// <summary>
    /// Represents a binary search tree for storing and managing ReportIssue objects.
    /// </summary>
    public class BinarySearchTree
    {
        // Internal node class representing each node in the binary search tree
        private class BinaryTreeNode
        {
            public ReportIssue Data { get; set; } // Data stored in the node
            public BinaryTreeNode Left { get; set; } // Left child of the node
            public BinaryTreeNode Right { get; set; } // Right child of the node

            //---------------------------------------------------------------------------------------------------------------------------------------------//

            // Constructor to initialize a node with data
            public BinaryTreeNode(ReportIssue data)
            {
                Data = data;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private BinaryTreeNode root; // Root node of the binary search tree

        /// <summary>
        /// Inserts a new ReportIssue into the binary search tree.
        /// Throws an exception for null or invalid ReportIssue objects.
        /// </summary>
        public void Insert(ReportIssue report)
        {
            if (report == null || string.IsNullOrWhiteSpace(report.RequestID))
                throw new ArgumentException("Invalid report or missing RequestID."); // Validate input

            root = InsertRec(root, report); // Start recursive insertion
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Recursive method to insert a ReportIssue into the tree
        private BinaryTreeNode InsertRec(BinaryTreeNode node, ReportIssue report)
        {
            // Base case: Insert the new node at the correct position
            if (node == null) return new BinaryTreeNode(report);

            // Compare the new report's RequestID with the current node's RequestID
            int comparison = string.Compare(report.RequestID, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);
            if (comparison < 0)
                node.Left = InsertRec(node.Left, report); // Traverse the left subtree
            else if (comparison > 0)
                node.Right = InsertRec(node.Right, report); // Traverse the right subtree
            else
                throw new InvalidOperationException("Duplicate RequestID detected!"); // Prevent duplicate RequestID

            return node; // Return the current node after insertion
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Searches for a ReportIssue by its RequestID.
        /// Returns the ReportIssue if found, otherwise null.
        /// </summary>
        public ReportIssue Search(string requestId)
        {
            if (string.IsNullOrWhiteSpace(requestId))
                throw new ArgumentException("RequestID cannot be null or empty."); // Validate input

            return SearchRec(root, requestId); // Start recursive search
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Recursive method to search for a ReportIssue by RequestID
        private ReportIssue SearchRec(BinaryTreeNode node, string requestId)
        {
            if (node == null) return null; // Base case: Not found

            // Compare the search RequestID with the current node's RequestID
            int comparison = string.Compare(requestId, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);
            if (comparison == 0) return node.Data; // Found the ReportIssue
            if (comparison < 0) return SearchRec(node.Left, requestId); // Search in the left subtree
            return SearchRec(node.Right, requestId); // Search in the right subtree
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves all reports in sorted order using in-order traversal.
        /// </summary>
        public ObservableCollection<ReportIssue> InOrderTraversal()
        {
            var results = new ObservableCollection<ReportIssue>(); // Collection to store results
            InOrderRec(root, results); // Start recursive in-order traversal
            return results;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Recursive method for in-order traversal
        private void InOrderRec(BinaryTreeNode node, ObservableCollection<ReportIssue> results)
        {
            if (node == null) return; // Base case: No node to traverse

            InOrderRec(node.Left, results); // Traverse the left subtree
            results.Add(node.Data); // Add current node's data to results
            InOrderRec(node.Right, results); // Traverse the right subtree
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Deletes a ReportIssue by its RequestID.
        /// Ensures the tree remains valid after deletion.
        /// </summary>
        public void Delete(string requestId)
        {
            if (string.IsNullOrWhiteSpace(requestId))
                throw new ArgumentException("RequestID cannot be null or empty."); // Validate input

            root = DeleteRec(root, requestId); // Start recursive deletion
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Recursive method to delete a node by RequestID
        private BinaryTreeNode DeleteRec(BinaryTreeNode node, string requestId)
        {
            if (node == null) return null; // Base case: Node not found

            // Compare the RequestID with the current node's RequestID
            int comparison = string.Compare(requestId, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);
            if (comparison < 0)
                node.Left = DeleteRec(node.Left, requestId); // Traverse the left subtree
            else if (comparison > 0)
                node.Right = DeleteRec(node.Right, requestId); // Traverse the right subtree
            else
            {
                // Node found: Handle different cases for deletion
                if (node.Left == null) return node.Right; // No left child
                if (node.Right == null) return node.Left; // No right child

                // Node has two children: Replace with the smallest value in the right subtree
                node.Data = MinValue(node.Right);
                node.Right = DeleteRec(node.Right, node.Data.RequestID); // Delete the inorder successor
            }

            return node; // Return the modified node
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Finds the smallest value (inorder successor) in a subtree
        private ReportIssue MinValue(BinaryTreeNode node)
        {
            var current = node; // Start from the given node
            while (current.Left != null)
            {
                current = current.Left; // Traverse to the leftmost node
            }
            return current.Data; // Return the data of the leftmost node
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
