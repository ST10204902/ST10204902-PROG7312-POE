using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace EventScraper
{
    public class EventSearcher : IEventSearcher
    {
        private readonly SortedDictionary<DateTime, List<Event>> _eventsByDate = new SortedDictionary<DateTime, List<Event>>();
        private readonly SortedDictionary<string, List<Event>> _eventsByCategory = new SortedDictionary<string, List<Event>>();

        private readonly IEventService _eventService;

        public EventSearcher(IEventService eventService)
        {
            _eventService = eventService;

            // Initialize event storage
            var result = Task.Run(eventService.GetAllEventsAsync).Result;
            foreach (var ev in result)
            {
                AddEvent(ev);
            }
        }

        public void AddEvent(Event evnt)
        {
            // Add to events by date
            if (!_eventsByDate.ContainsKey(evnt.Date))
            {
                _eventsByDate[evnt.Date] = new List<Event>();
            }
            _eventsByDate[evnt.Date].Add(evnt);

            // Add to events by category if applicable
            if (!string.IsNullOrEmpty(evnt.Category))
            {
                if (!_eventsByCategory.ContainsKey(evnt.Category))
                {
                    _eventsByCategory[evnt.Category] = new List<Event>();
                }
                _eventsByCategory[evnt.Category].Add(evnt);
            }
        }

        public async Task<List<Event>> SearchEvents(string query)
        {
            // Sorted dictionary acting as a priority queue
            // where the int key is the priority value
            // and the value is a list of all events with that priority level
            SortedDictionary<int, List<Event>> priorityQueue = new SortedDictionary<int, List<Event>>();

            var allEvents = await _eventService.GetAllEventsAsync();

            foreach (var ev in allEvents)
            {
                int priority = CalculatePriority(query, ev);

                if (!priorityQueue.ContainsKey(priority))
                {
                    priorityQueue[priority] = new List<Event>();
                }
                priorityQueue[priority].Add(ev);
            }

            // Create a list of events sorted by priority (highest priority first)
            var sortedEvents = new List<Event>();
            foreach (var kvp in priorityQueue.Reverse())
            {
                sortedEvents.AddRange(kvp.Value);
            }

            return sortedEvents;
        }

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

            // Events happening sooner get higher priority
            var daysUntilEvent = (ev.Date - DateTime.Now).Days;
            if (daysUntilEvent >= 0)
            {
                priority += Math.Max(0, 10 - daysUntilEvent);
            }

            return priority;
        }

        public List<Event> GetEventsByCategory(string category)
        {
            if (_eventsByCategory.ContainsKey(category))
            {
                return _eventsByCategory[category];
            }
            return new List<Event>();
        }

        public List<Event> GetEventsByDate(DateTime date)
        {
            if (_eventsByDate.ContainsKey(date))
            {
                return _eventsByDate[date];
            }
            return new List<Event>();
        }

        public List<Event> GetAllEvents()
        {
            return _eventsByDate.Values.SelectMany(x => x).ToList();
        }
    }
}
