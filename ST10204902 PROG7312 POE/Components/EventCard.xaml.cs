using EventScraper;
using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
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
        public EventCard()
        {
            InitializeComponent();
            this.Loaded += EventCard_Loaded;
        }

        private void EventCard_Loaded(object sender, RoutedEventArgs e)
        {
            
        }

        public void OnEventCardClick(object sender, RoutedEventArgs e)
        {
            
        }

    }
}
