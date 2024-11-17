using ST10204902_PROG7312_POE.DataStructures;
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
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using ST10204902_PROG7312_POE.Models;

namespace ST10204902_PROG7312_POE.Components
{
    /// <summary>
    /// Interaction logic for ServiceRequestCard.xaml
    /// </summary>
    public partial class ServiceRequestCard : UserControl
    {
        //------------------------------------------------------------------
        // Fields
        //------------------------------------------------------------------
        private ServiceRequestWindow _parentWindow;
        private ServiceRequestGraph _serviceRequestGraph;
        
        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ServiceRequestCard class.
        /// </summary>
        public ServiceRequestCard()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ServiceRequestCard class with a parent window and service request graph.
        /// </summary>
        public ServiceRequestCard(ServiceRequestWindow parentWindow, ServiceRequestGraph serviceRequestGraph)
        {
            InitializeComponent();
            _parentWindow = parentWindow;
            _serviceRequestGraph = serviceRequestGraph;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the parent window.
        /// </summary>
        public ServiceRequestWindow ParentWindow
        {
            get => _parentWindow;
            set => _parentWindow = value;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the service request card.
        /// </summary>
        private void OnServiceRequestCardClick(object sender, MouseButtonEventArgs e)
        {
            // Open the service request window
            //Show the service request details window for this service request

        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the more details button.
        /// </summary>
        private void MoreDetailsButton_Click(object sender, RoutedEventArgs e)
        {
            var selectedRequest = (ServiceRequest)DataContext;
            ServiceRequestDetailsWindow detailsWindow = new ServiceRequestDetailsWindow(selectedRequest, _serviceRequestGraph);
            detailsWindow.ShowDialog();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the edit button.
        /// </summary>
        private void EditButton_Click(object sender, RoutedEventArgs e)
        {
            if(_parentWindow != null){
                var selectedRequest = (ServiceRequest)DataContext;
                _parentWindow.ShowEditPanel(selectedRequest);
            }
        }

    }
}
// ------------------------------EOF------------------------------------