using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScraper
{
    public class EventRepository : IEventRepository
    {
        private readonly IEventSearcher _eventSearcher;

        public event EventHandler<Event> EventAdded;

        public EventRepository(IEventSearcher eventSearcher)
        {
            _eventSearcher = eventSearcher;
        }

        public Task AddEventAsync(Event evnt)
        {
            _eventSearcher.AddEvent(evnt);
            EventAdded?.Invoke(this, evnt);
            return Task.CompletedTask;
        }

        public Task<List<Event>> GetEventsByDateAsync(DateTime date)
        {
            var events = _eventSearcher.GetEventsByDate(date);
            return Task.FromResult(events);
        }

        public Task<List<Event>> GetAllEventsAsync()
        {
            var events = _eventSearcher.GetAllEvents();
            return Task.FromResult(events);
        }
    }
}
