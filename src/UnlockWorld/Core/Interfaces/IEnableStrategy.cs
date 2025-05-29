using FlaUI.Core.AutomationElements;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Interface for element enabling strategies
    /// </summary>
    public interface IEnableStrategy
    {
        bool CanHandle(AutomationElement element);
        bool Execute(AutomationElement element);
    }
}
