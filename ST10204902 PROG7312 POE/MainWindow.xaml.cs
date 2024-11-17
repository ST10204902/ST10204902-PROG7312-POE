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

        //----------------------------------------------------------------
        /// <summary>
        /// View Issues button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnViewIssues_Click(object sender, RoutedEventArgs e)
        {
            ViewReportedIssues viewReportedIssues = new ViewReportedIssues(this, _issueRepository);
            viewReportedIssues.Show();
            this.Hide();
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Event handler for when the media fails to load
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videoPlayer_MediaFailed(object sender, ExceptionRoutedEventArgs e)
        {
            MessageBox.Show($"Media failed to load: {e.ErrorException.Message}");
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Restart the video when it ends
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void videoPlayer_MediaEnded(object sender, RoutedEventArgs e)
        {
            videoPlayer.Position = TimeSpan.Zero;
            videoPlayer.Play();
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Local Events button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnLocalEvents_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var viewEventsWindow = _serviceProvider.GetRequiredService<ViewEvents>();
                await viewEventsWindow.LoadEventsAsync();
                viewEventsWindow.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Exception in btnLocalEvents_Click: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void btnServiceStatus_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                ServiceRequestWindow srw = new ServiceRequestWindow(this);
                srw.Show();
                this.Hide();
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
                MessageBox.Show($"Exception in btnServiceStatus_Click: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}

//----------------------------------EOF---------------------------------------