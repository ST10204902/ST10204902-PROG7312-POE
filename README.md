# ST10204902 PROG7312 POE

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
- **Report Issues**: Users can report issues by providing details such as location, category, and description.
- **Attach Media**: Users can attach media files (images, videos) to the reported issues.
- **Progress Tracking**: The application tracks the progress of issue reporting.
- **Tooltips**: Provides tooltips for better user guidance.

## Getting Started

### Prerequisites
- Visual Studio
- .NET Framework

### Installation
1. Clone the repository:
    ```bash
    git clone https://github.com/yourusername/ST10204902_PROG7312_POE.git
    ```
2. Open the solution in Visual Studio:
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
2. The main window will provide options to report issues, view local events (coming soon), and check service status (coming soon).
3. Click on "Report Issues" to navigate to the issue reporting form.
4. Fill in the required details and attach any media files if necessary.
5. Submit the issue to save it.

## Project Structure
```ST10204902_PROG7312_POE/
├── Models/
│   ├── IIssueRepository.cs
│   ├── Issue.cs
│   └── MediaAttachment.cs
├── Views/
│   ├── MainWindow.xaml
│   ├── MainWindow.xaml.cs
│   ├── ReportIssues.xaml
│   └── ReportIssues.xaml.cs
├── App.xaml
├── App.xaml.cs
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
