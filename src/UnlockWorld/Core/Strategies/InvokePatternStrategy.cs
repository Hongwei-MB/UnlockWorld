using FlaUI.Core.AutomationElements;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to enable elements using the Invoke pattern
    /// </summary>
    public class InvokePatternStrategy : EnableStrategyBase
    {
        public InvokePatternStrategy(LogHelper logger) : base(logger)
        {
        }

        public override bool CanHandle(AutomationElement element)
        {
            return element.Patterns.Invoke.IsSupported;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Attempting to enable using Invoke pattern");
            var invokePattern = element.Patterns.Invoke.Pattern;
            
            // Try to invoke the element which might reset its state
            invokePattern.Invoke();
            Logger.Log("Element invoked");
            
            return VerifyEnabled(element);
        }
    }
}
