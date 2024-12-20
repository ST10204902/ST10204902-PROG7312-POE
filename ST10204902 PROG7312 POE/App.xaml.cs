﻿using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using ST10204902_PROG7312_POE.Models;
using System.IO;
using System.Threading.Tasks;
using EventScraper;

namespace ST10204902_PROG7312_POE
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        //---------------------------------------------------------
        // Variable Declaration
        public IServiceProvider ServiceProvider { get; private set; }

        //----------------------------------------------------------------
        /// <summary>
        /// Start the application with the main window
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            try
            {
                var serviceCollection = new ServiceCollection();
                ConfigureServices(serviceCollection);
                ServiceProvider = serviceCollection.BuildServiceProvider();

                CheckEventCSV(ServiceProvider);

                var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
                mainWindow.Show();
            } catch (Exception ex)
            {
                MessageBox.Show($"Exception during startup: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                throw;
            }
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Configure the services for the application
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureServices(IServiceCollection services)
        {
            // Register your application's services here
            services.AddSingleton<IIssueRepository, IssueRepository>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ReportIssues>();
            services.AddTransient<ViewEvents>();

            // Use the EventService in EventRepository and EventSearcher
            services.AddSingleton<IEventRepository, EventRepository>();
        }

        //----------------------------------------------------------------
        /// <summary>
        /// Try to scrape the events from the website
        /// </summary>
        /// <param name="serviceProvider"></param>
        private static void CheckEventCSV(IServiceProvider serviceProvider)
        {
            Task.Run(async () =>
            {
                try
                {
                    var rootUrl = "https://eventsincapetown.com/all-events/";
                    var eventRepository = serviceProvider.GetRequiredService<IEventRepository>();

                    var ev = new EventScraper.EventScraper(rootUrl, eventRepository);

                    Console.WriteLine("Beginning web scraping for events...");

                    await ev.ScrapeAndStoreEventsAsync();

                    Console.WriteLine("Events have been scraped and are in memory");
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Failed to obtain online event information: " + ex.Message);
                }
            });
        }
    }
}
//-------------------------------EOF---------------------------------