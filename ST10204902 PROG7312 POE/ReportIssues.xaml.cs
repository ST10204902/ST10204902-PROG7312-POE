using Microsoft.Win32;
using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Documents;
using System.Windows.Media;
using System.Windows.Media.Animation;

namespace ST10204902_PROG7312_POE
{
    //---------------------------------------------------------------------------------------
    /// <summary>
    /// Interaction logic for ReportIssues.xaml
    /// </summary>
    public partial class ReportIssues : Window
    {
        //---------------------------------------------------------------------------------------
        //Variables
        private readonly MainWindow _mainWindow;

        private readonly IIssueRepository _issueRepository;

        private Issue _currentIssue = new Issue();
        private Task _updateProgressBarTask = Task.CompletedTask;
        private CancellationTokenSource _updateCancellationTokenSource = new CancellationTokenSource();
        private bool _isIssueSubmitted = false;

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Parameterized constructor. Initializes the ReportIssues window with the MainWindow instance.
        /// </summary>
        /// <param name="mainWindow"></param>
        public ReportIssues(MainWindow mainWindow, IIssueRepository issueRepository)
        {
            InitializeComponent();
            _mainWindow = mainWindow;
            _issueRepository = issueRepository;
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Default constructor. If this somehow runs, something has gone terribly wrong.
        /// </summary>
        public ReportIssues()
        {
            InitializeComponent();
            DataContext = this;
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Attach media files to the current issue.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void btnAttachMedia_Click(object sender, RoutedEventArgs e)
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
                        await Task.Run(() => _currentIssue.AddMediaAttachment(mediaAttachment));
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
                    UpdateProgressBar(); // Call UpdateProgressBar after updating the text and visibility
                }
            }
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Back button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
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

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Submit button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void btnSubmit_Click(object sender, RoutedEventArgs e)
        {
            if (ValidateFields())
            {
                //Cast selected item to ComboBoxItem to get its content
                ComboBoxItem selectedItem = (ComboBoxItem)cmbCategory.SelectedItem;

                _currentIssue.Location = txtLocation.Text;
                _currentIssue.Category = selectedItem.Content.ToString();
                TextRange textRange = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd);
                _currentIssue.Description = textRange.Text.Trim();

                _issueRepository.AddIssue(_currentIssue);
                foreach (var issue in _issueRepository.GetAllIssues())
                {
                    // Display the issue details
                    Console.WriteLine($"Issue at {issue.Location}");
                    Console.WriteLine($"Category: {issue.Category}");
                    Console.WriteLine($"Description: {issue.Description}");
                    Console.WriteLine("Media Attachments:");
                    foreach (string mediaAttachment in issue.GetMediaAttachmentDetails())
                    {
                        Console.WriteLine(mediaAttachment);
                    }
                }

                MessageBox.Show("Issue reported successfully.", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                _isIssueSubmitted = true;
                _mainWindow.Show();
                this.Close();
            }
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Validate the fields before submitting the form
        /// </summary>
        /// <returns></returns>
        private bool ValidateFields()
        {
            if (FieldsAreEmpty())
            {
                MessageBox.Show("Please fill in all fields before submitting.", "Incomplete Form", MessageBoxButton.OK, MessageBoxImage.Warning);
                return false;
            }
            return true;
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Check if any of the fields are empty
        /// </summary>
        /// <returns></returns>
        private bool FieldsAreEmpty()
        {
            return !LocationSelected() && !DescriptionEntered() && !CategorySelected() && !MediaIsAttached();
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Check if a location has been selected
        /// </summary>
        /// <returns></returns>
        private bool LocationSelected()
        {
            return !string.IsNullOrWhiteSpace(txtLocation.Text);
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Check if a description has been entered
        /// </summary>
        /// <returns></returns>
        private bool DescriptionEntered()
        {
            TextRange textRange = new TextRange(rtbDescription.Document.ContentStart, rtbDescription.Document.ContentEnd);
            return !string.IsNullOrWhiteSpace(textRange.Text.Trim());
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Check if a category has been selected
        /// </summary>
        /// <returns></returns>
        private bool CategorySelected()
        {
            return cmbCategory.SelectedIndex != -1 || cmbCategory.SelectedItem != null;
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Check if media is attached
        /// </summary>
        /// <returns></returns>
        private bool MediaIsAttached()
        {
            return txtMediaAttachment.Visibility == Visibility.Visible
            && !string.IsNullOrWhiteSpace(txtMediaAttachment.Text);
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Calculate the progress of the form completion
        /// </summary>
        /// <returns></returns>
        private int CalculateProgress()
        {
            int progress = 0;
            if (LocationSelected()) progress += 25;
            if (DescriptionEntered()) progress += 25;
            if (CategorySelected()) progress += 25;
            if (MediaIsAttached()) progress += 25;
            return progress;
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Set the color of the progress bar based on the progress
        /// </summary>
        /// <param name="progress"></param>
        private void SetProgressBarColor(int progress)
        {
            if (progress == 0)
            {
                reportProgressBar.Foreground = (Brush)FindResource("ProgressBarRed");
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

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Update the progress bar based on the form completion
        /// </summary>
        private void UpdateProgressBar()
        {
            //Cancel any pending update
            _updateCancellationTokenSource.Cancel();
            _updateCancellationTokenSource = new CancellationTokenSource();
            var cancellationToken = _updateCancellationTokenSource.Token;

            //Delay update to ensure it's not called too frequently
            _updateProgressBarTask = Task.Run(async () =>
            {
                try
                {
                    await Task.Delay(500, cancellationToken); //wait half a second before updating the progress bar

                    //check if task was cancelled during delay
                    cancellationToken.ThrowIfCancellationRequested();

                    Dispatcher.Invoke(() =>
                    {
                        int progress = CalculateProgress();
                        DoubleAnimation animation = new DoubleAnimation
                        {
                            From = reportProgressBar.Value,
                            To = progress,
                            Duration = TimeSpan.FromSeconds(1),
                            EasingFunction = new QuadraticEase
                            {
                                EasingMode = EasingMode.EaseInOut
                            }
                        };
                        reportProgressBar.BeginAnimation(ProgressBar.ValueProperty, animation);
                        SetProgressBarColor(progress);
                    });
                }
                catch (TaskCanceledException)
                {
                    //Task was cancelled
                }
            }, cancellationToken);
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Window closing event handler.
        /// Displays a confirm dialog if the user has entered anything into any of the fields
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Display a confirm dialog if the user has entered anything into any of the fields
            // including media attachments
            if (!_isIssueSubmitted && !FieldsAreEmpty())
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

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Text changed event handler for the location textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtLocation_TextChanged(object sender, TextChangedEventArgs e)
        {
            LocationValidationBrushUpdate();
            UpdateProgressBar();
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Selection changed event handler for the category combobox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void cmbCategory_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            CategoryValidationBrushUpdate();
            UpdateProgressBar();
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Text changed event handler for the description rich text box
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void rtbDescription_TextChanged(object sender, TextChangedEventArgs e)
        {
            DescriptionValidationBrushUpdate();
            UpdateProgressBar();
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Text changed event handler for the media attachment textbox
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void txtMediaAttachment_TextChanged(object sender, TextChangedEventArgs e)
        {
            UpdateProgressBar();
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Update the location validation brush
        /// </summary>
        private void LocationValidationBrushUpdate()
        {
            if (!LocationSelected())
            {
                txtLocation.BorderBrush = Brushes.Red;
            }
            else
            {
                txtLocation.BorderBrush = Brushes.Green;
            }
        }

        //---------------------------------------------------------------------------------------
        /// <summary>
        /// Update the category validation brush
        /// </summary>
        private void CategoryValidationBrushUpdate()
        {
            if (!CategorySelected())
            {
                cmbCategory.BorderBrush = Brushes.Red;
            }
            else
            {
                cmbCategory.BorderBrush = Brushes.Green;
            }
        }

        /// <summary>
        /// Update the description validation brush
        /// </summary>
        private void DescriptionValidationBrushUpdate()
        {
            if (!DescriptionEntered())
            {
                rtbDescription.BorderBrush = Brushes.Red;
            }
            else
            {
                rtbDescription.BorderBrush = Brushes.Green;
            }
        }
    }
}

//----------------------------------------EOF----------------------------------------------