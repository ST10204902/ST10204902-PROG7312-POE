using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScraper
{
    public class EventService:IEventService
    {
        private readonly List<Event> _events;

        public EventService()
        {
            _events = new List<Event>();

        }

        public Task<List<Event>> GetAllEventsAsync()
        {
            return Task.FromResult(_events);
        }
    }
}
