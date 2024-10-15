using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;

namespace EventScraper
{
    public class EventRepository : IEventRepository
    {
        private readonly List<Event> _events;

        public EventRepository()
        {
            _events = LoadEvents(); // Load initial events here
        }

        // Load events from a source like CSV, web scraping, or use test data
        private List<Event> LoadEvents()
        {
            // Example data for testing purposes
            return new List<Event>
            {
            };
        }

        // Fetch all events
        public Task<List<Event>> GetAllEventsAsync()
        {
            return Task.FromResult(_events);
        }

        // Add an event to the repository
        public async Task AddEventAsync(Event evnt)
        {
            if (!_events.Any(e => e.Title == evnt.Title && e.Date == evnt.Date))
            {
                _events.Add(evnt);
                EventAdded?.Invoke(this, evnt);
                await Task.CompletedTask;
            }
        }

        // Event handler for when a new event is added
        public event EventHandler<Event> EventAdded;

        // Filter events by category
        public List<Event> GetEventsByCategory(string category)
        {
            return _events.Where(ev => ev.Category.Equals(category, StringComparison.OrdinalIgnoreCase)).ToList();
        }

        // Filter events by date
        public List<Event> GetEventsByDate(DateTime date)
        {
            return _events.Where(ev => ev.Date.Date == date.Date).ToList();
        }

        // Search for events using a query string, using a priority-based search
        public Task<List<Event>> SearchEvents(string query)
        {
            var priorityQueue = new SortedDictionary<int, List<Event>>();

            foreach (var ev in _events)
            {
                int priority = CalculatePriority(query, ev);

                if (!priorityQueue.ContainsKey(priority))
                {
                    priorityQueue[priority] = new List<Event>();
                }
                priorityQueue[priority].Add(ev);
            }

            // Sort by priority descending and flatten the list
            var sortedEvents = priorityQueue.Reverse().SelectMany(kvp => kvp.Value).ToList();

            return Task.FromResult(sortedEvents);
        }

        // Calculate the priority of an event based on the search query
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

        public async Task<bool> EventExistsAsync(string title, DateTime date)
        {
            return await Task.FromResult(_events.Any(e => e.Title == title && e.Date == date));
        }
    }
}
