using FlaUI.Core.AutomationElements;
using System;
namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to enable checkboxes
    /// </summary>
    public class CheckboxStrategy : EnableStrategyBase
    {
        private readonly NativeMethodsWrapper _nativeMethods;

        public CheckboxStrategy(LogHelper logger, NativeMethodsWrapper nativeMethods) : base(logger)
        {
            _nativeMethods = nativeMethods;
        }

        public override bool CanHandle(AutomationElement element)
        {
            return element.Patterns.Toggle.IsSupported;
        }        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Using specialized checkbox enabling strategy");
            
            // Use our specialized CheckboxHandler for a clean, efficient approach
            var checkboxHandler = new CheckboxHandler(Logger, _nativeMethods);
            
            // For enabling, we just need to prepare the checkbox - we don't actually toggle it
            if (element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) && hWnd != IntPtr.Zero)
            {
                Logger.Log("Applying minimal checkbox enabling technique");
                
                // Simply remove read-only flags from control and parent
                _nativeMethods.RemoveReadOnlyFlag(hWnd);
                
                var parentHwnd = _nativeMethods.GetParentWindow(hWnd);
                if (parentHwnd != IntPtr.Zero)
                {
                    _nativeMethods.RemoveReadOnlyFlag(parentHwnd);
                }
                
                // Send minimal visual enhancement
                checkboxHandler.EnhanceVisualAppearance(hWnd);
                
                // We consider the operation successful if we were able to apply these changes
                return true;
            }
            
            // If no handle, try using UI Automation pattern
            var togglePattern = element.Patterns.Toggle.Pattern;
            if (togglePattern != null)
            {
                // We don't actually toggle here, we just check if the pattern is accessible
                try
                {
                    // Just check if we can access the state
                    var state = togglePattern.ToggleState.ValueOrDefault;
                    Logger.Log($"Successfully accessed toggle pattern state: {state}");
                    return true;
                }
                catch (Exception ex)
                {
                    Logger.LogWarning($"Error accessing Toggle pattern: {ex.Message}");
                }
            }
            
            return false;
        }
    }
}
