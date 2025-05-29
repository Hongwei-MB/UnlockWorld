using FlaUI.Core.AutomationElements;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to enable elements using the ExpandCollapse pattern
    /// </summary>
    public class ExpandCollapsePatternStrategy : EnableStrategyBase
    {
        public ExpandCollapsePatternStrategy(LogHelper logger) : base(logger)
        {
        }

        public override bool CanHandle(AutomationElement element)
        {
            return element.Patterns.ExpandCollapse.IsSupported;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Attempting to enable using ExpandCollapse pattern");
            
            var expandCollapsePattern = element.Patterns.ExpandCollapse.Pattern;
            var currentState = expandCollapsePattern.ExpandCollapseState.ValueOrDefault;
            
            if (currentState == FlaUI.Core.Definitions.ExpandCollapseState.Collapsed)
            {
                // Try expanding the element
                expandCollapsePattern.Expand();
            }
            else
            {
                // Try toggling the state to see if it helps enable the element
                expandCollapsePattern.Collapse();
                expandCollapsePattern.Expand();
            }
            
            return VerifyEnabled(element);
        }
    }
}
