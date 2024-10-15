using System.Windows;

namespace ST10029256_PROG7312
{
    // Partial class for the App which inherits from Application.
    // This class handles application-wide events such as startup and unhandled exceptions.
    public partial class App : Application
    {
        // Overrides the OnStartup method to initialize the application and register event handlers.
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e); // Calls the base class's OnStartup to ensure the default startup behavior occurs.

            // Registers an event handler to catch unhandled exceptions on the application's main thread.
            this.DispatcherUnhandledException += App_DispatcherUnhandledException;
        }

        // Event handler that is triggered when an unhandled exception occurs in the main thread.
        private void App_DispatcherUnhandledException(object sender, System.Windows.Threading.DispatcherUnhandledExceptionEventArgs e)
        {
            // Displays a message box to inform the user that an unexpected error has occurred.
            // Shows the exception's message in the error dialog.
            MessageBox.Show($"An unexpected error occurred: {e.Exception.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);

            // Marks the exception as handled to prevent the application from crashing.
            e.Handled = true;
        }
    }
}
