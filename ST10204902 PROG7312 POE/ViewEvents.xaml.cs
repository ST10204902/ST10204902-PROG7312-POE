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
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Shapes;

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for ViewEvents.xaml
    /// </summary>
    public partial class ViewEvents : Window, INotifyPropertyChanged
    {
        private readonly IEventRepository _eventRepository;
        private readonly EventSearcher _eventSearcher;
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

        public ViewEvents(IEventRepository eventRepository, MainWindow mainWindow)
        {
            InitializeComponent();
            _eventRepository = eventRepository;
            _mainWindow = mainWindow;
            _eventSearcher = new EventSearcher(eventRepository);
            Events = new ObservableCollection<Event>();
            DataContext = this;

            _eventRepository.EventAdded += OnEventAdded;

            Task.Run(LoadEventsAsync);
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

        private void FilterButton_Click(object sender, RoutedEventArgs e)
        {
            if(CategoryComboBox.SelectedItem is string category)
            {
                if (category.Equals("All categories"))
                {
                    Task.Run(LoadEventsAsync);
                }
                else
                {
                    var events = _eventSearcher.GetEventsByCategory(category);
                    Events = new ObservableCollection<Event>(events);
                }
            }

            if (!string.IsNullOrEmpty(SearchTextBox.Text))
            {
                SearchEvents(SearchTextBox.Text);
            }
            
        }

        private void SearchEvents(string query)
        {
            var searchResults = _eventSearcher.SearchEvents(query);
            Events = new ObservableCollection<Event>(searchResults.Result);
        }

        private void Window_Closing(object sender, CancelEventArgs e)
        {
            _mainWindow.Show();
        }
    }
}
