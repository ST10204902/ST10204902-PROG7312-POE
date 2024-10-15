using System.Collections.Generic;

namespace EventScraper
{
    public interface IEventSearcher
    {
        void AddEvent(Event evnt);
        List<Event> GetAllEvents();
        List<Event> GetEventsByDate(System.DateTime date);
    }
}