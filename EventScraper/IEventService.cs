using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScraper
{
    public interface IEventService
    {
        Task<List<Event>> GetAllEventsAsync();
    }
}
