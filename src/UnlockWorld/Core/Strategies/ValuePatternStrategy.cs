using FlaUI.Core.AutomationElements;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to enable elements using the Value pattern
    /// </summary>
    public class ValuePatternStrategy : EnableStrategyBase
    {
        public ValuePatternStrategy(LogHelper logger) : base(logger)
        {
        }

        public override bool CanHandle(AutomationElement element)
        {
            return element.Patterns.Value.IsSupported;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Attempting to enable using Value pattern");
            
            // Some controls can be activated by setting their value
            var valuePattern = element.Patterns.Value.Pattern;
            
            if (!valuePattern.IsReadOnly.ValueOrDefault)
            {                // Try with a dummy value
                var currentValue = valuePattern.Value.ValueOrDefault;
                // Use null check to avoid compiler warning
                if (valuePattern != null) 
                {
                    valuePattern.SetValue("test");
                    valuePattern.SetValue(currentValue ?? string.Empty); // Restore original value
                }
                
                if (VerifyEnabled(element))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
