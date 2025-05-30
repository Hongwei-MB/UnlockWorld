using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Win32 控件操作器
    /// </summary>
    public class Win32ControlOperator : IControlOperator
    {
        private nint _hWnd;

        public Win32ControlOperator(nint hWnd)
        {
            _hWnd = hWnd;
        }

        public bool CanHandle(nint hWnd)
        {
            string className = ControlOperatorFactory.GetWindowClassName(hWnd);
            return className.StartsWith("WindowsForms", StringComparison.OrdinalIgnoreCase) ||
                   className.StartsWith("Edit") ||
                   className.StartsWith("Button");
        }

        public bool EnableControl()
        {
            return EnableWindow(_hWnd, true);
        }

        public bool SetValue(string value)
        {
            return SendMessage(_hWnd, WM_SETTEXT, nint.Zero, value) != nint.Zero;
        }

        [DllImport("user32.dll")]
        private static extern bool EnableWindow(nint hWnd, bool bEnable);

        [DllImport("user32.dll", CharSet = CharSet.Auto)]
        private static extern nint SendMessage(nint hWnd, int msg, nint wParam, string lParam);

        private const int WM_SETTEXT = 0x000C;
    }
}
