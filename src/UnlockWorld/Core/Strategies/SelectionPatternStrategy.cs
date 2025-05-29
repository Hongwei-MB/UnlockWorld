using FlaUI.Core.AutomationElements;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to enable elements using the Selection pattern
    /// </summary>
    public class SelectionPatternStrategy : EnableStrategyBase
    {
        public SelectionPatternStrategy(LogHelper logger) : base(logger)
        {
        }

        public override bool CanHandle(AutomationElement element)
        {
            return element.Patterns.Selection.IsSupported;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {            Logger.Log("Attempting to enable using Selection pattern");
            
            // Try to select items which might enable the element
            var selectionPattern = element.Patterns.Selection.Pattern;
            // The Select method might require a parameter, let's try a different approach
            if (element.Patterns.SelectionItem.IsSupported)
            {
                element.Patterns.SelectionItem.Pattern.Select();
            }
            
            return VerifyEnabled(element);
        }
    }
}
