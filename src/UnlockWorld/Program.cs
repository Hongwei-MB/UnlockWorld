using System;
using System.Windows.Forms;
using System.Security.Principal;
using System.Diagnostics;
using System.Reflection;

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

        // To customize application configuration such as set high DPI settings or default font,
        // see https://aka.ms/applicationconfiguration.
        ApplicationConfiguration.Initialize();
        Application.Run(new Form1());
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
                Verb = "runas" // 请求管理员权限
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