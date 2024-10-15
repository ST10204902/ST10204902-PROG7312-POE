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
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace ST10204902_PROG7312_POE.Components
{
    /// <summary>
    /// Interaction logic for IssueCard.xaml
    /// </summary>
    public partial class IssueCard : UserControl
    {
        /// <summary>
        /// Default constructor
        /// </summary>
        public IssueCard()
        {
            InitializeComponent();
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Occurs when the issue card is clicked.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OnIssueCardClick(object sender, MouseButtonEventArgs e)
        {
            // Mark the event as handled
            e.Handled = true;

            if (DataContext is Issue issue)
            {
                ViewDetails(issue);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// View the details of the issue.
        /// </summary>
        /// <param name="issue"></param>
        private void ViewDetails(Issue issue)
        {
            if (issue.MediaAttachments == null || !issue.MediaAttachments.Any())
            {
                MessageBox.Show("No media attachments available for this issue.", "No Media Attachments", MessageBoxButton.OK, MessageBoxImage.Information);
                return;
            }

            var fileNames = issue.MediaAttachments.Select(m => m.FileName).ToList();
            var fileNamesString = string.Join(Environment.NewLine, fileNames);

            var selectedFileName = PromptUserToSelectFile(fileNamesString);
            if (selectedFileName == null)
            {
                return; //User canceled selection
            }

            //Find selected media attachment
            var selectedMediaAttachment = issue.MediaAttachments.FirstOrDefault(m => m.FileName == selectedFileName);
            if (selectedMediaAttachment != null)
            {
                try
                {
                    OpenFile(selectedMediaAttachment.FilePath);
                }
                catch
                {
                    MessageBox.Show("An error occurred while trying to open the file. Has the file's location changed?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
            else
            {
                MessageBox.Show("The selected media attachment could not be found.", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Prompt the user to select a file to open.
        /// </summary>
        /// <param name="fileNames"></param>
        /// <returns></returns>
        private string PromptUserToSelectFile(string fileNames)
        {
            var inputDialog = new InputDialog("Select a file to open:", fileNames);
            if (inputDialog.ShowDialog() == true)
            {
                return inputDialog.SelectedFileName;
            }
            return null;
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Try to open the file at the specified file path.
        /// </summary>
        /// <param name="filePath"></param>
        private static void OpenFile(string filePath)
        {
            try
            {
                System.Diagnostics.Process.Start(filePath);
            }
            catch
            {
                MessageBox.Show("An error occurred while trying to open the file. Has the file's location changed?", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}
//----------------------------------EOF----------------------------------------------