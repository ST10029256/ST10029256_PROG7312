using System;
using System.Collections.ObjectModel;

namespace ST10029256_PROG7312.Classes
{
    public class AVLTree
    {
        // Represents a node in the AVL tree
        private class AVLNode
        {
            public ReportIssue Data { get; set; } // The data held by the node
            public AVLNode Left { get; set; } // Reference to the left child
            public AVLNode Right { get; set; } // Reference to the right child
            public int Height { get; set; } // Height of the node

            //---------------------------------------------------------------------------------------------------------------------------------------------//

            // Constructor for AVLNode
            public AVLNode(ReportIssue data)
            {
                Data = data;
                Height = 1; // Height is initialized to 1 when the node is created
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        private AVLNode root; // Root node of the AVL tree

        //---------------------------------------------------------------------------------------------------------------------------------------------//



        /// <summary>
        /// Inserts a new report into the AVL tree.
        /// Ensures the tree remains balanced after the insertion.
        /// </summary>
        public void Insert(ReportIssue report)
        {
            root = InsertRec(root, report); // Start the recursive insertion from the root
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Recursive method to insert a report into the tree
        private AVLNode InsertRec(AVLNode node, ReportIssue report)
        {
            // Base case: If node is null, create a new node
            if (node == null) return new AVLNode(report);

            // Compare the new report's RequestID with the current node's RequestID
            int comparison = string.Compare(report.RequestID, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);

            if (comparison < 0)
                node.Left = InsertRec(node.Left, report); // Insert into the left subtree
            else if (comparison > 0)
                node.Right = InsertRec(node.Right, report); // Insert into the right subtree
            else
                throw new InvalidOperationException("Duplicate RequestID detected!"); // Prevent duplicate RequestID

            // Update the height of the current node
            node.Height = 1 + Math.Max(GetHeight(node.Left), GetHeight(node.Right));

            // Balance the node to ensure AVL property
            return Balance(node);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Searches for a report in the AVL tree by RequestID.
        /// Returns the report if found, otherwise null.
        /// </summary>
        public ReportIssue Search(string requestId)
        {
            return SearchRec(root, requestId); // Start the recursive search from the root
        }

        // Recursive method to search for a report by RequestID
        private ReportIssue SearchRec(AVLNode node, string requestId)
        {
            if (node == null) return null; // Base case: Not found

            // Compare the search RequestID with the current node's RequestID
            int comparison = string.Compare(requestId, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);

            if (comparison == 0)
                return node.Data; // Found the report
            if (comparison < 0)
                return SearchRec(node.Left, requestId); // Search in the left subtree
            return SearchRec(node.Right, requestId); // Search in the right subtree
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves all reports in the tree in sorted order (in-order traversal).
        /// </summary>
        public ObservableCollection<ReportIssue> InOrderTraversal()
        {
            var results = new ObservableCollection<ReportIssue>(); // Collection to hold the results
            InOrderRec(root, results); // Perform the recursive in-order traversal
            return results;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Recursive method for in-order traversal
        private void InOrderRec(AVLNode node, ObservableCollection<ReportIssue> results)
        {
            if (node == null) return; // Base case: If node is null, do nothing

            InOrderRec(node.Left, results); // Traverse the left subtree
            results.Add(node.Data); // Add the current node's data to results
            InOrderRec(node.Right, results); // Traverse the right subtree
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Helper method to get the height of a node
        private int GetHeight(AVLNode node) => node?.Height ?? 0;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Balances the given node to ensure AVL property
        private AVLNode Balance(AVLNode node)
        {
            int balanceFactor = GetBalanceFactor(node); // Calculate the balance factor

            // Left-heavy case
            if (balanceFactor > 1)
            {
                if (GetBalanceFactor(node.Left) < 0) // Left-Right case
                    node.Left = RotateLeft(node.Left); // Perform left rotation on left child
                return RotateRight(node); // Perform right rotation
            }
            // Right-heavy case
            if (balanceFactor < -1)
            {
                if (GetBalanceFactor(node.Right) > 0) // Right-Left case
                    node.Right = RotateRight(node.Right); // Perform right rotation on right child
                return RotateLeft(node); // Perform left rotation
            }
            return node; // Node is already balanced
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Calculates the balance factor of a node
        private int GetBalanceFactor(AVLNode node) => GetHeight(node.Left) - GetHeight(node.Right);

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Performs a right rotation on the given node
        private AVLNode RotateRight(AVLNode y)
        {
            var x = y.Left; // Left child becomes the new root
            y.Left = x.Right; // Move right subtree of x to left subtree of y
            x.Right = y; // y becomes the right child of x

            // Update heights
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;

            return x; // Return the new root
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        // Performs a left rotation on the given node
        private AVLNode RotateLeft(AVLNode x)
        {
            var y = x.Right; // Right child becomes the new root
            x.Right = y.Left; // Move left subtree of y to right subtree of x
            y.Left = x; // x becomes the left child of y

            // Update heights
            x.Height = Math.Max(GetHeight(x.Left), GetHeight(x.Right)) + 1;
            y.Height = Math.Max(GetHeight(y.Left), GetHeight(y.Right)) + 1;

            return y; // Return the new root
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
