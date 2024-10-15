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
        private readonly IEventRepository _eventRepository;
        private readonly IEventSearcher _eventSearcher;
        private readonly MainWindow _mainWindow;
        private ObservableCollection<Event> _events;

        public ObservableCollection<Event> Events
        {
            get => _events;
            set
            {
                _events = value;
                OnPropertyChanged();
            }
        }

        public ViewEvents(IEventRepository eventRepository ,IEventSearcher eventSearcher, MainWindow mainWindow)
        {
            InitializeComponent();
            _eventRepository = eventRepository;
            _eventSearcher = eventSearcher;
            
            _mainWindow = mainWindow;

            Events = new ObservableCollection<Event>();
            DataContext = this;

            _eventRepository.EventAdded += OnEventAdded;

            Task.Run(LoadEventsAsync);
            Console.WriteLine(Events);
        }

        public async Task LoadEventsAsync()
        {
            await LoadCategoriesAsync();
            var events = await _eventRepository.GetAllEventsAsync();
            Events.Clear();
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                foreach (var ev in events)
                {
                    Events.Add(ev);
                }
            });
        }

        private async Task LoadCategoriesAsync()
        {
            var events = await _eventRepository.GetAllEventsAsync();
            HashSet<string> categories = new HashSet<string>();

            categories.Add("All categories");

            categories.UnionWith(events.Select(ev => ev.Category));

            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                CategoryComboBox.ItemsSource = categories;
            });
        }

        private async void OnEventAdded(object sender, Event evnt)
        {
            // Ensure UI updates occur on the main thread
            await Application.Current.Dispatcher.InvokeAsync(() =>
            {
                Events.Add(evnt);
            });
        }

        // Implement INotifyPropertyChanged
        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string name = "")
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }


        //TODO: FIX THIS
        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            var filteredEvents = new List<Event>();

            // Filter by category if one is selected
            if (CategoryComboBox.SelectedItem is string category && !category.Equals("All categories"))
            {
                filteredEvents = _eventSearcher.GetEventsByCategory(category);
            }
            else
            {
                // If no category is selected or "All categories" is selected, get all events
                filteredEvents = _eventRepository.GetAllEventsAsync().Result;
            }

            // Apply search filter if there is text in the search box
            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                var searchResults = _eventSearcher.SearchEvents(SearchTextBox.Text).Result;

                // If a category filter is applied, intersect with search results
                if (filteredEvents.Any())
                {
                    filteredEvents = filteredEvents.Intersect(searchResults).ToList();
                }
                else
                {
                    filteredEvents = searchResults;
                }
            }

            // Update the observable collection with the filtered results
            Events.Clear();

            Events = new ObservableCollection<Event>();
            foreach (var ev in filteredEvents)
            {
                Events.Add(ev);
            }
            Console.Write(Events);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _mainWindow.Show();
        }
    }
}
