using FlaUI.Core.AutomationElements;
using System;
namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy specifically for removing read-only states from controls
    /// </summary>
    public class ReadOnlyRemovalStrategy : EnableStrategyBase
    {
        private readonly NativeMethodsWrapper _nativeMethods;

        public ReadOnlyRemovalStrategy(LogHelper logger, NativeMethodsWrapper nativeMethods) : base(logger)
        {
            _nativeMethods = nativeMethods;
        }

        public override bool CanHandle(AutomationElement element)
        {
            // This strategy can handle any control with a native window handle
            // It's designed to be tried early in the chain
            return element.Properties.NativeWindowHandle.IsSupported;
        }        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Using read-only removal strategy");
            
            if (!element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) || hWnd == IntPtr.Zero)
            {
                Logger.LogWarning("Element doesn't have a valid window handle");
                return false;
            }
            
            // Check if read-only flag is set
            bool hadReadOnlyFlag = _nativeMethods.HasReadOnlyFlag(hWnd);
            if (hadReadOnlyFlag)
            {
                Logger.Log("Read-only flag detected - attempting to remove");
            }
            
            // Handle special case for checkboxes
            string className = _nativeMethods.GetClassName(hWnd);
            bool isCheckbox = className.Contains("Button") || className.Contains("Check");
            
            if (isCheckbox && element.Patterns.Toggle.IsSupported)
            {
                // If this is a checkbox, we should just remove the read-only flag
                // and let the CheckboxStrategy or ToggleStrategy handle the rest
                Logger.Log("Checkbox detected - applying minimal read-only handling");
                
                // Just remove read-only flags from the control and its parent
                _nativeMethods.RemoveReadOnlyFlag(hWnd);
                
                var parentHwnd = _nativeMethods.GetParentWindow(hWnd);
                if (parentHwnd != IntPtr.Zero)
                {
                    _nativeMethods.RemoveReadOnlyFlag(parentHwnd);
                }
                
                return true;
            }
            
            // Standard approach for other controls
            bool readOnlyRemoved = _nativeMethods.RemoveReadOnlyFlag(hWnd);
            
            // Try to enable the window too - but only once
            bool windowEnabled = _nativeMethods.EnableWindow(hWnd, true);
            
            Logger.Log($"Read-only removal results: Flag removed={readOnlyRemoved}, Window enabled={windowEnabled}");
            
            // For read-only strategy, even partial success is still success
            return readOnlyRemoved || windowEnabled || hadReadOnlyFlag;
        }
    }
}
