using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EventScraper
{
    /// <summary>
    /// Event Repository class to manage events
    /// </summary>
    public class EventRepository : IEventRepository
    {
        //----------------------------------------------------------------
        // Variable Declaration
        private readonly List<Event> _events;

        //----------------------------------------------------------------
        /// <summary>
        /// Default constructor
        /// </summary>
        public EventRepository()
        {
            _events = LoadEvents(); // Load initial events here
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Load events from a source like CSV, web scraping, or use test data
        /// </summary>
        /// <returns></returns>
        private List<Event> LoadEvents()
        {
            // Example data for testing purposes
            return new List<Event>
            {
            };
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Fetch all events
        /// </summary>
        /// <returns></returns>
        public Task<List<Event>> GetAllEventsAsync()
        {
            return Task.FromResult(_events);
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Add an event to the repository
        /// </summary>
        /// <param name="evnt"></param>
        /// <returns></returns>
        public async Task AddEventAsync(Event evnt)
        {
            if (!_events.Any(e => e.Title == evnt.Title && e.Date == evnt.Date))
            {
                _events.Add(evnt);
                EventAdded?.Invoke(this, evnt);
                await Task.CompletedTask;
            }
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Event handler for when a new event is added
        /// </summary>
        public event EventHandler<Event> EventAdded;

        //----------------------------------------------------------------
        /// <summary>
        /// Filter events by category
        /// </summary>
        /// <param name="category"></param>
        /// <returns></returns>
        public List<Event> GetEventsByCategory(string category)
        {
            return _events.Where(ev => ev.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Filter events by date
        /// </summary>
        /// <param name="date"></param>
        /// <returns></returns>
        public List<Event> GetEventsByDate(DateTime date)
        {
            return _events.Where(ev => ev.Date.Date == date.Date).ToList();
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Search for events using a query string, using a priority-based search
        /// </summary>
        /// <param name="query"></param>
        /// <returns></returns>
        public Task<List<Event>> SearchEvents(string query)
        {
            var lowerQuery = query.ToLower();
            var priorityQueue = new SortedDictionary<int, List<Event>>();

            foreach (var ev in _events)
            {
                int priority = 0;

                // Check for title match or partial match (highest priority)
                if (ev.Title.ToLower().Contains(lowerQuery))
                {
                    priority += 10;
                }

                // Check for venue match or partial match (medium priority)
                if (ev.Venue.ToLower().Contains(lowerQuery))
                {
                    priority += 5;
                }

                // Check for description match or partial match (lowest priority)
                if (ev.Description.ToLower().Contains(lowerQuery))
                {
                    priority += 2;
                }

                // Only include the event if it has a non-zero priority (i.e., at least one match)
                if (priority > 0)
                {
                    if (!priorityQueue.ContainsKey(priority))
                    {
                        priorityQueue[priority] = new List<Event>();
                    }
                    priorityQueue[priority].Add(ev);
                }
            }

            // Sort by priority descending and flatten the list
            var sortedEvents = priorityQueue.Reverse().SelectMany(kvp => kvp.Value).ToList();

            return Task.FromResult(sortedEvents);
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Calculate the priority of an event based on the search query
        /// </summary>
        /// <param name="query"></param>
        /// <param name="ev"></param>
        /// <returns></returns>
        private static int CalculatePriority(string query, Event ev)
        {
            int priority = 0;

            if (ev.Title.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                priority += 10;
            }

            if (ev.Description.IndexOf(query, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                priority += 5;
            }

            var daysTilEvent = (ev.Date - DateTime.Now).Days;
            if (daysTilEvent >= 0)
            {
                priority += Math.Max(0, 10 - daysTilEvent); // Events happening sooner get higher priority
            }

            return priority;
        }
        //----------------------------------------------------------------
        /// <summary>
        /// Check if an event with the same title and date already exists
        /// </summary>
        /// <param name="title"></param>
        /// <param name="date"></param>
        /// <returns></returns>
        public async Task<bool> EventExistsAsync(string title, DateTime date)
        {
            return await Task.FromResult(_events.Any(e => e.Title == title && e.Date == date));
        }
    }
}

//----------------------------------EOF---------------------------------------
