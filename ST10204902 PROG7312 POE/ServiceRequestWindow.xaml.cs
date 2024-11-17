using ST10204902_PROG7312_POE.Components;
using ST10204902_PROG7312_POE.DataStructures;
using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Input;
using System.Windows.Media;
using System.Diagnostics;
using Microsoft.Win32;
using System.IO;
using ST10204902_PROG7312_POE.Helpers;
using ST10204902_PROG7312_POE.Constants;

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for ServiceRequestWindow.xaml
    /// Manages the service request window UI and logic.
    /// </summary>
    public partial class ServiceRequestWindow : Window, INotifyPropertyChanged
    {
        //------------------------------------------------------------------
        // Fields - Main Window
        //------------------------------------------------------------------
        private readonly MainWindow _mainWindow;

        //------------------------------------------------------------------
        // Fields - Data Structures
        //------------------------------------------------------------------
        private readonly ServiceRequestGraph _serviceRequestGraph;

        private bool _isEditMode;
        private ServiceRequest _currentRequest;
        private readonly ServiceRequestPriorityQueue _priorityQueue;
        private readonly ServiceRequestBST _serviceRequestBST;

        //------------------------------------------------------------------
        // Fields - UI
        //------------------------------------------------------------------
        public event PropertyChangedEventHandler PropertyChanged;

        private ListView _highPriorityListView;
        private bool _IsFormVisible;
        private string _FormTitle;
        private ListBox _attachmentsListBox;

        //------------------------------------------------------------------
        /// <summary>
        /// Initializes a new instance of the ServiceRequestWindow class.
        /// </summary>
        /// <param name="mainWindow">The main window instance.</param>
        public ServiceRequestWindow(MainWindow mainWindow)
        {
            InitializeComponent();
            DataContext = this;
            _serviceRequestGraph = new ServiceRequestGraph();
            _mainWindow = mainWindow;
            _priorityQueue = new ServiceRequestPriorityQueue();
            _serviceRequestBST = new ServiceRequestBST();
            IsFormVisible = false;
            FormTitle = "Add New Request";
            _highPriorityListView = this.FindName("HighPriorityListView") as ListView;
            _attachmentsListBox = this.FindName("AttachmentsListBox") as ListBox;
            InitializeValidationEvents();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a value indicating whether the form is visible.
        /// </summary>
        public bool IsFormVisible
        {
            get => _IsFormVisible;
            set
            {
                if (_IsFormVisible != value)
                {
                    _IsFormVisible = value;
                    OnPropertyChanged(nameof(IsFormVisible));
                    UpdateFormColumnVisibility();
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets or sets the form title.
        /// </summary>
        public string FormTitle
        {
            get => _FormTitle;
            set
            {
                if (_FormTitle != value)
                {
                    _FormTitle = value;
                    OnPropertyChanged(nameof(FormTitle));
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets or sets a value indicating whether the form is in edit mode.
        /// </summary>
        public bool IsEditMode
        {
            get => _isEditMode;
            set
            {
                if (_isEditMode != value)
                {
                    _isEditMode = value;
                    OnPropertyChanged(nameof(IsEditMode));
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Shows the edit panel for a service request.
        /// </summary>
        /// <param name="serviceRequest">The service request to edit.</param>
        public void ShowEditPanel(ServiceRequest serviceRequest)
        {
            IsFormVisible = true;
            IsEditMode = true;
            FormTitle = "Edit Request";
            _currentRequest = serviceRequest;
            DataContext = this;
            PopulateFormFields(serviceRequest);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Populates the form fields with the service request data.
        /// </summary>
        /// <param name="request">The service request to populate the form with.</param>
        private void PopulateFormFields(ServiceRequest request)
        {
            RequesterNameTextBox.Text = request.RequesterName;
            ContactInfoTextBox.Text = request.ContactInfo;
            LocationTextBox.Text = request.Location;
            DescriptionTextBox.Text = request.Description;
            SetComboBoxItem(CategoryComboBox, request.Category);
            SetComboBoxItem(PriorityComboBox, GetPriorityString(request.Priority));
            AssignedToTextBox.Text = request.AssignedTo;
            DateResolvedPicker.SelectedDate = request.DateResolved;
            ResolutionCommentTextBox.Text = request.ResolutionComment;

            // Set Category
            foreach (ComboBoxItem item in CategoryComboBox.Items)
            {
                if (item.Content.ToString() == request.Category)
                {
                    CategoryComboBox.SelectedItem = item;
                    break;
                }
            }

            // Set Priority
            string priorityString = request.Priority switch
            {
                1 => "High",
                2 => "Medium",
                3 => "Low",
                _ => "Low"
            };

            foreach (ComboBoxItem item in PriorityComboBox.Items)
            {
                if (item.Content.ToString() == priorityString)
                {
                    PriorityComboBox.SelectedItem = item;
                    break;
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Clears the form fields.
        /// </summary>
        private void ClearFormFields()
        {
            RequesterNameTextBox.Text = string.Empty;
            ContactInfoTextBox.Text = string.Empty;
            LocationTextBox.Text = string.Empty;
            DescriptionTextBox.Text = string.Empty;
            CategoryComboBox.SelectedIndex = -1;
            PriorityComboBox.SelectedIndex = -1;
            AssignedToTextBox.Text = string.Empty;
            DateResolvedPicker.SelectedDate = null;
            ResolutionCommentTextBox.Text = string.Empty;
            AttachmentsListBox.ItemsSource = null;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Sets the selected item in a ComboBox based on the provided value.
        /// </summary>
        /// <param name="comboBox">The ComboBox control.</param>
        /// <param name="value">The value to match and set as the selected item.</param>
        private void SetComboBoxItem(ComboBox comboBox, string value)
        {
            foreach (ComboBoxItem item in comboBox.Items)
            {
                if (item.Content.ToString() == value)
                {
                    comboBox.SelectedItem = item;
                    break;
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets the priority value based on the provided priority string.
        /// </summary>
        /// <param name="priority">The priority string.</param>
        /// <returns>The priority value.</returns>
        private int GetPriorityValue(string priority)
        {
            switch (priority)
            {
                case "High":
                    return 1;

                case "Medium":
                    return 2;

                case "Low":
                    return 3;

                default:
                    return 3; // Default to Low priority
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets the priority string based on the provided priority value.
        /// </summary>
        /// <param name="priority">The priority value.</param>
        /// <returns>The priority string.</returns>
        private string GetPriorityString(int priority)
        {
            switch (priority)
            {
                case 1:
                    return "High";

                case 2:
                    return "Medium";

                case 3:
                    return "Low";

                default:
                    return "Low"; // Default value
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Generates a new unique ID for a service request.
        /// </summary>
        /// <returns>The new unique ID.</returns>
        private int GenerateNewId()
        {
            var existingIds = _serviceRequestGraph.GetAllServiceRequests().Select(r => r.Id);
            return existingIds.Any() ? existingIds.Max() + 1 : 1;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Validates the form fields.
        /// </summary>
        /// <returns>True if the form is valid, otherwise false.</returns>
        private bool ValidateForm()
        {
            var errors = new List<string>();

            // Validate Attachments
            if (_currentRequest.Attachments.Count == 0)
            {
                MessageBox.Show("Please attach at least one media file to the request.",
                    "Validation Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }

            // Validate Requester Name
            if (string.IsNullOrWhiteSpace(RequesterNameTextBox.Text))
            {
                errors.Add("Requester Name is required");
                RequesterNameTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                RequesterNameTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");
            }

            // Validate Contact Information
            if (string.IsNullOrWhiteSpace(ContactInfoTextBox.Text))
            {
                errors.Add("Contact Information is required");
                ContactInfoTextBox.BorderBrush = Brushes.Red;
            }
            else if (!IsValidEmail(ContactInfoTextBox.Text))
            {
                errors.Add("Please enter a valid email address");
                ContactInfoTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                ContactInfoTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");
            }

            // Validate Location
            if (string.IsNullOrWhiteSpace(LocationTextBox.Text))
            {
                errors.Add("Location is required");
                LocationTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                LocationTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");
            }

            // Validate Description
            if (string.IsNullOrWhiteSpace(DescriptionTextBox.Text))
            {
                errors.Add("Description is required");
                DescriptionTextBox.BorderBrush = Brushes.Red;
            }
            else if (DescriptionTextBox.Text.Length < 10)
            {
                errors.Add("Description must be at least 10 characters long");
                DescriptionTextBox.BorderBrush = Brushes.Red;
            }
            else
            {
                DescriptionTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");
            }

            // Validate Category
            if (CategoryComboBox.SelectedItem == null)
            {
                errors.Add("Please select a Category");
                CategoryComboBox.BorderBrush = Brushes.Red;
            }
            else
            {
                CategoryComboBox.BorderBrush = (Brush)FindResource("ComboBoxBorderBrush");
            }

            // Validate Priority
            if (PriorityComboBox.SelectedItem == null)
            {
                errors.Add("Please select a Priority");
                PriorityComboBox.BorderBrush = Brushes.Red;
            }
            else
            {
                PriorityComboBox.BorderBrush = (Brush)FindResource("ComboBoxBorderBrush");
            }

            // Additional validation for Edit Mode
            if (IsEditMode)
            {
                // Validate Assigned To
                if (string.IsNullOrWhiteSpace(AssignedToTextBox.Text))
                {
                    errors.Add("Assigned To is required in edit mode");
                    AssignedToTextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    AssignedToTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");
                }

                // Validate Resolution Comment if Status is Completed
                if (_currentRequest.Status == "Completed" && string.IsNullOrWhiteSpace(ResolutionCommentTextBox.Text))
                {
                    errors.Add("Resolution Comment is required for completed requests");
                    ResolutionCommentTextBox.BorderBrush = Brushes.Red;
                }
                else
                {
                    ResolutionCommentTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");
                }
            }

            // If there are any validation errors, show them to the user
            if (errors.Any())
            {
                MessageBox.Show(
                    string.Join("\n", errors),
                    "Validation Errors",
                    MessageBoxButton.OK,
                    MessageBoxImage.Warning);
                return false;
            }

            return true;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Updates the form column visibility.
        /// </summary>
        private void UpdateFormColumnVisibility()
        {
            FormColumn.Width = IsFormVisible ? new GridLength(1, GridUnitType.Auto) : new GridLength(0);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Analyzes the impact of completing a service request.
        /// </summary>
        /// <param name="request">The service request to analyze.</param>
        private void AnalyzeRequestImpact(ServiceRequest request)
        {
            // Use BFS to find all immediately affected requests
            var immediateImpact = _serviceRequestGraph.BreadthFirstSearch(request).Skip(1).Take(1);

            // Use DFS to find the complete chain of dependent requests
            var fullDependencyChain = _serviceRequestGraph.DepthFirstSearch(request).Skip(1);

            var message = $"Completing this request will:\n" +
                         $"- Immediately affect {immediateImpact.Count()} requests\n" +
                         $"- Impact a total of {fullDependencyChain.Count()} dependent requests";

            MessageBox.Show(message, "Impact Analysis", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Invokes the PropertyChanged event.
        /// </summary>
        /// <param name="propertyName">The name of the property that changed.</param>
        protected void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Updates the high priority list view.
        /// </summary>
        private void UpdateHighPriorityList()
        {
            // Get top 5 high priority requests
            var highPriorityRequests = GetHighPriorityRequests(5);
            _highPriorityListView.ItemsSource = highPriorityRequests;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Gets the top N high priority requests.
        /// </summary>
        /// <param name="count">The number of requests to get.</param>
        /// <returns>The top N high priority requests.</returns>
        private List<ServiceRequest> GetHighPriorityRequests(int count)
        {
            var allRequests = _priorityQueue.GetTopN(count);
            return allRequests.Where(r => r.Priority == 1).ToList();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Validates an email address.
        /// </summary>
        /// <param name="email">The email address to validate.</param>
        /// <returns>True if the email address is valid, otherwise false.</returns>
        private bool IsValidEmail(string email)
        {
            try
            {
                var addr = new System.Net.Mail.MailAddress(email);
                return addr.Address == email;
            }
            catch
            {
                return false;
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Initializes the validation events for the form fields.
        /// </summary>
        private void InitializeValidationEvents()
        {
            RequesterNameTextBox.TextChanged += (s, e) =>
                RequesterNameTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");

            ContactInfoTextBox.TextChanged += (s, e) =>
                ContactInfoTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");

            LocationTextBox.TextChanged += (s, e) =>
                LocationTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");

            DescriptionTextBox.TextChanged += (s, e) =>
                DescriptionTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");

            CategoryComboBox.SelectionChanged += (s, e) =>
                CategoryComboBox.BorderBrush = (Brush)FindResource("ComboBoxBorderBrush");

            PriorityComboBox.SelectionChanged += (s, e) =>
                PriorityComboBox.BorderBrush = (Brush)FindResource("ComboBoxBorderBrush");

            AssignedToTextBox.TextChanged += (s, e) =>
                AssignedToTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");

            ResolutionCommentTextBox.TextChanged += (s, e) =>
                ResolutionCommentTextBox.BorderBrush = (Brush)FindResource("TextControlBorderBrush");
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Add New Request" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void AddNewRequestButton_Click(object sender, RoutedEventArgs e)
        {
            IsFormVisible = true;
            IsEditMode = false;
            FormTitle = "Add New Request";
            _currentRequest = new ServiceRequest
            {
                DateSubmitted = DateTime.Now,
                Status = "Pending",
                StatusHistory = new List<Tuple<DateTime, string>>(),
                Attachments = new List<MediaAttachment>(),
                Dependencies = new List<ServiceRequest>()
            };
            ClearFormFields();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Save Changes" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void SaveChangesButton_Click(object sender, RoutedEventArgs e)
        {
            if (!ValidateForm())
                return;

            try
            {
                if (!IsEditMode)
                {
                    // Store the current dependencies before creating new request
                    var currentDependencies = _currentRequest?.Dependencies ?? new List<ServiceRequest>();

                    _currentRequest = new ServiceRequest
                    {
                        Id = GenerateNewId(),
                        Status = "Pending",
                        DateSubmitted = DateTime.Now,
                        StatusHistory = new List<Tuple<DateTime, string>>(),
                        Attachments = new List<MediaAttachment>(),
                        Dependencies = currentDependencies
                    };
                    _serviceRequestGraph.AddServiceRequest(_currentRequest);

                    // Re-add the dependencies to the graph
                    foreach (var dependency in currentDependencies)
                    {
                        _serviceRequestGraph.AddDependency(_currentRequest, dependency);
                    }
                }

                // Update request properties
                _currentRequest.RequesterName = RequesterNameTextBox.Text;
                _currentRequest.ContactInfo = ContactInfoTextBox.Text;
                _currentRequest.Location = LocationTextBox.Text;
                _currentRequest.Description = DescriptionTextBox.Text;
                _currentRequest.Category = (CategoryComboBox.SelectedItem as ComboBoxItem)?.Content.ToString();
                _currentRequest.AssignedTo = AssignedToTextBox.Text;
                _currentRequest.DateResolved = DateResolvedPicker.SelectedDate;
                _currentRequest.ResolutionComment = ResolutionCommentTextBox.Text;
                _currentRequest.DateSubmitted = DateTime.Now;

                // Update status if resolved date is set
                if (DateResolvedPicker.SelectedDate.HasValue && !string.IsNullOrEmpty(ResolutionCommentTextBox.Text))
                {
                    string newStatus = "Resolved";
                    if (_currentRequest.Status != newStatus)
                    {
                        _currentRequest.StatusHistory.Add(new Tuple<DateTime, string>(DateTime.Now, newStatus));
                        _currentRequest.Status = newStatus;
                    }
                }

                // Get the new priority value
                int newPriority = 3; // Default to Low
                if (PriorityComboBox.SelectedItem is ComboBoxItem selectedPriority)
                {
                    newPriority = GetPriorityValue(selectedPriority.Content.ToString());
                }

                if (IsEditMode)
                {
                    // Update priority using the new method
                    _priorityQueue.UpdatePriority(_currentRequest, newPriority);
                }
                else
                {
                    _currentRequest.Priority = newPriority;
                    _priorityQueue.Enqueue(_currentRequest);
                    _serviceRequestBST.Insert(_currentRequest);

                    // Create and add new card
                    var card = new ServiceRequestCard(this, _serviceRequestGraph)
                    {
                        DataContext = _currentRequest
                    };

                    var items = (ServiceRequestsControl.ItemsSource as List<ServiceRequestCard>) ?? new List<ServiceRequestCard>();
                    items.Add(card);
                    ServiceRequestsControl.ItemsSource = null;
                    ServiceRequestsControl.ItemsSource = items;
                }

                // Update UI
                UpdateHighPriorityList();
                ClearFormFields();
                IsFormVisible = false;
                DataContext = this;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Error saving changes: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Cancel" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            IsFormVisible = false;
            DataContext = this;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Manage Dependencies" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void ManageDependenciesButton_Click(object sender, RoutedEventArgs e)
        {
            var dependencyWindow = new DependencyManagementWindow(_currentRequest, _serviceRequestGraph);
            if (dependencyWindow.ShowDialog() == true && dependencyWindow.SelectedDependencies != null)
            {
                _currentRequest.Dependencies = new List<ServiceRequest>(dependencyWindow.SelectedDependencies);

                // Clear existing dependencies in graph
                _serviceRequestGraph.ClearDependencies(_currentRequest);

                // Add new dependencies to graph
                foreach (var dependency in _currentRequest.Dependencies)
                {
                    _serviceRequestGraph.AddDependency(_currentRequest, dependency);
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Refresh High Priority" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void RefreshHighPriority_Click(object sender, RoutedEventArgs e)
        {
            UpdateHighPriorityList();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Search" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void SearchButton_Click(object sender, RoutedEventArgs e)
        {
            SearchServiceRequests();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the key up event for the search text box.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The key event arguments.</param>
        private void SearchTextBox_KeyUp(object sender, KeyEventArgs e)
        {
            SearchServiceRequests();
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Attach Media" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private async void AttachMediaButton_Click(object sender, RoutedEventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png|Document Files|*.pdf;*.docx;*.txt",
                Title = "Select Media Files",
                Multiselect = true
            };

            if (openFileDialog.ShowDialog() == true)
            {
                List<string> validFiles = new List<string>();
                foreach (string filePath in openFileDialog.FileNames)
                {
                    try
                    {
                        MediaAttachment mediaAttachment = new MediaAttachment(
                            Path.GetFileName(filePath),
                            filePath,
                            MediaAttachment.LoadFileData(filePath),
                            typeof(object));

                        validFiles.Add(filePath);
                        _currentRequest.Attachments.Add(mediaAttachment);
                    }
                    catch (InvalidOperationException ex)
                    {
                        MessageBox.Show(ex.Message, "Invalid Media Attachment",
                            MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                if (validFiles.Any())
                {
                    AttachmentsListBox.ItemsSource = null;
                    AttachmentsListBox.ItemsSource = _currentRequest.Attachments;
                }
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Open Attachment" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void OpenAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (AttachmentsListBox.SelectedItem is MediaAttachment attachment)
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

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Remove Attachment" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void RemoveAttachment_Click(object sender, RoutedEventArgs e)
        {
            if (AttachmentsListBox.SelectedItem is MediaAttachment attachment)
            {
                _currentRequest.Attachments.Remove(attachment);
                AttachmentsListBox.ItemsSource = null;
                AttachmentsListBox.ItemsSource = _currentRequest.Attachments;
            }
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the click event for the "Populate Data" button.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The routed event arguments.</param>
        private void PopulateDataButton_Click(object sender, RoutedEventArgs e)
        {
            // Generate sample requests
            var serviceRequests = ServiceRequestDataGenerator.GenerateSampleRequests(15);

            // Add service requests to the graph
            foreach (var request in serviceRequests)
            {
                _serviceRequestGraph.AddServiceRequest(request);
            }

            // Add dependencies to the graph
            foreach (var request in serviceRequests)
            {
                foreach (var dependency in request.Dependencies)
                {
                    _serviceRequestGraph.AddDependency(request, dependency);
                }
            }

            // Add service requests to the priority queue
            foreach (var request in serviceRequests)
            {
                _priorityQueue.Enqueue(request);
            }

            // Update the UI
            var serviceRequestCards = serviceRequests.Select(sr => new ServiceRequestCard(this, _serviceRequestGraph)
            {
                DataContext = sr
            }).ToList();

            ServiceRequestsControl.ItemsSource = serviceRequestCards;

            // Disable the button after use
            PopulateDataButton.IsEnabled = false;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Searches for service requests based on the search criteria.
        /// </summary>
        private void SearchServiceRequests()
        {
            string searchText = SearchTextBox.Text?.Trim().ToLower();

            // If search is empty or only whitespace, show all requests
            if (string.IsNullOrWhiteSpace(searchText))
            {
                var allRequests = _serviceRequestGraph.GetAllServiceRequests();
                UpdateServiceRequestDisplay(allRequests.ToList());
                return;
            }

            var searchBy = (SearchByComboBox.SelectedItem as ComboBoxItem)?.Content.ToString() ?? "Description";
            var allServiceRequests = _serviceRequestGraph.GetAllServiceRequests();

            var filteredRequests = allServiceRequests.Where(request =>
            {
                return searchBy switch
                {
                    "Description" => request.Description?.ToLower().Contains(searchText) ?? false,
                    "Location" => request.Location?.ToLower().Contains(searchText) ?? false,
                    "Requester" => request.RequesterName?.ToLower().Contains(searchText) ?? false,
                    "Category" => request.Category?.ToLower().Contains(searchText) ?? false,
                    "Status" => request.Status?.ToLower().Contains(searchText) ?? false,
                    _ => false
                };
            }).ToList();

            UpdateServiceRequestDisplay(filteredRequests);
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Updates the service request display.
        /// </summary>
        /// <param name="requests">The list of service requests to display.</param>
        private void UpdateServiceRequestDisplay(List<ServiceRequest> requests)
        {
            var serviceRequestCards = requests.Select(sr => new ServiceRequestCard(this, _serviceRequestGraph)
            {
                DataContext = sr
            }).ToList();

            ServiceRequestsControl.ItemsSource = serviceRequestCards;
        }

        //------------------------------------------------------------------
        /// <summary>
        /// Handles the window closing event.
        /// </summary>
        /// <param name="sender">The sender object.</param>
        /// <param name="e">The cancel event arguments.</param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            _mainWindow.Show();
        }
    }
}

// ------------------------------EOF------------------------------------