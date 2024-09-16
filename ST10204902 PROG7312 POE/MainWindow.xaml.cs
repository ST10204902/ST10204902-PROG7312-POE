using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Interop;
using System.Windows.Media;
using System.Windows.Media.Animation;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ST10204902_PROG7312_POE.Models;

namespace ST10204902_PROG7312_POE
{
    //---------------------------------------------------------
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IIssueRepository _issueRepository;
        private readonly IServiceProvider _serviceProvider;

        //---------------------------------------------------------
        //Events
        public event EventHandler IssuesUpdated;



        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="issueRepository"></param>
        public MainWindow(IIssueRepository issueRepository, IServiceProvider serviceProvider)
        {
            InitializeComponent();
            ToolTipSetup();
            IssuesUpdated += OnIssuesUpdated;
            _issueRepository = issueRepository;
            _serviceProvider = serviceProvider;
        }


        //---------------------------------------------------------
        /// <summary>
        /// Add an issue to the list of issues
        /// </summary>
        /// <param name="issue"></param>
        public void AddIssueToList(Issue issue)
        {
            _issueRepository.AddIssue(issue);
            IssuesUpdated?.Invoke(this, EventArgs.Empty);
        }

        //---------------------------------------------------------
        /// <summary>
        /// Setup the tooltips for the buttons
        /// </summary>
        private void ToolTipSetup()
        {
            ToolTip toolTipReportIssues = new ToolTip();
            toolTipReportIssues.Content = "Report issues with the municipal services.";
            btnReportIssues.ToolTip = toolTipReportIssues;

            ToolTip toolTipComingSoon = new ToolTip();
            toolTipComingSoon.Content = "This feature is coming soon.";
            btnLocalEvents.ToolTip = toolTipComingSoon;
            btnServiceStatus.ToolTip = toolTipComingSoon;

            ToolTip toolTipExit = new ToolTip();
            toolTipExit.Content = "Exit the application.";
            btnExit.ToolTip = toolTipExit;
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
        /// Update the list of issues
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIssuesUpdated(object sender, EventArgs e)
        {
            var issues = _issueRepository.GetAllIssues();
            Console.WriteLine("List of Issues:");
            foreach(var issue in issues)
            {
                Console.WriteLine($"Issue at {issue.Location}");
                Console.WriteLine($"Category: {issue.Category}");
                Console.WriteLine($"Description: {issue.Description}");
                Console.WriteLine("Media Attachments:");
                foreach (string mediaAttachment in issue.GetMediaAttachmentDetails())
                {
                    Console.WriteLine(mediaAttachment);
                }
            }
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
    }
}
//----------------------------------EOF---------------------------------------