using System;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
using EventScraper;
using Microsoft.Extensions.DependencyInjection;
using ST10204902_PROG7312_POE.Models;

namespace ST10204902_PROG7312_POE
{
    //---------------------------------------------------------
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        //---------------------------------------------------------
        // Variable Declaration
        private readonly IIssueRepository _issueRepository;

        private readonly IServiceProvider _serviceProvider;
        private readonly IEventRepository _eventRepository;
        private bool _isExitConfirmed = false;

        //---------------------------------------------------------
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="issueRepository"></param>
        public MainWindow(IIssueRepository issueRepository, IEventRepository eventRepository, IServiceProvider serviceProvider)
        {
            InitializeComponent();

            _issueRepository = issueRepository;
            _eventRepository = eventRepository;
            _serviceProvider = serviceProvider;

            LoadEventsAsync();

            Console.WriteLine("MainWindow constructor called");
            Console.WriteLine($"Issue Repository: {_issueRepository != null}");
            Console.WriteLine($"Service Provider: {_serviceProvider != null}");
            Console.WriteLine($"Event Repository: {_eventRepository != null}");
            
        }

        private async Task LoadEventsAsync()
        {
            bool retry = true;

            while (retry)
            {
                try
                {
                    await Task.Delay(10000);
                    
                    var allEvents = await _eventRepository.GetAllEventsAsync();

                    Console.WriteLine("All Events: ");
                    foreach (var ev in allEvents)
                    {
                        Console.WriteLine(ev.ToString());
                    }

                    retry = false; // Exit the loop if successful
                }
                catch (InvalidOperationException ex)
                {
                    var result = MessageBox.Show(
                        "No internet connection and no local events.csv file found. \r\nPlease connect to the internet and press OK to try again.",
                        "Error",
                        MessageBoxButton.OKCancel,
                        MessageBoxImage.Error
                    );

                    if (result == MessageBoxResult.Cancel)
                    {
                        retry = false; // Exit the loop if the user cancels
                    }
                }
            }
        }

        //---------------------------------------------------------
        /// <summary>
        /// Report Issues button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnReportIssues_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Report Issues Form passing the main menu
            ReportIssues reportIssues = new ReportIssues(this, _issueRepository);
            reportIssues.Show();
            this.Hide();
        }

        //---------------------------------------------------------
        /// <summary>
        /// Exit button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            ConfirmExit();
        }

        //---------------------------------------------------------
        /// <summary>
        /// Window loaded event handler. Displays an animation for the logo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Play();
        }

        //---------------------------------------------------------
        /// <summary>
        /// Feedback button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnFeedback_Click(object sender, RoutedEventArgs e)
        {
            string url = Properties.Settings.Default.GFormURL;

            try
            {
                System.Diagnostics.Process.Start(new System.Diagnostics.ProcessStartInfo
                {
                    FileName = url,
                    UseShellExecute = true
                });
            }
            catch (Exception ex)
            {
                MessageBox.Show("An error occurred while trying to open the feedback form. Please try again later.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //---------------------------------------------------------
        /// <summary>
        /// Window closing event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            if (!_isExitConfirmed)
            {
                e.Cancel = true;
                ConfirmExit();
            }
        }

        //---------------------------------------------------------
        /// <summary>
        /// Private method to confirm exit
        /// </summary>
        private void ConfirmExit()
        {
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Municipal Service Application", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                _isExitConfirmed = true;
                Application.Current.Shutdown();
            }
        }

        private void btnViewIssues_Click(object sender, RoutedEventArgs e)
        {
            ViewReportedIssues viewReportedIssues = new ViewReportedIssues(this, _issueRepository);
            viewReportedIssues.Show();
            this.Hide();
        }

        private void videoPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show($"Media failed to load: {e.ErrorException.Message}");
        }

        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Position = TimeSpan.Zero;
            videoPlayer.Play();
        }

        private async void btnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            ViewEvents viewEvents = new ViewEvents(_eventRepository, this);
            await viewEvents.LoadEventsAsync();
            viewEvents.Show();
            this.Hide();
        }
    }
}

//----------------------------------EOF---------------------------------------