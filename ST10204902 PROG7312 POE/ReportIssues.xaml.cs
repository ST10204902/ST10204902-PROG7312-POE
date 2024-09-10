using Microsoft.Win32;
using ST10204902_PROG7312_POE.Models;
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
using Windows.Foundation.Diagnostics;

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for ReportIssues.xaml
    /// </summary>
    public partial class ReportIssues : Window
    {
        private MainWindow _mainWindow;
        private Issue _currentIssue = new Issue();

        /// <summary>
        /// Parameterized constructor. Initializes the ReportIssues window with the MainWindow instance.
        /// </summary>
        /// <param name="mainWindow"></param>
        public ReportIssues(MainWindow mainWindow)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
        }

        /// <summary>
        /// Default constructor. If this somehow runs, something has gone terribly wrong.
        /// </summary>
        public ReportIssues()
        {
            InitializeComponent();
        }

        private void btnAttachMedia_Click(object sender, RoutedEventArgs e)
        {
            // Open a multiselect file dialog that allows standard image and document files
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files|*.jpg;*.jpeg;*.png|Document Files|*.pdf;*.docx;*.txt",
                Title = "Select a Media File",
                Multiselect = true
            };

            // Show the dialog and get result
            if (openFileDialog.ShowDialog() == true)
            {
                List<string> validFiles = new List<string>();
                foreach (string filePath in openFileDialog.FileNames)
                {
                    try
                    {
                        // Attempt to create a MediaAttachment instance
                        MediaAttachment mediaAttachment = new MediaAttachment(System.IO.Path.GetFileName(filePath), filePath, typeof(object)); // Replace typeof(object) with actual type if known
                        validFiles.Add(filePath);
                        // Add the media attachment to the current issue (assuming you have a currentIssue object)
                        _currentIssue.AddMediaAttachment(mediaAttachment);
                    }
                    catch (InvalidOperationException ex)
                    {
                        // Handle the exception, e.g., show a message to the user
                        MessageBox.Show(ex.Message, "Invalid Media Attachment", MessageBoxButton.OK, MessageBoxImage.Error);
                    }
                }

                // Display the valid file names in the text box and make it visible
                if (validFiles.Any())
                {
                    txtMediaAttachment.Text = string.Join(",", validFiles);
                    txtMediaAttachment.Visibility = Visibility.Visible;
                }
            }
        }

        private void btnBack_Click(object sender, RoutedEventArgs e)
        {
            //Show the main window and close current window
            // Display a confirm dialog if the user has entered anything into any of the fields
            // including media attachments

            if (!FieldsAreEmpty())
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to go back? Any unsaved data will be lost.", "Go Back", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.Yes)
                {
                    _mainWindow.Show();
                    this.Close();
                }
            }
            else
            {
                //Show an information messagebox
                _mainWindow.Show();
                this.Close();
            }
        }

        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show(_currentIssue.GetMediaAttachmentDetails().ToString(), "Media Attachments", MessageBoxButton.OK, MessageBoxImage.Information);
        }

        private bool FieldsAreEmpty()
        {
            bool _ = !LocationSelected() && !DescriptionEntered() && !CategorySelected() && !MediaIsAttached();
            return _;
        }

        private bool LocationSelected()
        {
            bool _ = !string.IsNullOrWhiteSpace(txtLocation.Text);
            return _;
        }

        private bool DescriptionEntered()
        {
            TextRange textRange = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd);
            return !string.IsNullOrWhiteSpace(textRange.Text.Trim());
        }

        private bool CategorySelected()
        {
            bool _ = cmbCategory.SelectedIndex != -1 || cmbCategory.SelectedItem != null;
            return _;
        }

        private bool MediaIsAttached()
        {
            bool _ = txtMediaAttachment.Visibility == Visibility.Visible
                && !string.IsNullOrWhiteSpace(txtMediaAttachment.Text);
            return _;
        }

        private void UpdateProgressBar()
        {
            int progress = 0;
            if (LocationSelected())
            {
                progress += 25;
            }
            if (DescriptionEntered())
            {
                progress += 25;
            }
            if (CategorySelected())
            {
                progress += 25;
            }
            if (MediaIsAttached())
            {
                progress += 25;
            }

            reportProgressBar.Value = progress;

            //Update the colour based on progress
            if (progress == 0)
            {
                reportProgressBar.Foreground = (Brush)FindResource("ProgressBarRed");
            }
            else if (progress == 50)
            {
                reportProgressBar.Foreground = (Brush)FindResource("ProgressBarYellow");
            }
            else if (progress == 100)
            {
                reportProgressBar.Foreground = (Brush)FindResource("ProgressBarGreen");
            }
            else
            {
                reportProgressBar.Foreground = (Brush)FindResource("ProgressBarYellow");
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Display a confirm dialog if the user has entered anything into any of the fields
            // including media attachments
            if (!FieldsAreEmpty())
            {
                MessageBoxResult result = MessageBox.Show("Are you sure you want to go back? Any unsaved data will be lost.",
                    "Go Back", MessageBoxButton.YesNo, MessageBoxImage.Question);
                if (result == MessageBoxResult.No)
                {
                    //Cancel the closing event
                    e.Cancel = true;
                }
                else
                {
                    //Show the main window
                    _mainWindow.Show();
                }
            }
            else
            {
                _mainWindow.Show();
            }
        }

        private void txtLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void rtbDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        private void txtMediaAttachment_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgressBar();
        }
    }
}