using EventScraper;
using ST10204902_PROG7312_POE.Components;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for ViewEvents.xaml
    /// </summary>
    public partial class ViewEvents : Window, INotifyPropertyChanged
    {
        //----------------------------------------------------------------
        //Variable Declaration
        private readonly IEventRepository _eventRepository;
        private readonly MainWindow _mainWindow;
        private ObservableCollection<Event> _events;
        private List<string> _searchTerms;
        private HashSet<string> _eventKeys;

        public ObservableCollection<Event> Events
        {
            get => _events;
            set
            {
                _events = value;
                OnPropertyChanged();
            }
        }

        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="eventRepository"></param>
        /// <param name="mainWindow"></param>
        public ViewEvents(IEventRepository eventRepository, MainWindow mainWindow)
        {
            InitializeComponent();
            _eventRepository = eventRepository;
            _mainWindow = mainWindow;
            _searchTerms = new List<string>();
            _eventKeys = new HashSet<string>();

            Events = new ObservableCollection<Event>();
            DataContext = this;

            _eventRepository.EventAdded += OnEventAdded;

            Task.Run(LoadEventsAsync);
        }

        /// <summary>
        /// Load the events from the repository
        /// </summary>
        /// <returns></returns>
        public async Task LoadEventsAsync()
        {
            await LoadCategoriesAsync();
            var events = await _eventRepository.GetAllEventsAsync();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Events.Clear();
                foreach (var ev in events)
                {
                    AddEventIfNotExists(ev);
                }
            });
        }

        /// <summary>
        /// Add an event to the list if it does not already exist
        /// </summary>
        /// <param name="evnt"></param>
        private void AddEventIfNotExists(Event evnt)
        {
            var eventKey = $"{evnt.Title}-{evnt.Date}";
            if (!_eventKeys.Contains(eventKey))
            {
                _eventKeys.Add(eventKey);
                Events.Add(evnt);
            }
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Load the categories for the events
        /// </summary>
        /// <returns></returns>
        private async Task LoadCategoriesAsync()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            HashSet<string> categories = new HashSet<string> { "All categories" };

            categories.UnionWith(events.Select(ev => ev.Category));

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                CategoryComboBox.ItemsSource = categories;
            });
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Event handler for when a new event is added
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="evnt"></param>
        private async void OnEventAdded(object sender, Event evnt)
        {
            // Ensure UI updates occur on the main thread
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                AddEventIfNotExists(evnt);
            });
        }

        //----------------------------------------------------------------
        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;

        //----------------------------------------------------------------
        /// <summary>
        /// OnPropertyChanged method to notify the UI of property changes
        /// </summary>
        /// <param name="name"></param>
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }

        /// <summary>
        /// Search for events based on the search term
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                var searchResults = _eventRepository.SearchEvents(SearchTextBox.Text).Result;
                if(searchResults!= null && searchResults.Count > 0)
                {
                    // Add search term to list of search terms
                    _searchTerms.Add(SearchTextBox.Text);

                    // Update the UI with the search results
                    UpdateEventList(searchResults);

                    // Add "Recommended" section
                    AddRecommendedExpander();

                    // Add or update "Search Results" section
                    UpdateSearchResultsExpander(searchResults);
                }
                else
                {
                    MessageBox.Show(GetWindow(this), "No events found for the search term", "Search Results", MessageBoxButton.OK, MessageBoxImage.Information);

                    await ClearAll();
                }
                
            }
        }

        /// <summary>
        /// Update the "Search Results" expander in the UI
        /// </summary>
        /// <param name="searchResults"></param>
        private void UpdateSearchResultsExpander(List<Event> searchResults)
        {
            // Get the top 5 search results
            var topSearchResults = searchResults.Take(5).ToList();

            // Update the content of the "Search Results" expander in the UI
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Set the ItemsSource for the search results ItemsControl
                SearchItemsControl.ItemsSource = topSearchResults;

                // Expand the "Search Results" expander
                SearchResults.IsExpanded = true;
            });
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Filter the events based on the selected category
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var filteredEvents = new List<Event>();

            // Filter by category if one is selected
            if (CategoryComboBox.SelectedItem is string category && !category.Equals("All categories"))
            {
                filteredEvents = _eventRepository.GetEventsByCategory(category);
            }
            else
            {
                // If no category is selected or "All categories" is selected, get all events
                filteredEvents = _eventRepository.GetAllEventsAsync().Result;
            }

            // Update the UI with the filtered events
            UpdateEventList(filteredEvents);
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Sort the events based on the selected option
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SortButton_Click(object sender, RoutedEventArgs e)
        {
            List<Event> sortedEvents;

            // Determine the sort option selected
            switch (SortByComboBox.SelectedIndex)
            {
                case 0: // Sort by Date (Asc)
                    sortedEvents = Events.OrderBy(ev => ev.Date).ToList();
                    break;

                case 1: // Sort by Date (Desc)
                    sortedEvents = Events.OrderByDescending(ev => ev.Date).ToList();
                    break;

                case 2: // Sort by Title (Asc)
                    sortedEvents = Events.OrderBy(ev => ev.Title).ToList();
                    break;

                case 3: // Sort by Title (Desc)
                    sortedEvents = Events.OrderByDescending(ev => ev.Title).ToList();
                    break;

                case 4: // Sort by Venue (Asc)
                    sortedEvents = Events.OrderBy(ev => ev.Venue).ToList();
                    break;

                case 5: // Sort by Venue (Desc)
                    sortedEvents = Events.OrderByDescending(ev => ev.Venue).ToList();
                    break;

                default:
                    sortedEvents = Events.ToList(); // Default case if no valid selection
                    break;
            }

            // Update the UI with the sorted events
            UpdateEventList(sortedEvents);
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Helper method to update the list of events in the UI
        /// </summary>
        /// <param name="updatedEvents"></param>
        private void UpdateEventList(List<Event> updatedEvents)
        {
            Application.Current.Dispatcher.Invoke(() =>
            {
                Events.Clear();
                foreach (var ev in updatedEvents)
                {
                    Events.Add(ev);
                }
            });
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Helper method to add the recommended events to the "Recommended" expander
        /// </summary>
        private void AddRecommendedExpander()
        {
            // Create a priority queue to determine the recommended events
            var priorityQueue = new SortedDictionary<int, List<Event>>();

            foreach (var searchTerm in _searchTerms)
            {
                var searchResults = _eventRepository.SearchEvents(searchTerm).Result;
                foreach (var ev in searchResults)
                {
                    int priority = CalculatePriority(searchTerm, ev);
                    if (!priorityQueue.ContainsKey(priority))
                    {
                        priorityQueue[priority] = new List<Event>();
                    }
                    priorityQueue[priority].Add(ev);
                }
            }

            // Get the top 5 recommended events
            var recommendedEvents = new List<Event>();
            foreach (var kvp in priorityQueue.Reverse())
            {
                recommendedEvents.AddRange(kvp.Value);
                if (recommendedEvents.Count >= 5)
                {
                    break;
                }
            }

            recommendedEvents = recommendedEvents.Take(5).ToList();

            // Update the content of the "Recommended" expander in the UI
            Application.Current.Dispatcher.Invoke(() =>
            {
                // Set the ItemsSource for the recommended events ItemsControl
                RecommendedItemsControl.ItemsSource = recommendedEvents;
            });
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Calculate the priority of an event based on the search term
        /// Priority is based on the number of matches in the event's title and description
        /// </summary>
        /// <param name="searchTerm"></param>
        /// <param name="ev"></param>
        /// <returns></returns>
        private int CalculatePriority(string searchTerm, Event ev)
        {
            int priority = 0;

            if (ev.Title.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                priority += 10;
            }

            if (ev.Description.IndexOf(searchTerm, StringComparison.OrdinalIgnoreCase) >= 0)
            {
                priority += 5;
            }

            return priority;
        }

        /// <summary>
        /// Window closing event handler, return to mainWindow
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _mainWindow.Show();
        }

        /// <summary>
        /// Clear the search results and close the expanders
        /// Reload events
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void ClearButton_Click(object sender, RoutedEventArgs e)
        {
            await ClearAll();
        }


        private async Task ClearAll()
        {
            // Clear the search text, category selection, and sort selection
            SearchTextBox.Text = string.Empty;
            CategoryComboBox.SelectedIndex = 0;
            SortByComboBox.SelectedIndex = 0;

            // Collapse the Recommended and Search Results expanders
            RecommendedExpander.IsExpanded = false;
            SearchResults.IsExpanded = false;

            // Clear the Search and Recommended ItemsControls
            SearchItemsControl.ItemsSource = null;
            RecommendedItemsControl.ItemsSource = null;

            // Clear search terms and reset the event keys
            _searchTerms.Clear();
            _eventKeys.Clear();

            // Reload the events without clearing the grouped collection view expanders
            await LoadEventsAsync();
        }
    }
}
//-------------------------------EOF---------------------------------