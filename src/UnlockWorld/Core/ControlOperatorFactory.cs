using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Automation;

namespace UnlockWorld.Core
{
    /// <summary>
    /// 控制器上下文，根据窗口选择最佳处理器
    /// </summary>
    public class ControlOperatorFactory
    {
        public static IControlOperator Create(nint hWnd)
        {
            // 优先尝试 UI Automation
            try
            {
                var element = AutomationElement.FromHandle(hWnd);
                if (element != null && element.Current.ControlType != null)
                {
                    return new UIAutomationControlOperator(element);
                }
            }
            catch
            {
                // UIA 获取失败，继续尝试其他方案
            }

            string className = GetWindowClassName(hWnd).ToLowerInvariant();
            if (className.StartsWith("windowsforms") || className.Contains("edit") || className.Contains("button"))
            {
                return new Win32ControlOperator(hWnd);
            }

            // 可扩展支持 Avalonia 等非标准应用标识判断
            var processName = GetProcessNameByHWnd(hWnd);
            if (processName.Contains("avalonia", StringComparison.OrdinalIgnoreCase))
            {
                // Avalonia 默认无 UIA 支持，使用 Fallback
                return new FallbackControlOperator(hWnd);
            }

            // 默认使用兜底
            return new FallbackControlOperator(hWnd);
        }

        public static string GetWindowClassName(nint hWnd)
        {
            var className = new StringBuilder(256);
            GetClassName(hWnd, className, className.Capacity);
            return className.ToString();
        }

        public static string GetProcessNameByHWnd(nint hWnd)
        {
            _ = GetWindowThreadProcessId(hWnd, out uint processId);
            try
            {
                return Process.GetProcessById((int)processId).ProcessName;
            }
            catch
            {
                return string.Empty;
            }
        }

        [DllImport("user32.dll", SetLastError = true)]
        private static extern int GetClassName(nint hWnd, StringBuilder lpClassName, int nMaxCount);

        [DllImport("user32.dll", SetLastError = true)]
        private static extern uint GetWindowThreadProcessId(nint hWnd, out uint lpdwProcessId);
    }
}
