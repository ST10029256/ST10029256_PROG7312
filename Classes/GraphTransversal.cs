using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ST10029256_PROG7312.Classes
{
    /// <summary>
    /// Provides methods for traversing the graph using Depth-First Search (DFS).
    /// </summary>
    public class GraphTraversal
    {
        // Keeps track of visited nodes during traversal to avoid revisiting
        private readonly HashSet<string> visited;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes a new instance of the <see cref="GraphTraversal"/> class.
        /// </summary>
        public GraphTraversal()
        {
            visited = new HashSet<string>(); // Initialize the visited set
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Performs Depth-First Search (DFS) traversal starting from a specific service request.
        /// </summary>
        public ObservableCollection<ReportIssue> DFS(Graph graph, ReportIssue startReport)
        {
            if (startReport == null || string.IsNullOrWhiteSpace(startReport.RequestID))
                throw new System.ArgumentException("Invalid start report or missing RequestID."); // Validate input

            visited.Clear(); // Clear the visited set before starting the traversal
            var result = new ObservableCollection<ReportIssue>(); // Collection to store traversal results
            DFSVisit(graph, startReport, result); // Start the recursive DFS traversal
            return result; // Return the result collection
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Recursive helper method for performing DFS traversal.
        /// </summary>
        private void DFSVisit(Graph graph, ReportIssue currentReport, ObservableCollection<ReportIssue> result)
        {
            // Check if the current node has already been visited
            if (visited.Contains(currentReport.RequestID))
                return; // Stop recursion if already visited

            visited.Add(currentReport.RequestID); // Mark the current node as visited
            result.Add(currentReport); // Add the current node to the traversal result

            // Retrieve the neighbors of the current node
            var neighbors = graph.GetNeighbors(currentReport);
            foreach (var neighbor in neighbors.Keys) // Iterate through connected nodes
            {
                // Recursively visit unvisited neighbors
                DFSVisit(graph, neighbor, result);
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
