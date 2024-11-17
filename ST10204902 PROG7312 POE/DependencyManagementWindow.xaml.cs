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
using System.Diagnostics;

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
        public DependencyManagementWindow(ServiceRequest currentRequest, ServiceRequestGraph serviceRequestGraph, Window parentWindow)
        {
            InitializeComponent();
            _currentRequest = currentRequest;
            _serviceRequestGraph = serviceRequestGraph;
            Owner = parentWindow;
            // Exclude current request and filter out null entries
            _availableRequests = serviceRequestGraph.GetAllServiceRequests()
            .Where(sr => sr != null
                && sr != currentRequest
                && sr.Id != 0
                && !string.IsNullOrEmpty(sr.Description))
            .ToList();

            AvailableRequestsListBox.ItemsSource = _availableRequests;
            AvailableRequestsListBox.DisplayMemberPath = "Description";

            // Preselect current dependencies if they exist
            if (currentRequest.Dependencies != null)
            {
                foreach (var dependency in currentRequest.Dependencies.Where(d => d != null))
                {
                    AvailableRequestsListBox.SelectedItems.Add(dependency);
                }
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
            
            // Validate dependency levels first
            if (!ValidateDependencyLevels(_currentRequest, SelectedDependencies))
            {
                return;
            }

            // Create a temporary copy of the graph to test the new dependencies
            var tempGraph = new ServiceRequestGraph();

            // First add all vertices
            foreach (var request in _serviceRequestGraph.GetAllServiceRequests())
            {
                tempGraph.AddServiceRequest(request);
            }

            // Then add all existing dependencies
            foreach (var request in _serviceRequestGraph.GetAllServiceRequests())
            {
                if (request.Dependencies != null)
                {
                    foreach (var dep in request.Dependencies.Where(d => d != null))
                    {
                        tempGraph.AddDependency(request, dep);
                    }
                }
            }

            // Clear current request's existing dependencies
            tempGraph.ClearDependencies(_currentRequest);

            // Add only the new selected dependencies for current request
            foreach (var dependency in SelectedDependencies.Where(d => d != null))
            {
                tempGraph.AddDependency(_currentRequest, dependency);
            }

            // Check for circular dependencies
            if (tempGraph.HasCircularDependency())
            {
                ShowCircularDependencyWarning(_currentRequest, SelectedDependencies.First());
                return;
            }

            // If we get here, update the actual dependencies
            _currentRequest.Dependencies = new List<ServiceRequest>(SelectedDependencies);


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
        private string GetDependencyPath(ServiceRequest start, ServiceRequest end, ServiceRequestGraph graph)
        {
            var path = graph.FindDependencyPath(start, end);
            if (path == null || path.Count == 0) return string.Empty;

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
            var tempGraph = new ServiceRequestGraph();

            // Recreate the graph state
            foreach (var request in _serviceRequestGraph.GetAllServiceRequests())
            {
                tempGraph.AddServiceRequest(request);
                foreach (var dep in request.Dependencies ?? new List<ServiceRequest>())
                {
                    tempGraph.AddDependency(request, dep);
                }
            }

            // Add the new dependency that would create the cycle
            tempGraph.AddDependency(request1, request2);

            var path = GetDependencyPath(request2, request1, tempGraph);
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
            var tempGraph = new ServiceRequestGraph();
            
            // Copy existing graph state
            foreach (var existingRequest in _serviceRequestGraph.GetAllServiceRequests())
            {
                tempGraph.AddServiceRequest(existingRequest);
                if (existingRequest.Dependencies != null)
                {
                    foreach (var dep in existingRequest.Dependencies)
                    {
                        tempGraph.AddDependency(existingRequest, dep);
                    }
                }
            }

            // Add new dependencies temporarily
            foreach (var dependency in newDependencies)
            {
                tempGraph.AddDependency(request, dependency);
            }

            // Check the length of dependency chains
            foreach (var serviceRequest in tempGraph.GetAllServiceRequests())
            {
                var chain = tempGraph.DepthFirstSearch(serviceRequest).ToList();
                if (chain.Count > 3)
                {
                    MessageBox.Show(
                        $"Warning: Adding these dependencies would create a chain longer than 3 levels starting from request {serviceRequest.Id} with description: {serviceRequest.Description}.\n" +
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