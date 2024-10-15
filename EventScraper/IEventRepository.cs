using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace EventScraper
{
    /// <summary>
    /// Interface for the Event Repository
    /// </summary>
    public interface IEventRepository
    {
        // Method to add a new event
        Task AddEventAsync(Event evnt);

        // Method to get all events
        Task<List<Event>> GetAllEventsAsync();

        // Method to get events by category
        List<Event> GetEventsByCategory(string category);

        // Method to search for events based on a query string
        Task<List<Event>> SearchEvents(string query);

        // Method to get events by date
        Task<bool> EventExistsAsync(string title, DateTime date);

        // Event to notify when a new event is added
        event EventHandler<Event> EventAdded;
    }
}
//---------------------------------EOF-----------------------------------------------