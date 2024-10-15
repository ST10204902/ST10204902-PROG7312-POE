# ST10204902 PROG7312 POE

[GitHub Link](https://github.com/ST10204902/ST10204902-PROG7312-POE)

## Overview
This project is a WPF application designed to allow users to report issues related to municipal services. The application provides a user-friendly interface for submitting issues, attaching media files, and managing reported issues.

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
    ```bash
    dotnet restore
    ```

## Usage
1. Build and run the application from Visual Studio.
2. The main window will provide options to report issues, view local events, and check service status.
3. Click on "Report Issues" to navigate to the issue reporting form.
4. Fill in the required details and attach any media files if necessary.
5. Return to Main Menu by closing the report issues window
6. Open the view reported issues page to see previously reported issues.
7. Open the Event window to view events, apply search, filters, and sorting.

## Project Structure

```ST10204902_PROG7312_POE/
├── Models/
│   ├── IIssueRepository.cs
│   ├── Issue.cs
│   ├── MediaAttachment.cs
│   ├── Event.cs
│   ├── IEventRepository.cs
├── Components/
│   ├── EventCard.xaml
│   └── EventCard.xaml.cs
├── App.xaml
├── App.xaml.cs
├── MainWindow.xaml
├── MainWindow.xaml.cs
├── ReportIssues.xaml
├── ReportIssues.xaml.cs
├── ViewEvents.xaml
├── ViewEvents.xaml.cs
├── Properties/
│   ├── AssemblyInfo.cs
│   └── Resources.resx
├── ST10204902_PROG7312_POE.csproj
└── README.md
```

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
