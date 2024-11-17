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
using ST10204902_PROG7312_POE.Models;
using ST10204902_PROG7312_POE.DataStructures;

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for DependencyManagementWindow.xaml
    /// </summary>
    public partial class DependencyManagementWindow : Window
    {
        //------------------------------------------------------------------
        // Fields
        //------------------------------------------------------------------
        private readonly ServiceRequest _currentRequest;
        private readonly List<ServiceRequest> _availableRequests;
        private readonly ServiceRequestGraph _serviceRequestGraph;
        public List<ServiceRequest> SelectedDependencies { get; private set; }

        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the DependencyManagementWindow.
        /// </summary>
        /// <param name="currentRequest">The current service request.</param>
        /// <param name="serviceRequestGraph">The service request graph.</param>    
        public DependencyManagementWindow(ServiceRequest currentRequest, ServiceRequestGraph serviceRequestGraph)
        {
            InitializeComponent();
            _currentRequest = currentRequest;
            _serviceRequestGraph = serviceRequestGraph;

            //Exclude current request from list
            _availableRequests = serviceRequestGraph.GetAllServiceRequests()
                .Where(sr => sr != currentRequest).ToList() ;

            AvailableRequestsListBox.ItemsSource = _availableRequests;
            AvailableRequestsListBox.DisplayMemberPath = "Description";

            // Preselect current dependencies
            foreach(var dependency in currentRequest.Dependencies)
            {
                AvailableRequestsListBox.SelectedItems.Add(dependency);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "OK" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            // Get the selected dependencies
            SelectedDependencies = AvailableRequestsListBox.SelectedItems.Cast<ServiceRequest>().ToList();

            // Create a temporary copy of the graph to test the new dependencies
            var tempGraph = new ServiceRequestGraph();
            
            // Add all existing service requests to the temp graph
            foreach (var request in _serviceRequestGraph.GetAllServiceRequests())
            {
                tempGraph.AddServiceRequest(request);
                
                // Add existing dependencies except for the current request's dependencies
                if (request != _currentRequest)
                {
                    foreach (var dep in request.Dependencies)
                    {
                        tempGraph.AddDependency(request, dep);
                    }
                }
            }

            // Add the new dependencies to the temp graph
            foreach (var dependency in SelectedDependencies)
            {
                tempGraph.AddDependency(_currentRequest, dependency);
            }

            // Check for circular dependencies
            if (tempGraph.HasCircularDependency())
            {
                // Find the first circular dependency for demonstration
                foreach (var dependency in SelectedDependencies)
                {
                    if (tempGraph.FindDependencyPath(dependency, _currentRequest).Any())
                    {
                        ShowCircularDependencyWarning(_currentRequest, dependency);
                        return;
                    }
                }
            }

            DialogResult = true;
            Close();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Cancel" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets the dependency path between two service requests.
        /// </summary>
        /// <param name="start">The starting service request.</param>
        private string GetDependencyPath(ServiceRequest start, ServiceRequest end)
        {
            var path = _serviceRequestGraph.FindDependencyPath(start, end);
            if (path.Count == 0) return string.Empty;

            return string.Join(" → ", path.Select(r => r.Id));
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Shows a circular dependency warning.
        /// </summary>
        /// <param name="request1">The first service request.</param>
        /// <param name="request2">The second service request.</param>
        private void ShowCircularDependencyWarning(ServiceRequest request1, ServiceRequest request2)
        {
            var path = GetDependencyPath(request1, request2);
            var message = "Circular dependency detected!\n\n";
            
            if (!string.IsNullOrEmpty(path))
            {
                message += $"Dependency path: {path} → {request1.Id}\n\n";
            }
            
            message += "Please select different dependencies.";

            MessageBox.Show(
                message,
                "Circular Dependency Warning",
                MessageBoxButton.OK,
                MessageBoxImage.Warning
            );
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Validates the dependency levels of new dependencies.
        /// </summary>
        /// <param name="request">The current service request.</param>
        /// <param name="newDependencies">The new dependencies to validate.</param>
        private bool ValidateDependencyLevels(ServiceRequest request, List<ServiceRequest> newDependencies)
        {
            foreach (var dependency in newDependencies)
            {
                // Use BFS to check dependency levels
                var dependencyChain = _serviceRequestGraph.BreadthFirstSearch(dependency).ToList();
                
                // If dependency chain is too long (e.g., more than 3 levels)
                if (dependencyChain.Count > 3)
                {
                    MessageBox.Show(
                        "Warning: This would create a dependency chain longer than 3 levels.\n" +
                        "Consider restructuring the dependencies.",
                        "Deep Dependency Chain",
                        MessageBoxButton.OK,
                        MessageBoxImage.Warning
                    );
                    return false;
                }
            }
            return true;
        }
    }
}
// ------------------------------EOF------------------------------------
