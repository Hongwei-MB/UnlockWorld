using System;
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
        // 检查是否已经以管理员权限运行
        if (!IsRunningAsAdministrator())
        {
            // 如果不是管理员权限，尝试重新以管理员权限启动
            RestartAsAdministrator();
            return;
        }

        // 配置管理
        var configuration = new ConfigurationBuilder()
            .SetBasePath(AppDomain.CurrentDomain.BaseDirectory)
            .AddJsonFile("appsettings.json", optional: true, reloadOnChange: true)
            .Build();

        // Serilog集成
        Log.Logger = new LoggerConfiguration()
            .WriteTo.Console()
            .WriteTo.File("Logs/log.txt", rollingInterval: RollingInterval.Day)
            .CreateLogger();

        // 依赖注入容器
        var services = new ServiceCollection();
        services.AddSingleton<IConfiguration>(configuration);
        services.AddLogging(loggingBuilder => loggingBuilder.AddSerilog());
        services.AddTransient<Form1>();
        // 注册其他服务: services.AddTransient<IMyService, MyService>();

        var serviceProvider = services.BuildServiceProvider();

        ApplicationConfiguration.Initialize();
        // 通过DI获取Form1示例（如需注入服务可修改Form1构造函数）
        Application.Run(serviceProvider.GetRequiredService<Form1>());
    }

    /// <summary>
    /// 检查应用程序是否以管理员权限运行
    /// </summary>
    private static bool IsRunningAsAdministrator()
    {
        WindowsIdentity identity = WindowsIdentity.GetCurrent();
        WindowsPrincipal principal = new WindowsPrincipal(identity);
        return principal.IsInRole(WindowsBuiltInRole.Administrator);
    }

    /// <summary>
    /// 重新以管理员权限启动应用程序
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
            MessageBox.Show($"无法以管理员权限启动应用程序。某些功能可能无法正常工作。\n\n错误: {ex.Message}",
                "权限错误", MessageBoxButtons.OK, MessageBoxIcon.Warning);
        }
    }
}