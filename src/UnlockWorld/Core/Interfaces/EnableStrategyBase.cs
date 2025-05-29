using FlaUI.Core.AutomationElements;
using System;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Base class for enabling strategies with common functionality
    /// </summary>
    public abstract class EnableStrategyBase : IEnableStrategy
    {
        protected readonly LogHelper Logger;

        protected EnableStrategyBase(LogHelper logger)
        {
            Logger = logger;
        }

        public abstract bool CanHandle(AutomationElement element);
        
        public bool Execute(AutomationElement element)
        {
            try
            {
                Logger.Log($"Attempting to enable element using {GetType().Name}");
                bool result = ExecuteCore(element);
                
                if (result)
                {
                    Logger.LogSuccess($"Successfully enabled element using {GetType().Name}");
                }
                
                return result;
            }
            catch (Exception ex)
            {
                Logger.LogError($"Error in {GetType().Name}: {ex.Message}");
                return false;
            }
        }

        protected abstract bool ExecuteCore(AutomationElement element);
        
        protected bool VerifyEnabled(AutomationElement element)
        {
            return element.Properties.IsEnabled.TryGetValue(out bool isEnabled) && isEnabled;
        }
    }
}
