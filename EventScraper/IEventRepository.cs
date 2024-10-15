using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScraper
{
    public interface IEventRepository
    {
        Task AddEventAsync(Event evnt);
        Task<List<Event>> GetEventsByDateAsync(DateTime date);
        Task<List<Event>> GetAllEventsAsync();

        event EventHandler<Event> EventAdded;
    }
}
