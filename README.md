# ST10204902 PROG7312 POE

[GitHub Link](https://github.com/ST10204902/ST10204902-PROG7312-POE)

## Overview
This project is a WPF application designed to allow users to report issues, view local events, and manage service requests. Issues can be reported and viewed, events can be viewed and searched, and service requests can be created, tracked, and managed. You can navigate through the application using the menus on the main window. Each part of the application has its own window and functionality. 

## Table of Contents
- [Overview](#overview)
- [Features](#features)
- [Getting Started](#getting-started)
- [Prerequisites](#prerequisites)
- [Installation](#installation)
- [Usage](#usage)
- [Project Structure](#project-structure)
- [Contributing](#contributing)
- [License](#license)

## Features
### Part 1: Issue Reporting
- **Report Issues**: Users can report issues by providing details such as location, category, and description.
- **Attach Media**: Users can attach media files (images and documents) to the reported issues.
- **Tooltips**: Provides tooltips for better user guidance.
- **Feedback**: Users can submit feedback about the app via a Google Form.

### Part 2: Viewing Local Events
- **Event Display & Grouping**: Events are displayed in a grouped format, categorized by month and year for easy navigation. Custom event cards (`EventCard`) are used to display individual event information.
- **Search & Filter Functionality**: Users can search for events by keywords, and filter events by selecting a category. Matches are prioritized by title, venue, and description.
- **Sort Functionality**: Events can be sorted by date, title, or venue in both ascending and descending order.
- **Recommended Events Section**: Users' search terms are tracked to generate recommendations, and the top 5 recommended events are displayed.
- **UI Components**: The UI leverages WPF components, with expandable sections for grouped events, search results, and recommended events.
- **Clear Functionality**: The "Clear" button resets filters, search results, and recommendations while preserving grouped event expanders.

### Part 3: Service Request Management
- **Service Request Management**
  - Create and track service requests with priority levels
  - Attach media files to requests
  - Manage dependencies between related requests
  - Track request status and resolution
  - View detailed request history

- **Advanced Data Structures**
  - Priority Queue: Manages requests based on urgency
  - Binary Search Tree: Enables efficient request lookup and organization
  - Graph Implementation: Handles request dependencies and relationships
  - Status History Tracking: Maintains complete request lifecycle

- **Search and Filter Capabilities**
  - Search by multiple criteria (Description, Location, Requester, etc.)
  - Filter by status and priority
  - View high-priority requests separately

## Technical Implementation

### Data Structures
1. **Priority Queue (Min Heap)**
   - Organizes requests by priority level
   - Supports priority updates and rebalancing
   - Enables quick access to high-priority items

2. **Binary Search Tree Using AVL**
   - Efficient request storage and retrieval
   - Supports multiple search criteria
   - Maintains sorted order for reporting

3. **Dependency Graph**
   - Tracks relationships between requests
   - Implements cycle detection
   - Supports both DFS and BFS traversal

## Getting Started
### Prerequisites
- Visual Studio Code 2022
- .NET 4.8
- C# 8

### Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/ST10204902/ST10204902_PROG7312_POE.git
    ```
2. Open the solution in Visual Studio Code 2022:
    ```bash
    cd ST10204902_PROG7312_POE
    start ST10204902_PROG7312_POE.sln
    ```
3. Restore the NuGet packages:
    - Right click the solution in the Solution Explorer
    - Select "Restore NuGet Packages"
    - Or use the Package Manager Console:
    ```bash
    Update-Package -reinstall
    ```
4. Build the solution:
    - Press F6 or
    - Build -> Build Solution

### Running the Application
1. Press F5 to run in debug mode, or
2. Press Ctrl+F5 to run without debugging
3. Use the application by navigating through the menus

## Usage Guide
1. The main window will provide options to report issues, view local events, and check service status.
2. Click on "Report Issues" to navigate to the issue reporting form.
3. Fill in the required details and attach any media files if necessary.
4. Return to Main Menu by closing the report issues window
5. Open the view reported issues page to see previously reported issues.
6. Open the Event window to view events, apply search, filters, and sorting.
7. Open the Service Request window to create, track, and manage service requests.
8. There is a populate service request data generator that will populate the service request window with sample data. Just click the "Populate Data" button.

### Creating a Service Request
1. Click "Add New Request"
2. Fill in required fields:
   - Description
   - Location
   - Priority level
   - Category
3. Add any attachments if needed
4. Click "Save"

### Manage Dependencies
1. Click "Manage Dependencies"
2. Click "Add Dependency"
3. Select a request to depend on from the list
4. Click "Save"

### Resolving a Request
1. Edit an existing request
2. Set resolution date and comments
3. System will automatically update status
4. Dependencies will be checked before resolution

## Project Structure

```ST10204902_PROG7312_POE/
├── Components/
│ ├── ServiceRequestCard.xaml
│ ├── ServiceRequestCard.xaml.cs
│ ├── EventCard.xaml
│ └── EventCard.xaml.cs
├── Constants/
│ └── ApplicationConstants.cs
├── Converters/
│ └── PriorityToStringConverter.cs
├── DataStructures/
│ ├── ServiceRequestBST.cs
│ ├── ServiceRequestGraph.cs
│ └── ServiceRequestPriorityQueue.cs
├── Helpers/
│ └── ServiceRequestDataGenerator.cs
├── Models/
│ ├── ServiceRequest.cs
│ ├── MediaAttachment.cs
│ ├── Event.cs
│ ├── Issue.cs
│ ├── IEventRepository.cs
│ └── IIssueRepository.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── ServiceRequestWindow.xaml
├── ServiceRequestWindow.xaml.cs
├── ServiceRequestDetailsWindow.xaml
├── ServiceRequestDetailsWindow.xaml.cs
├── DependencyManagementWindow.xaml
├── DependencyManagementWindow.xaml.cs
├── ReportIssues.xaml
├── ReportIssues.xaml.cs
├── ViewEvents.xaml
├── ViewEvents.xaml.cs
├── App.xaml
├── App.xaml.cs
└── README.md
```

### Directory Structure Explanation
- **Components/**: Contains reusable UI components
  - ServiceRequestCard: Card component for displaying service requests
  - EventCard: Card component for displaying events

- **Constants/**: Application-wide constant values

- **Converters/**: Value converters for XAML bindings
  - PriorityToStringConverter: Converts priority values to display strings

- **DataStructures/**: Custom data structure implementations
  - ServiceRequestBST: Binary Search Tree for service requests
  - ServiceRequestGraph: Graph structure for request dependencies
  - ServiceRequestPriorityQueue: Priority queue for request urgency

- **Helpers/**: Utility and helper classes
  - ServiceRequestDataGenerator: Generates sample service request data

- **Models/**: Data models and interfaces
  - ServiceRequest: Main service request model
  - Other supporting models and interfaces


## Contributing
Contributions are welcome! Please fork the repository and create a pull request with your changes.

1. Fork the repository.
2. Create a new branch:
    ```bash
    git checkout -b feature-branch
    ```
3. Commit your changes:
    ```bash
    git commit -am 'Add new feature'
    ```
4. Push to the branch:
    ```bash
    git push origin feature-branch
    ```
5. Create a new Pull Request.

## License
This project is licensed under the MIT License. See the LICENSE file for details.
