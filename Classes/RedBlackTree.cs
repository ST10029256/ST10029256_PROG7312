using System; 
using System.Collections.Generic; 
using System.Linq;

namespace ST10029256_PROG7312.Classes
{

    /// <summary>
    /// Implements a Red-Black Tree for managing and storing ReportIssue objects.
    /// </summary>
    public class RedBlackTree
    {
        private RedBlackTreeNode root; // Root node of the tree

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Inserts a new report into the tree.
        /// </summary>
        public void Insert(ReportIssue report)
        {
            root = InsertRec(root, report); // Recursive insert
            root.IsRed = false; // Root must always be black
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Recursively inserts a report into the tree.
        /// </summary>
        private RedBlackTreeNode InsertRec(RedBlackTreeNode node, ReportIssue report)
        {
            if (node == null) return new RedBlackTreeNode(report); // Create a new node if the tree is empty

            int comparison = string.Compare(report.RequestID, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);
            if (comparison < 0)
                node.Left = InsertRec(node.Left, report); // Insert into the left subtree
            else if (comparison > 0)
                node.Right = InsertRec(node.Right, report); // Insert into the right subtree

            // Rebalance the tree
            if (IsRed(node.Right) && !IsRed(node.Left)) node = RotateLeft(node); // Case 1: Right-leaning red link
            if (IsRed(node.Left) && IsRed(node.Left.Left)) node = RotateRight(node); // Case 2: Consecutive red links on the left
            if (IsRed(node.Left) && IsRed(node.Right)) FlipColors(node); // Case 3: Both children are red

            return node;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Checks if a node is red.
        /// </summary>
        private bool IsRed(RedBlackTreeNode node) => node != null && node.IsRed;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Rotates the subtree to the left.
        /// </summary>
        private RedBlackTreeNode RotateLeft(RedBlackTreeNode node)
        {
            var x = node.Right;
            node.Right = x.Left;
            x.Left = node;
            x.IsRed = node.IsRed;
            node.IsRed = true;
            return x;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Rotates the subtree to the right.
        /// </summary>
        private RedBlackTreeNode RotateRight(RedBlackTreeNode node)
        {
            var x = node.Left;
            node.Left = x.Right;
            x.Right = node;
            x.IsRed = node.IsRed;
            node.IsRed = true;
            return x;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Flips the colors of a node and its children.
        /// </summary>
        private void FlipColors(RedBlackTreeNode node)
        {
            node.IsRed = true;
            if (node.Left != null) node.Left.IsRed = false;
            if (node.Right != null) node.Right.IsRed = false;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Searches for a report by its Request ID.
        /// </summary>
        public ReportIssue Search(string requestId)
        {
            var node = SearchRec(root, requestId); // Recursive search
            return node?.Data;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Recursively searches for a node by its Request ID.
        /// </summary>
        private RedBlackTreeNode SearchRec(RedBlackTreeNode node, string requestId)
        {
            if (node == null) return null; // Return null if the tree is empty

            int comparison = string.Compare(requestId, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);
            if (comparison == 0) return node; // Found the node
            if (comparison < 0) return SearchRec(node.Left, requestId); // Search in the left subtree
            return SearchRec(node.Right, requestId); // Search in the right subtree
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Deletes a report by its Request ID.
        /// </summary>
        public void Delete(string requestId)
        {
            Console.WriteLine($"Attempting to delete: {requestId}");
            if (root == null)
            {
                Console.WriteLine("Tree is empty. Nothing to delete.");
                return;
            }

            if (!Contains(root, requestId)) // Check if the tree contains the report
            {
                Console.WriteLine($"Request ID {requestId} not found in the tree.");
                return;
            }

            root = DeleteRec(root, requestId); // Recursive delete

            if (root != null)
                root.IsRed = false; // Ensure the root is always black

            Console.WriteLine($"Request ID {requestId} deleted successfully.");

            // Log remaining reports
            foreach (var report in InOrderTraversal())
            {
                Console.WriteLine($"Remaining report: {report.RequestID}");
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Recursively deletes a node by its Request ID.
        /// </summary>
        private RedBlackTreeNode DeleteRec(RedBlackTreeNode node, string requestId)
        {
            if (node == null) return null; // Return null if the node doesn't exist

            int comparison = string.Compare(requestId, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);

            if (comparison < 0) // If the report is in the left subtree
            {
                if (!IsRed(node.Left) && !IsRed(node.Left?.Left)) // Move red link if necessary
                    node = MoveRedLeft(node);
                node.Left = DeleteRec(node.Left, requestId);
            }
            else // If the report is in the right subtree or current node
            {
                if (IsRed(node.Left)) node = RotateRight(node); // Rotate right if left child is red
                if (comparison == 0 && node.Right == null) return null; // If it's a leaf node
                if (!IsRed(node.Right) && !IsRed(node.Right?.Left)) // Move red link if necessary
                    node = MoveRedRight(node);
                if (comparison == 0) // If the report is found
                {
                    var minNode = Min(node.Right); // Get the minimum node in the right subtree
                    node.Data = minNode.Data; // Replace the data
                    node.Right = DeleteMin(node.Right); // Delete the minimum node
                }
                else
                {
                    node.Right = DeleteRec(node.Right, requestId);
                }
            }

            return Balance(node); // Balance the tree
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Finds the minimum node in a subtree.
        /// </summary>
        private RedBlackTreeNode Min(RedBlackTreeNode node)
        {
            while (node.Left != null) node = node.Left; // Traverse to the leftmost node
            return node;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Deletes the minimum node in a subtree.
        /// </summary>
        private RedBlackTreeNode DeleteMin(RedBlackTreeNode node)
        {
            if (node.Left == null) return null; // If it's the minimum node

            if (!IsRed(node.Left) && !IsRed(node.Left?.Left)) // Move red link if necessary
                node = MoveRedLeft(node);

            node.Left = DeleteMin(node.Left); // Recursively delete the leftmost node
            return Balance(node); // Balance the tree
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Moves a red link from the right to the left subtree.
        /// </summary>
        private RedBlackTreeNode MoveRedLeft(RedBlackTreeNode node)
        {
            FlipColors(node); // Flip colors
            if (IsRed(node.Right?.Left))
            {
                node.Right = RotateRight(node.Right);
                node = RotateLeft(node);
                FlipColors(node);
            }
            return node;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Moves a red link from the left to the right subtree.
        /// </summary>
        private RedBlackTreeNode MoveRedRight(RedBlackTreeNode node)
        {
            FlipColors(node); // Flip colors
            if (IsRed(node.Left?.Left))
            {
                node = RotateRight(node);
                FlipColors(node);
            }
            return node;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Balances the tree after deletion or insertion.
        /// </summary>
        private RedBlackTreeNode Balance(RedBlackTreeNode node)
        {
            if (IsRed(node.Right)) node = RotateLeft(node); // Rotate left if right child is red
            if (IsRed(node.Left) && IsRed(node.Left?.Left)) node = RotateRight(node); // Rotate right if consecutive reds
            if (IsRed(node.Left) && IsRed(node.Right)) FlipColors(node); // Flip colors if both children are red
            return node;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Checks if a specific Request ID exists in the tree.
        /// </summary>
        private bool Contains(RedBlackTreeNode node, string requestId)
        {
            if (node == null) return false; // Return false if the node is null

            int comparison = string.Compare(requestId, node.Data.RequestID, StringComparison.OrdinalIgnoreCase);
            if (comparison == 0) return true; // If the Request ID matches
            if (comparison < 0) return Contains(node.Left, requestId); // Search in the left subtree
            return Contains(node.Right, requestId); // Search in the right subtree
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves all reports in sorted order using in-order traversal.
        /// </summary>
        public IEnumerable<ReportIssue> InOrderTraversal()
        {
            var result = new List<ReportIssue>();
            InOrderRec(root, result); // Perform in-order traversal
            return result;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Finds related requests based on category or location.
        /// </summary>
        public IEnumerable<ReportIssue> FindRelatedRequests(ReportIssue startReport)
        {
            return InOrderTraversal().Where(report =>
                report.Category == startReport.Category || report.Location == startReport.Location);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Recursively performs in-order traversal of the tree.
        /// </summary>
        private void InOrderRec(RedBlackTreeNode node, List<ReportIssue> result)
        {
            if (node == null) return; // Base case for recursion
            InOrderRec(node.Left, result); // Visit left subtree
            result.Add(node.Data); // Add the current node's data
            InOrderRec(node.Right, result); // Visit right subtree
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
