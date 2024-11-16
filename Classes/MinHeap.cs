using ST10029256_PROG7312;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

/// <summary>
/// Represents a MinHeap structure for managing ReportIssue objects based on their priority.
/// The heap ensures the smallest-priority item is always at the root.
/// </summary>
public class MinHeap
{
    private ObservableCollection<ReportIssue> heap; // Internal list representing the heap structure

    //---------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Initializes a new instance of the <see cref="MinHeap"/> class.
    /// </summary>
    public MinHeap()
    {
        heap = new ObservableCollection<ReportIssue>(); // Initialize the heap as an empty list
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Inserts a new ReportIssue into the MinHeap.
    /// Ensures the heap property is maintained after insertion.
    /// </summary>
    public void Insert(ReportIssue report)
    {
        heap.Add(report); // Add the new report at the end of the heap
        HeapifyUp(heap.Count - 1); // Restore the heap property by moving the new report up
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Removes a specific ReportIssue from the MinHeap.
    /// Ensures the heap property is maintained after removal.
    /// </summary>

    public void Remove(ReportIssue report)
    {
        // Find the index of the report to be removed using LINQ
        int index = heap.Select((r, i) => new { Item = r, Index = i })
                        .FirstOrDefault(x => x.Item.RequestID == report.RequestID)?.Index ?? -1;

        if (index == -1) return; // If the report is not found, exit the method

        // Replace the report to be removed with the last item in the heap
        heap[index] = heap[heap.Count - 1];
        heap.RemoveAt(heap.Count - 1); // Remove the last item

        // Restore the heap property if the removed item was not the last element
        if (index < heap.Count)
        {
            HeapifyUp(index); // Attempt to move the element up
            HeapifyDown(index); // Attempt to move the element down
        }
    }


    //---------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Retrieves all ReportIssue objects in the heap.
    /// </summary>
    public ObservableCollection<ReportIssue> GetAllRequests()
    {
        return new ObservableCollection<ReportIssue>(heap); // Return a copy to avoid modifying the original heap
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Restores the heap property by moving an element up the heap.
    /// </summary>
    private void HeapifyUp(int index)
    {
        // Continue moving the element up until the heap property is restored
        while (index > 0)
        {
            int parentIndex = (index - 1) / 2; // Calculate the parent index

            // Stop if the current element's priority is greater than or equal to its parent's priority
            if (heap[index].Priority >= heap[parentIndex].Priority) break;

            Swap(index, parentIndex); // Swap the current element with its parent
            index = parentIndex; // Move to the parent's index
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Restores the heap property by moving an element down the heap.
    /// </summary>
    private void HeapifyDown(int index)
    {
        int leftChild;
        // Continue moving the element down while it has a left child
        while ((leftChild = 2 * index + 1) < heap.Count)
        {
            int rightChild = leftChild + 1; // Calculate the right child's index

            // Determine the smaller child
            int smallestChild = (rightChild < heap.Count && heap[rightChild].Priority < heap[leftChild].Priority)
                ? rightChild
                : leftChild;

            // Stop if the current element's priority is less than or equal to the smaller child's priority
            if (heap[index].Priority <= heap[smallestChild].Priority) break;

            Swap(index, smallestChild); // Swap the current element with the smaller child
            index = smallestChild; // Move to the smallest child's index
        }
    }

    //---------------------------------------------------------------------------------------------------------------------------------------------//

    /// <summary>
    /// Swaps two elements in the heap.
    /// </summary>
    private void Swap(int i, int j)
    {
        var temp = heap[i]; // Temporarily store the first element
        heap[i] = heap[j]; // Assign the second element to the first position
        heap[j] = temp; // Assign the temporary element to the second position
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
