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
using System.Windows.Shapes;
using ST10204902_PROG7312_POE.DataStructures;
using ST10204902_PROG7312_POE.Models;
using System.Diagnostics;

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for ServiceRequestDetailsWindow.xaml
    /// </summary>
    public partial class ServiceRequestDetailsWindow : Window
    {
        //------------------------------------------------------------------
        // Fields
        //------------------------------------------------------------------
        private readonly ServiceRequestGraph _serviceRequestGraph;
        private ServiceRequest _currentRequest;

        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ServiceRequestDetailsWindow.
        /// </summary>
        public ServiceRequestDetailsWindow()
        {
            InitializeComponent();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ServiceRequestDetailsWindow.
        /// </summary>
        /// <param name="selectedRequest">The selected service request.</param>
        /// <param name="serviceRequestGraph">The service request graph.</param>
        public ServiceRequestDetailsWindow(ServiceRequest selectedRequest, ServiceRequestGraph serviceRequestGraph)
        {
            InitializeComponent();
            DataContext = selectedRequest;
            _serviceRequestGraph = serviceRequestGraph;
            _currentRequest = selectedRequest;

            // Initialize lists
            StatusHistoryList.ItemsSource = _currentRequest.StatusHistory;
            AttachmentsList.ItemsSource = _currentRequest.Attachments;
            
            DisplayAssociatedServiceRequests(_currentRequest);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Displays the associated service requests for the selected request.
        /// </summary>
        /// <param name="selectedRequest">The selected service request.</param>
        private void DisplayAssociatedServiceRequests(ServiceRequest selectedRequest)
        {
            if (_serviceRequestGraph?.ContainsServiceRequest(selectedRequest) != true)
                return;

            var traversalType = (TraversalTypeComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
            var associatedRequests = traversalType == "Depth-First Search"
                ? _serviceRequestGraph.DepthFirstSearch(selectedRequest)
                : _serviceRequestGraph.BreadthFirstSearch(selectedRequest);

            AssociatedRequestsListBox.ItemsSource = associatedRequests;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the selection change event for the traversal type combo box.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The selection changed event arguments.</param>  
        private void TraversalTypeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            DisplayAssociatedServiceRequests(_currentRequest);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the open attachment button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The click event arguments.</param>
        private void OpenAttachment_Click(object sender, RoutedEventArgs e)
        {
            if ((sender as Button)?.DataContext is MediaAttachment attachment)
            {
                try
                {
                    Process.Start(new ProcessStartInfo
                    {
                        FileName = attachment.FilePath,
                        UseShellExecute = true
                    });
                }
                catch
                {
                    MessageBox.Show("Unable to open the file. The file may have been moved or deleted.",
                        "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}
// ------------------------------EOF------------------------------------
