using System;
using System.IO;
using System.Windows.Forms;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;

namespace UnlockWorld;

static class Program
{
    /// <summary>
    ///  The main entry point for the application.
    /// </summary>
    [STAThread]
    static void Main()
    {
        // Check if running with administrator privileges
        if (!IsRunningAsAdministrator())
        {
            // If not running as administrator, restart the application with admin rights
            RestartAsAdministrator();
            return;
        }

        // Configuration setup
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
            .Build();

        // Configure Serilog from config file
        Log.Logger = new LoggerConfiguration()
            .ReadFrom.Configuration(configuration)
            .CreateLogger();

        try
        {
            // Log application startup information
            Log.Information("Application is starting");
            Log.Information("Current working directory: {WorkingDirectory}", Environment.CurrentDirectory);
            Log.Information("Application base directory: {BaseDirectory}", AppDomain.CurrentDomain.BaseDirectory);

            // Create startup class and configure services
            var startup = new Startup(configuration);
            var services = new ServiceCollection();
            startup.ConfigureServices(services);

            // Build service provider
            var serviceProvider = services.BuildServiceProvider();

            Log.Information("Initializing Windows application");
            ApplicationConfiguration.Initialize();

            // Get StartMain from DI container and run
            Log.Information("Ready to launch main form");
            Application.Run(serviceProvider.GetRequiredService<MainForm>());
            Log.Information("Application exited normally");
        }
        catch (Exception ex)
        {
            Log.Fatal(ex, "Application startup failed");
            MessageBox.Show($"An error occurred during application startup: {ex.Message}", "Error",
                MessageBoxButtons.OK, MessageBoxIcon.Error);
        }
        finally
        {
            Log.Information("Application shutting down");
            Log.CloseAndFlush();
        }
    }

    /// <summary>
    /// Check if the application is running with administrator privileges
    /// </summary>
    private static bool IsRunningAsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>
    /// Restart the application with administrator privileges
    /// </summary>
    private static void RestartAsAdministrator()
    {
        try
        {
            ProcessStartInfo startInfo = new ProcessStartInfo
            {
                FileName = Assembly.GetExecutingAssembly().Location,
                UseShellExecute = true,
                Verb = "runas"
            };

            Process.Start(startInfo);
        }
        catch (Exception ex)
        {
            MessageBox.Show($"Unable to restart application with administrator privileges. Some features may not work properly.\n\nError: {ex.Message}",
                "Permission Error", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}