using FlaUI.Core.AutomationElements;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to enable elements using the Window pattern
    /// </summary>
    public class WindowPatternStrategy : EnableStrategyBase
    {
        public WindowPatternStrategy(LogHelper logger) : base(logger)
        {
        }

        public override bool CanHandle(AutomationElement element)
        {
            return element.Patterns.Window.IsSupported;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Attempting to enable using Window pattern");
            
            // Try to focus the window which might enable elements
            var windowPattern = element.Patterns.Window.Pattern;
            
            if (!windowPattern.CanMaximize.ValueOrDefault)
            {
                Logger.LogWarning("Window cannot be maximized, skipping this strategy");
                return false;
            }
            
            // Try setting active state
            windowPattern.SetWindowVisualState(FlaUI.Core.Definitions.WindowVisualState.Normal);
            
            if (VerifyEnabled(element))
            {
                return true;
            }
            
            return false;
        }
    }
}
