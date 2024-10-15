using CsvHelper;
using HtmlAgilityPack;
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace EventScraper
{
    public class EventScraper
    {
        private readonly string _rootUrl;
        private readonly IEventRepository _eventRepository;

        public EventScraper(string rootUrl, IEventRepository eventRepository)
        {
            _rootUrl = rootUrl;
            _eventRepository = eventRepository;
        }

        /// <summary>
        /// Check if there is a working internet connection
        /// </summary>
        /// <returns>True if there is an internet connection, otherwise false</returns>
        private bool IsInternetAvailable()
        {
            try
            {
                using (var client = new HttpClient())
                {
                    var response = client.GetAsync("http://www.google.com").Result;
                    return response.IsSuccessStatusCode;
                }
            }
            catch
            {
                return false;
            }
        }

        /// <summary>
        /// Scrapes the root page to get individual event URLs
        /// </summary>
        /// <returns>A list of URLs for individual event pages</returns>
        public async Task<List<string>> GetEventLinksAsync()
        {
            var eventLinks = new List<string>();

            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetStringAsync(_rootUrl);

                var htmlDoc = new HtmlDocument();
                htmlDoc.LoadHtml(response);

                //Find event links
                var linkNodes = htmlDoc.DocumentNode.SelectNodes("//h4[contains(@class, 'mec-event-title')]/a");

                if (linkNodes != null)
                {
                    foreach (var node in linkNodes)
                    {
                        var eventUrl = node.GetAttributeValue("href", string.Empty);
                        if (!string.IsNullOrEmpty(eventUrl))
                        {
                            if (!eventUrl.StartsWith("http"))
                            {
                                eventUrl = new Uri(new Uri(_rootUrl), eventUrl).ToString();
                            }
                            eventLinks.Add(eventUrl);
                        }
                    }
                }
                return eventLinks;
            }
        }

        /// <summary>
        /// Scrapes the event details from the individual event pages
        /// </summary>
        /// <param name="eventUrls"></param>
        /// <returns>List of event objects containing the details</returns>
        // EventScraper.cs
        public async IAsyncEnumerable<Event> ScrapeEventDetailsAsync(List<string> eventUrls)
        {
            string[] formats = { "d MMMM yyyy", "MMMM d, yyyy", "yyyy-MM-dd" }; // Add more formats as needed

            using (HttpClient client = new HttpClient())
            {
                foreach (var url in eventUrls)
                {
                    var response = await client.GetStringAsync(url);
                    var htmlDoc = new HtmlDocument();
                    htmlDoc.LoadHtml(response);

                    // Get all event details from the event page
                    var titleNode = htmlDoc.DocumentNode.SelectSingleNode("//h1[@class='event-article-title']");
                    var dateNodes = htmlDoc.DocumentNode.SelectNodes("//li/strong[contains(text(), 'Event Date')]/following-sibling::text()");
                    var descriptionNode = htmlDoc.DocumentNode.SelectSingleNode("//div[@class='mec-single-event-description mec-events-content']");
                    var websiteNode = htmlDoc.DocumentNode.SelectSingleNode("//li/strong[contains(text(), 'Website')]/following-sibling::a");
                    var venueNode = htmlDoc.DocumentNode.SelectSingleNode("//li/strong[contains(text(), 'Venue')]/following-sibling::a")
                          ?? htmlDoc.DocumentNode.SelectSingleNode("//li/strong[contains(text(), 'Venue')]/following-sibling::text()");
                    var imageNode = htmlDoc.DocumentNode.SelectSingleNode("//meta[@property='og:image']");
                    var categoryNode = htmlDoc.DocumentNode.SelectSingleNode("//span[contains(@class, 'mec_taxonomy_tag')]");


                    string imageUrl = imageNode?.GetAttributeValue("content", "N/A");
                    if (imageUrl != null && imageUrl.Contains("optimole.com"))
                    {
                        // Extract only image URL
                        var lastHttpsIndex = imageUrl.LastIndexOf("https://");
                        if (lastHttpsIndex != -1)
                        {
                            imageUrl = imageUrl.Substring(lastHttpsIndex);
                        }
                    }

                    // Handle multiple dates
                    var eventDates = new List<DateTime>();
                    if (dateNodes != null)
                    {
                        foreach (var dateNode in dateNodes)
                        {
                            string dateString = dateNode.InnerText.Trim();
                            if (DateTime.TryParseExact(dateString, formats, CultureInfo.InvariantCulture, DateTimeStyles.None, out DateTime eventDate))
                            {
                                if (eventDate >= DateTime.Today)
                                {
                                    eventDates.Add(eventDate);
                                }
                            }
                        }
                    }

                    // If no valid future dates, skip this event
                    if (!eventDates.Any())
                    {
                        continue;
                    }

                    string venue = HtmlEntity.DeEntitize(venueNode?.InnerText.Trim() ?? venueNode?.GetAttributeValue("href", "N/A") ?? "N/A");

                    // Create the event object
                    var evnt = new Event
                    {
                        Title = HtmlEntity.DeEntitize(titleNode?.InnerText.Trim() ?? "N/A"),
                        Date = eventDates.FirstOrDefault() != DateTime.MinValue ? eventDates.FirstOrDefault() : DateTime.Now, // Use first date or fallback
                        Description = HtmlEntity.DeEntitize(descriptionNode?.InnerText.Trim() ?? "N/A"),
                        Website = HtmlEntity.DeEntitize(websiteNode?.InnerText.Trim() ?? "N/A"),
                        Venue = venue,
                        ImageUrl = imageUrl,
                        Category = HtmlEntity.DeEntitize(categoryNode?.InnerText.Trim() ?? "N/A")
                    };

                    // Yield the event immediately
                    yield return evnt;
                }
            }
        }


        /// <summary>
        /// Scrape the event details and store them in memory
        /// </summary>
        /// <returns> </returns>
        /// <exception cref="InvalidOperationException"></exception>
        public async Task ScrapeAndStoreEventsAsync()
        {
            if (IsInternetAvailable())
            {
                var eventUrls = await GetEventLinksAsync();
                HashSet<Event> events = new HashSet<Event>();
                await foreach (var evnt in ScrapeEventDetailsAsync(eventUrls))
                {
                    if (!await _eventRepository.EventExistsAsync(evnt.Title, evnt.Date))
                    {
                        await _eventRepository.AddEventAsync(evnt);
                        Console.WriteLine("Event added: " + evnt.Title);
                    }
                    else
                    {
                        Console.WriteLine("Event already exists: " + evnt.Title);
                    }
                }
            }
            else
            {
                var projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
                var filePath = Path.Combine(projectDirectory, "events.csv");

                if (File.Exists(filePath))
                {
                    var events = ImportFromCsv(filePath);
                    foreach (var evnt in events)
                    {
                        await _eventRepository.AddEventAsync(evnt);
                    }
                }
                else
                {
                    throw new InvalidOperationException("No internet connection and no local events.csv file found.");
                }
            }

            // Export events to CSV asynchronously
            var allEvents = await _eventRepository.GetAllEventsAsync();
            await Task.Run(() => ExportToCsv(allEvents));
        }


        //----------------------------------------------------------------
        // The following methods are used to keep an offline copy of the
        // events in a CSV file. This is useful for testing and debugging
        // or when there is no internet connection available.

        //----------------------------------------------------------------
        /// <summary>
        /// Export the list of events to a CSV file in the project directory
        /// </summary>
        /// <param name="events">List of events to export</param>
        public static void ExportToCsv(List<Event> events)
        {
            var projectDirectory = AppDomain.CurrentDomain.BaseDirectory;
            var filePath = Path.Combine(projectDirectory, "events.csv");

            //If a csv already exists, delete it or overwrite it
            if (File.Exists(filePath))
            {
                File.Delete(filePath);
            }

            using (var writer = new StreamWriter(filePath))
            using (var csv = new CsvWriter(writer, CultureInfo.InvariantCulture))
            {
                csv.WriteRecords(events);
            }

            Console.WriteLine($"Events exported to {filePath}");
        }


        //----------------------------------------------------------------
        /// <summary>
        /// Import events from a CSV file
        /// </summary>
        /// <param name="filePath"></param>
        /// <returns></returns>
        public static List<Event> ImportFromCsv(string filePath)
        {
            using (var reader = new StreamReader(filePath))
            using (var csv = new CsvReader(reader, CultureInfo.InvariantCulture))
            {
                var events = csv.GetRecords<Event>().ToList();
                return events;
            }
        }
    }
}