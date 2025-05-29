using FlaUI.Core.AutomationElements;
using FlaUI.Core.Definitions;
using System;
namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy to toggle checkbox state
    /// </summary>
    public class ToggleStrategy : EnableStrategyBase
    {
        private readonly NativeMethodsWrapper _nativeMethods;

        public ToggleStrategy(LogHelper logger, NativeMethodsWrapper nativeMethods) : base(logger)
        {
            _nativeMethods = nativeMethods;
        }

        public override bool CanHandle(AutomationElement element)
        {
            return element.Patterns.Toggle.IsSupported;
        }
        
        protected override bool ExecuteCore(AutomationElement element)
        {
            Logger.Log("Using specialized toggle strategy");
            
            // Use our specialized CheckboxHandler for clean, efficient toggle operation
            var checkboxHandler = new CheckboxHandler(Logger, _nativeMethods);
            return checkboxHandler.ToggleCheckbox(element);
        }
    }
}
