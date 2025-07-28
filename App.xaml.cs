using System;
using System.Windows;

namespace GitVersionControl
{
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            
            // Initialize any application-wide settings here
            AppDomain.CurrentDomain.UnhandledException += (sender, args) =>
            {
                MessageBox.Show($"An unexpected error occurred: {args.ExceptionObject}", 
                               "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            };
        }
    }
} 