﻿using ST10204902_PROG7312_POE.Components;
using ST10204902_PROG7312_POE.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;
using System.Windows.Input;

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for ViewReportedIssues.xaml
    /// </summary>
    public partial class ViewReportedIssues : Window
    {
        private readonly MainWindow _mainWindow;
        private readonly IIssueRepository _issueRepository;

        public ObservableCollection<Issue> LastWeekIssues { get; set; }
        public ObservableCollection<Issue> LastMonthIssues { get; set; }
        public ObservableCollection<Issue> OlderIssues { get; set; }

        public ViewReportedIssues(MainWindow mainWindow, IIssueRepository issueRepository)
        {
            InitializeComponent();
            DataContext = this;

            _mainWindow = mainWindow;
            _issueRepository = issueRepository;

            LoadIssues();
        }

        public ViewReportedIssues()
        {
            InitializeComponent();
        }

        private void LoadIssues()
        {
            var issues = _issueRepository.GetAllIssues().ToList();

            LastWeekIssues = new ObservableCollection<Issue>(issues.Where(i => i.DateOfIssue >= DateTime.Now.AddDays(-7)));
            LastMonthIssues = new ObservableCollection<Issue>(issues.Where(i => i.DateOfIssue >= DateTime.Now.AddMonths(-1) && i.DateOfIssue < DateTime.Now.AddDays(-7)));
            OlderIssues = new ObservableCollection<Issue>(issues.Where(i => i.DateOfIssue < DateTime.Now.AddMonths(-1)));


            AddIssueCardsOrMessage(LastWeekIssues, LastWeekIssuesControl, "No issues reported in the last week");
            AddIssueCardsOrMessage(LastMonthIssues, LastMonthIssuesControl, "No issues reported in the last month");
            AddIssueCardsOrMessage(OlderIssues, OlderIssuesControl, "No issues reported older than a month");
        }

        private void AddIssueCardsOrMessage(ObservableCollection<Issue> issues, ItemsControl itemsControl, string message)
        {
            itemsControl.Items.Clear();

            if (issues.Count == 0)
            {
                var textBlock = new TextBlock
                {
                    Text = message,
                    Margin = new Thickness(10),
                    FontStyle = FontStyles.Italic
                };
                itemsControl.Items.Add(textBlock);
            }
            else
            {
                foreach (var issue in issues)
                {
                    var issueCard = new IssueCard
                    {
                        DataContext = issue
                    };
                    itemsControl.Items.Add(issueCard);
                }
            }
        }


        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e)
        {
            //Open MainWindow and close this window
            _mainWindow.Show();
            _mainWindow.Activate();
        }

        private void InnerScrollViewer_PreviewMouseWheel(object sender, MouseWheelEventArgs e)
        {
            ScrollViewer scrollViewer = sender as ScrollViewer;
            if (scrollViewer != null && !e.Handled)
            {
                e.Handled = true;
                MouseWheelEventArgs eventArg = new MouseWheelEventArgs(e.MouseDevice, e.Timestamp, e.Delta);
                eventArg.RoutedEvent = UIElement.MouseWheelEvent;
                eventArg.Source = sender;
                OuterScrollViewer.RaiseEvent(eventArg);
            }
        }
    }
}