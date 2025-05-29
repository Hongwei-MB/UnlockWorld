using FlaUI.Core.AutomationElements;
using System.Reflection;

namespace UnlockWorld.Core
{
    /// <summary>
    /// Strategy using reflection as a last resort
    /// </summary>
    public class ReflectionStrategy : EnableStrategyBase
    {
        private readonly NativeMethodsWrapper _nativeMethods;

        public ReflectionStrategy(LogHelper logger, NativeMethodsWrapper nativeMethods) : base(logger)
        {
            _nativeMethods = nativeMethods;
        }

        public override bool CanHandle(AutomationElement element)
        {
            // This is a fallback strategy that can handle any element type
            return true;
        }

        protected override bool ExecuteCore(AutomationElement element)
        {
            // First try Win32 APIs if available
            if (element.Properties.NativeWindowHandle.TryGetValue(out var hWnd) && hWnd != IntPtr.Zero)
            {
                _nativeMethods.SendAdvancedWindowMessages(hWnd);
                
                if (VerifyEnabled(element))
                {
                    return true;
                }
            }
            
            // As a last resort, try reflection directly on the element
            var elementType = element.GetType();
            var enableMethod = elementType.GetMethod("SetEnabled", 
                BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static);
            
            if (enableMethod != null)
            {
                Logger.Log("Found SetEnabled method via reflection, attempting to call it");
                enableMethod.Invoke(element, new object[] { true });
                
                if (VerifyEnabled(element))
                {
                    return true;
                }
            }
            
            return false;
        }
    }
}
