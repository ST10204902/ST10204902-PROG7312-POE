using System;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Media.Animation;
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

        //---------------------------------------------------------
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="issueRepository"></param>
        public MainWindow(IIssueRepository issueRepository, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            ToolTipSetup();

            _issueRepository = issueRepository;
            _serviceProvider = serviceProvider;
        }

        //---------------------------------------------------------
        /// <summary>
        /// Set up the tooltips for the buttons
        /// </summary>
        private void ToolTipSetup()
        {
            SetButtonToolTip(btnReportIssues, "Report issues with the municipal services.");
            SetButtonToolTip(btnLocalEvents, "This feature is coming soon.");
            SetButtonToolTip(btnServiceStatus, "This feature is coming soon.");
            SetButtonToolTip(btnExit, "Exit the application.");
        }

        //---------------------------------------------------------
        /// <summary>
        /// Set the tooltip for the button
        /// </summary>
        /// <param name="button"></param>
        /// <param name="text"></param>
        private void SetButtonToolTip(Button button, string text)
        {
            ToolTip toolTip = new ToolTip
            {
                Content = text
            };
            button.ToolTip = toolTip;
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
            //Display a confirm dialog to close the app
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Municipal Service Application", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        //---------------------------------------------------------
        /// <summary>
        /// Window loaded event handler. Displays an animation for the logo
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard fadeInStoryboard = (Storyboard)FindResource("FadeInStoryboard");
            fadeInStoryboard.Begin();
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
    }
}

//----------------------------------EOF---------------------------------------