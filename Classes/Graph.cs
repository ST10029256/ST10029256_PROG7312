using System;
using System.Collections.Generic;
using System.Linq;

namespace ST10029256_PROG7312.Classes
{
    /// <summary>
    /// Represents a graph structure for managing service requests and their relationships.
    /// Provides methods to add nodes, add weighted edges, and retrieve neighbors or service request details.
    /// </summary>
    public class Graph
    {
        // Stores the adjacency list: each node maps to its neighbors and the weights of the edges.
        private readonly Dictionary<string, SortedDictionary<string, int>> adjacencyList;

        // Maps RequestIDs to their corresponding ReportIssue objects for quick lookup.
        private readonly Dictionary<string, ReportIssue> nodeLookup;

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Initializes a new instance of the <see cref="Graph"/> class.
        /// </summary>
        public Graph()
        {
            adjacencyList = new Dictionary<string, SortedDictionary<string, int>>();
            nodeLookup = new Dictionary<string, ReportIssue>();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Adds a new service request node to the graph.
        /// Ensures no duplicate nodes are added.
        /// </summary>
        public void AddServiceRequest(ReportIssue report)
        {
            if (report == null || string.IsNullOrWhiteSpace(report.RequestID))
                throw new ArgumentException("Report cannot be null and must have a valid RequestID."); // Validate input

            // If the RequestID is not already in the graph, add it
            if (!adjacencyList.ContainsKey(report.RequestID))
            {
                adjacencyList[report.RequestID] = new SortedDictionary<string, int>(); // Initialize an empty neighbor list
                nodeLookup[report.RequestID] = report; // Add the report to the node lookup
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Adds a weighted edge between two service requests.
        /// Updates the adjacency list for both nodes (undirected graph).
        /// </summary>
        public void AddEdge(ReportIssue report1, ReportIssue report2, int weight)
        {
            if (report1 == null || report2 == null || weight <= 0)
                throw new ArgumentException("Invalid reports or weight."); // Validate input

            // Ensure both nodes exist in the graph
            AddServiceRequest(report1);
            AddServiceRequest(report2);

            // Add the edge in both directions (undirected graph)
            adjacencyList[report1.RequestID][report2.RequestID] = weight;
            adjacencyList[report2.RequestID][report1.RequestID] = weight;
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves the neighbors of a given service request along with the edge weights.
        /// </summary>
        public IDictionary<ReportIssue, int> GetNeighbors(ReportIssue report)
        {
            if (report == null || string.IsNullOrWhiteSpace(report.RequestID))
                throw new ArgumentException("Invalid report or missing RequestID."); // Validate input

            // Attempt to retrieve the neighbors from the adjacency list
            return adjacencyList.TryGetValue(report.RequestID, out var neighbors)
                ? neighbors
                    .Where(n => nodeLookup.ContainsKey(n.Key)) // Ensure all neighbors exist in nodeLookup
                    .ToDictionary(n => nodeLookup[n.Key], n => n.Value) // Convert to dictionary with ReportIssue as keys
                : new Dictionary<ReportIssue, int>(); // Return an empty dictionary if no neighbors found
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Finds a service request by its RequestID.
        /// </summary>
        public ReportIssue GetReportByRequestId(string requestId)
        {
            if (string.IsNullOrWhiteSpace(requestId))
                throw new ArgumentException("RequestID cannot be null or empty."); // Validate input

            return nodeLookup.TryGetValue(requestId, out var report) ? report : null; // Return the found report or null
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Removes a service request node and its associated edges from the graph.
        /// </summary>
        /// <param name="report">The service request to remove.</param>
        public void RemoveServiceRequest(ReportIssue report)
        {
            if (report == null || string.IsNullOrWhiteSpace(report.RequestID))
                throw new ArgumentException("Invalid report or missing RequestID."); // Validate input

            if (!adjacencyList.ContainsKey(report.RequestID))
                return; // If the node does not exist, nothing to remove

            // Remove edges referencing this node from its neighbors
            foreach (var neighbor in adjacencyList[report.RequestID].Keys.ToList())
            {
                adjacencyList[neighbor].Remove(report.RequestID); // Remove this node from the neighbor's adjacency list
            }

            // Remove the node itself
            adjacencyList.Remove(report.RequestID);
            nodeLookup.Remove(report.RequestID);
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Exposes the adjacency list for external use.
        /// </summary>
        /// <returns>A read-only dictionary of the adjacency list.</returns>
        public IReadOnlyDictionary<string, SortedDictionary<string, int>> GetAdjacencyList()
        {
            return adjacencyList; // Return the adjacency list as a read-only dictionary
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
