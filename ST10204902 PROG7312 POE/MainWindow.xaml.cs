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

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        public MainWindow()
        {
            InitializeComponent();
            ToolTipSetup();
        }



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

        private void btnReportIssues_Click(object sender, RoutedEventArgs e)
        {
            // Navigate to Report Issues Form passing the main menu
            ReportIssues reportIssues = new ReportIssues(this);
            reportIssues.Show();
            this.Hide();
        }

        private void btnExit_Click(object sender, RoutedEventArgs e)
        {
            //Display a confirm dialog to close the app
            MessageBoxResult result = MessageBox.Show("Are you sure you want to exit?", "Exit Municipal Service Application", MessageBoxButton.YesNo, MessageBoxImage.Question);
            if (result == MessageBoxResult.Yes)
            {
                Application.Current.Shutdown();
            }
        }

        private void Window_Loaded(object sender, RoutedEventArgs e)
        {
            Storyboard fadeInStoryboard = (Storyboard)FindResource("FadeInStoryboard");
            fadeInStoryboard.Begin();
        }
    }
}
