using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace EventScraper
{
    public class Event
    {
        //--------------------------------------------------------------------------------
        // Properties
        public string Title { get; set; }
        public string Category { get; set; }
        public DateTime Date { get; set; }
        public string Venue { get; set; }
        public string Description { get; set; }
        public string Website { get; set; }
        public string ImageUrl { get; set; }
        public string EventURL { get; set; }

        /// <summary>
        /// ToString override to display event details
        /// </summary>
        /// <returns>All fields in Event</returns>
        public override string ToString()
        {
            string temp = $"Title: {Title}\nCategory: {Category}\nDate: {Date}\nVenue: {Venue}\nDescription: {Description}\nWebsite: {Website}\nImageUrl: {ImageUrl}\nEventUrl: {EventURL}";
            return temp;
        }

    }
}
//-------------------------------EOF------------------------------------//