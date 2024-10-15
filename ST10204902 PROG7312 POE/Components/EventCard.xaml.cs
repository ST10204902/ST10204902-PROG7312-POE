using EventScraper;
using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10204902_PROG7312_POE.Components
{
    /// <summary>
    /// Interaction logic for EventCard.xaml
    /// </summary>
    public partial class EventCard : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public EventCard()
        {
            InitializeComponent();
        }

        /// <summary>
        /// When the event card is clicked, open the event URL in the default browser
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        public void OnEventCardClick(object sender, RoutedEventArgs e)
        {
            if (DataContext is Event evnt && !string.IsNullOrEmpty(evnt.EventURL))
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = evnt.EventURL,
                        UseShellExecute = true
                    });
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to open URL: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }

    }
}
//-------------------------------------EOF-------------------------------------------