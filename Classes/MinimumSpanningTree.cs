using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ST10029256_PROG7312.Classes
{
    /// <summary>
    /// Provides methods to calculate the Minimum Spanning Tree (MST) using Prim's algorithm.
    /// </summary>
    public class MinimumSpanningTree
    {
        /// <summary>
        /// Calculates the Minimum Spanning Tree (MST) of the graph using Prim's algorithm.
        /// </summary>
        public ObservableCollection<Tuple<ReportIssue, ReportIssue, int>> PrimMST(Graph graph)
        {
            if (graph == null)
                throw new ArgumentNullException(nameof(graph), "Graph cannot be null."); // Validate input

            var mstEdges = new ObservableCollection<Tuple<ReportIssue, ReportIssue, int>>(); // Stores the MST edges
            var visited = new HashSet<string>(); // Tracks visited nodes
            var priorityQueue = new SortedSet<Tuple<int, string, string>>(new EdgeComparer()); // Priority queue for edges

            // Select an arbitrary starting node
            var startNode = graph.GetAdjacencyList().Keys.FirstOrDefault();
            if (startNode == null) return mstEdges; // Return an empty MST if the graph is empty

            visited.Add(startNode); // Mark the starting node as visited

            // Add all edges from the start node to the priority queue
            foreach (var neighbor in graph.GetAdjacencyList()[startNode])
            {
                priorityQueue.Add(Tuple.Create(neighbor.Value, startNode, neighbor.Key));
            }

            // Continue until the priority queue is empty
            while (priorityQueue.Count > 0)
            {
                // Get the edge with the smallest weight
                var edge = priorityQueue.Min;
                priorityQueue.Remove(edge);

                var (weight, fromId, toId) = edge;

                // Skip if the destination node is already visited
                if (visited.Contains(toId)) continue;

                visited.Add(toId); // Mark the destination node as visited

                // Add the edge to the MST
                mstEdges.Add(Tuple.Create(
                    graph.GetReportByRequestId(fromId), // Source node
                    graph.GetReportByRequestId(toId),   // Destination node
                    weight                              // Edge weight
                ));

                // Add all edges from the newly visited node to the priority queue
                foreach (var neighbor in graph.GetAdjacencyList()[toId])
                {
                    if (!visited.Contains(neighbor.Key)) // Only add edges to unvisited nodes
                    {
                        priorityQueue.Add(Tuple.Create(neighbor.Value, toId, neighbor.Key));
                    }
                }
            }

            return mstEdges; // Return the collection of MST edges
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Custom comparer for sorting edges based on their weights and node IDs.
        /// Ensures the priority queue in Prim's algorithm is ordered correctly.
        /// </summary>
        private class EdgeComparer : IComparer<Tuple<int, string, string>>
        {
            public int Compare(Tuple<int, string, string> x, Tuple<int, string, string> y)
            {
                // Compare edge weights first
                if (x.Item1 != y.Item1)
                    return x.Item1.CompareTo(y.Item1);

                // If weights are equal, compare the source node IDs
                int fromComparison = string.Compare(x.Item2, y.Item2, StringComparison.Ordinal);
                if (fromComparison != 0) return fromComparison;

                // If source node IDs are equal, compare the destination node IDs
                return string.Compare(x.Item3, y.Item3, StringComparison.Ordinal);
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
