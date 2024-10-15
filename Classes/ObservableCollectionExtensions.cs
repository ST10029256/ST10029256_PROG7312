using System.Collections.Generic;
using System.Collections.ObjectModel;

namespace ST10029256_PROG7312.Helpers
{
    // Extension methods for ObservableCollection
    public static class ObservableCollectionExtensions
    {
        // Adds a range of items to an ObservableCollection
        public static void AddRange<T>(this ObservableCollection<T> collection, IEnumerable<T> items)
        {
            // Iterate through each item in the provided collection and add it to the ObservableCollection
            foreach (var item in items)
            {
                collection.Add(item);
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
