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

namespace ST10204902_PROG7312_POE.Components
{
    /// <summary>
    /// Interaction logic for InputDialog.xaml
    /// </summary>
    public partial class InputDialog : Window
    {
        //--------------------------------------------------------------------------------
        //Variables
        public string SelectedFileName { get; private set; }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Parameterized constructor
        /// </summary>
        /// <param name="title"></param>
        /// <param name="fileNames"></param>
        public InputDialog(string title, string fileNames)
        {
            InitializeComponent();
            Title = title;
            FileNamesListBox.ItemsSource = fileNames.Split(new[] { Environment.NewLine }, StringSplitOptions.None);
        }

        //--------------------------------------------------------------------------------
        /// <summary>
        /// Selection changed event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void FileNamesListBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (FileNamesListBox.SelectedItem != null)
            {
                FileNameTextBox.Text = FileNamesListBox.SelectedItem.ToString();
            }
        }
        //--------------------------------------------------------------------------------
        /// <summary>
        /// Ok button click event handler
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void OkButton_Click(object sender, RoutedEventArgs e)
        {
            SelectedFileName = FileNameTextBox.Text;
            DialogResult = true;
        }

        private void CancelButton_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
        }
    }
}
//---------------------------------EOF-----------------------------------------------