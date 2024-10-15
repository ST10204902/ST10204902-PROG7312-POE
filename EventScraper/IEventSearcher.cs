using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventScraper
{
    public interface IEventSearcher
    {
        void AddEvent(Event evnt);
        List<Event> GetAllEvents();
        List<Event> GetEventsByDate(System.DateTime date);
        List<Event> GetEventsByCategory(string category);
        Task<List<Event>> SearchEvents(string query);
    }
}