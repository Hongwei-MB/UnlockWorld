using FlaUI.Core.AutomationElements;
using System;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to enable elements using legacy accessibility features
    /// </summary>
    public class LegacyAccessibilityStrategy : EnableStrategyBase
    {
        private readonly NativeMethodsWrapper _nativeMethods;

        public LegacyAccessibilityStrategy(LogHelper logger, NativeMethodsWrapper nativeMethods) : base(logger)
        {
            _nativeMethods = nativeMethods;
        }

        public override bool CanHandle(AutomationElement element)
        {
            // This strategy can handle elements with native window handles
            return element.Properties.NativeWindowHandle.TryGetValue(out var handle) && handle != IntPtr.Zero;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {
            if (element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) && hWnd != IntPtr.Zero)
            {
                // Try to get the parent window
                var parentHwnd = _nativeMethods.GetRootWindow(hWnd);
                
                if (parentHwnd != IntPtr.Zero)
                {
                    Logger.Log("Attempting to enable parent window first");
                    _nativeMethods.EnableWindow(parentHwnd, true);
                    
                    if (VerifyEnabled(element))
                    {
                        return true;
                    }
                }
            }
            
            return false;
        }
    }
}
