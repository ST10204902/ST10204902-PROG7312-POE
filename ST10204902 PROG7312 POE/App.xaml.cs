using Microsoft.Extensions.DependencyInjection;
using System;
using System.Windows;
using ST10204902_PROG7312_POE.Models;

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

        /// <summary>
        /// Start the application with the main window
        /// </summary>
        /// <param name="e"></param>
        protected override void OnStartup(StartupEventArgs e)
        {
            var serviceCollection = new ServiceCollection();
            ConfigureServices(serviceCollection);
            ServiceProvider = serviceCollection.BuildServiceProvider();

            var mainWindow = ServiceProvider.GetRequiredService<MainWindow>();
            mainWindow.Show();
        }

        /// <summary>
        /// Configure the services for the application
        /// </summary>
        /// <param name="services"></param>
        private void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<IIssueRepository, IssueRepository>();
            services.AddSingleton<MainWindow>();
            services.AddSingleton<ReportIssues>();
        }
    }
}
//----------------------------------EOF------------------------------