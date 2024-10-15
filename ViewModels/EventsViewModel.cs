using ST10029256_PROG7312.Classes;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;

namespace ST10029256_PROG7312.ViewModels
{
    /// <summary>
    /// ViewModel that manages the events, categories, and filtering logic for the application.
    /// </summary>
    public class EventsViewModel
    {
        // Collection of all available events
        public ObservableCollection<LocalEvent> AvailableEvents { get; set; }

        // Sorted dictionary to store events by date
        public SortedDictionary<DateTime, List<LocalEvent>> EventsByDate { get; set; }

        // Collection of event categories
        public ObservableCollection<string> Categories { get; set; }

        // Collection of events that have been filtered based on search or other criteria
        public ObservableCollection<LocalEvent> FilteredEvents { get; set; }

        // Collection of recommended events based on user search patterns
        public ObservableCollection<LocalEvent> RecommendedEvents { get; set; }

        // Dictionary to track search frequency patterns
        private Dictionary<string, int> SearchPatternFrequency { get; set; }

        // Threshold to determine when to generate recommendations based on search frequency
        private const int RecommendationThreshold = 3;

        // Set of unique categories to prevent duplicates
        private HashSet<string> UniqueCategories { get; set; }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Constructor that initializes the collections and sets up default categories.
        /// </summary>
        public EventsViewModel()
        {
            // Initialize collections and default categories
            AvailableEvents = new ObservableCollection<LocalEvent>();
            EventsByDate = new SortedDictionary<DateTime, List<LocalEvent>>();
            Categories = new ObservableCollection<string>
            {
                "Select a Category",
                "Concert",
                "Festival",
                "Workshop",
                "Conference",
                "Community Meeting",
                "Sports Event",
                "Other"
            };
            FilteredEvents = new ObservableCollection<LocalEvent>();
            RecommendedEvents = new ObservableCollection<LocalEvent>();
            SearchPatternFrequency = new Dictionary<string, int>();
            UniqueCategories = new HashSet<string>(Categories); // Ensures no duplicate categories
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Adds a new event to the AvailableEvents and EventsByDate collections, ensuring no duplicates.
        /// </summary>
        public void AddEvent(LocalEvent newEvent)
        {
            // Check for duplicate events based on name, date, and location using LINQ
            var duplicateEvent = AvailableEvents.FirstOrDefault(ev =>
                ev.Name == newEvent.Name &&
                ev.Date == newEvent.Date &&
                ev.Location == newEvent.Location);

            if (duplicateEvent == null) // Only add if no duplicate exists
            {
                AvailableEvents.Add(newEvent);

                // Add event to the SortedDictionary by date
                if (!EventsByDate.ContainsKey(newEvent.Date))
                {
                    EventsByDate[newEvent.Date] = new List<LocalEvent>();
                }
                EventsByDate[newEvent.Date].Add(newEvent); // Add to the list for the specified date
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Adds a new category to the Categories collection if it is unique.
        /// </summary>
        public void AddCategory(string category)
        {
            // Add category if it's not already in the HashSet
            if (UniqueCategories.Add(category))
            {
                Categories.Add(category);
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Retrieves events for a specific date from the EventsByDate dictionary.
        /// </summary>
        public List<LocalEvent> GetEventsByDate(DateTime date)
        {
            // Return events for the specified date, or an empty list if no events exist for that date
            return EventsByDate.ContainsKey(date) ? EventsByDate[date] : new List<LocalEvent>();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Filters the available events based on search text, category, and selected date.
        /// </summary>
        public void FilterEvents(string searchText, string selectedCategory, DateTime? selectedDate)
        {
            // Clear previous filtered events
            FilteredEvents.Clear();

            // Use LINQ to filter events based on search text, category, and date
            var filtered = AvailableEvents.Where(ev =>
                (string.IsNullOrEmpty(searchText) || ev.Name.ToLower().Contains(searchText.ToLower())) &&
                (string.IsNullOrEmpty(selectedCategory) || ev.Category.Equals(selectedCategory, StringComparison.OrdinalIgnoreCase)) &&
                (!selectedDate.HasValue || ev.Date.Date == selectedDate.Value.Date));

            // Add distinct filtered events to the FilteredEvents collection
            foreach (var ev in filtered.Distinct())
            {
                FilteredEvents.Add(ev);
            }

            // If search text is provided, record the search pattern
            if (!string.IsNullOrEmpty(searchText))
            {
                RecordSearchPattern(searchText.ToLower());
            }

            // Generate event recommendations based on search patterns
            GenerateRecommendations();
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Records the frequency of user search patterns.
        /// </summary>
        private void RecordSearchPattern(string searchText)
        {
            // Increment the search frequency for the search text, or add it if it's new
            if (SearchPatternFrequency.ContainsKey(searchText))
            {
                SearchPatternFrequency[searchText]++;
            }
            else
            {
                SearchPatternFrequency[searchText] = 1;
            }
        }

        //---------------------------------------------------------------------------------------------------------------------------------------------//

        /// <summary>
        /// Generates event recommendations based on user search patterns and frequency.
        /// </summary>
        public void GenerateRecommendations()
        {
            // Clear previous recommendations
            RecommendedEvents.Clear();

            // Get searches that have been repeated enough times to meet the recommendation threshold
            var frequentSearches = SearchPatternFrequency
                .Where(kvp => kvp.Value >= RecommendationThreshold) // Filter based on threshold
                .Select(kvp => kvp.Key) // Select the search terms
                .ToList();

            if (frequentSearches.Count == 0)
                return; // No recommendations if no frequent searches

            // Recommend events that match the frequent search terms for name, location, or category
            var recommended = AvailableEvents
                .Where(ev => frequentSearches.Contains(ev.Name.ToLower()) ||
                             frequentSearches.Contains(ev.Location.ToLower()) ||
                             frequentSearches.Contains(ev.Category.ToLower()))
                .OrderByDescending(ev => SearchPatternFrequency.ContainsKey(ev.Name.ToLower()) ? SearchPatternFrequency[ev.Name.ToLower()] : 0)
                .ToList();

            // Add recommended events to the RecommendedEvents collection
            foreach (var ev in recommended)
            {
                RecommendedEvents.Add(ev);
            }
        }
    }
}//------------------------------------------------------------------ENF OF FILE----------------------------------------------------------------------//
