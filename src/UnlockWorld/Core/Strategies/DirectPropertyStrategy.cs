using FlaUI.Core.AutomationElements;
using System;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to directly set IsEnabled property
    /// </summary>
    public class DirectPropertyStrategy : EnableStrategyBase
    {
        private readonly NativeMethodsWrapper _nativeMethods;

        public DirectPropertyStrategy(LogHelper logger, NativeMethodsWrapper nativeMethods) : base(logger)
        {
            _nativeMethods = nativeMethods;
        }

        public override bool CanHandle(AutomationElement element)
        {
            // This strategy can try to handle any element
            return true;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {
            // Try using the Win32 API if we have a handle
            if (element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) && hWnd != IntPtr.Zero)
            {
                if (_nativeMethods.EnableWindow(hWnd, true))
                {
                    if (VerifyEnabled(element))
                    {
                        return true;
                    }
                }
                
                // Try sending enable message
                _nativeMethods.SendMessage(hWnd, NativeMethodsWrapper.WM_ENABLE, new IntPtr(1), IntPtr.Zero);
                
                if (VerifyEnabled(element))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
