using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to enable elements using the Toggle pattern
    /// </summary>
    public class TogglePatternStrategy : EnableStrategyBase
    {
        private readonly NativeMethodsWrapper _nativeMethods;

        public TogglePatternStrategy(LogHelper logger, NativeMethodsWrapper nativeMethods) : base(logger)
        {
            _nativeMethods = nativeMethods;
        }

        public override bool CanHandle(AutomationElement element)
        {
            return element.Patterns.Toggle.IsSupported;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Attempting to enable using Toggle pattern");
            
            var togglePattern = element.Patterns.Toggle.Pattern;
            var currentState = togglePattern.ToggleState.ValueOrDefault;
            
            // First try native toggle - often works even on disabled checkboxes
            if (element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) && hWnd != IntPtr.Zero)
            {
                _nativeMethods.ToggleCheckboxStateNative(hWnd);
                
                if (VerifyEnabled(element))
                {
                    return true;
                }
            }
            
            // Try UI Automation pattern
            togglePattern.Toggle();
            
            // For checkboxes, check if the toggle actually changed the state
            if (togglePattern.ToggleState.TryGetValue(out ToggleState newState) && newState != currentState)
            {
                // If the state changed, the control might be usable even if not formally enabled
                return true;
            }
            
            return VerifyEnabled(element);
        }
    }
}
