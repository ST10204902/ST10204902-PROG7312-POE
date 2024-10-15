using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScraper
{
    public class EventRepository : IEventRepository
    {
        private readonly Lazy<IEventSearcher> _eventSearcher;

        public event EventHandler<Event> EventAdded;

        public EventRepository(Lazy<IEventSearcher> eventSearcher)
        {
            _eventSearcher = eventSearcher;
        }

        public Task AddEventAsync(Event evnt)
        {
            _eventSearcher.Value.AddEvent(evnt);
            EventAdded?.Invoke(this, evnt);
            return Task.CompletedTask;
        }

        public Task<List<Event>> GetEventsByDateAsync(DateTime date)
        {
            var events = _eventSearcher.Value.GetEventsByDate(date);
            return Task.FromResult(events);
        }

        public Task<List<Event>> GetAllEventsAsync()
        {
            var events = _eventSearcher.Value.GetAllEvents();
            return Task.FromResult(events);
        }
    }
}
