using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Windows.Automation;

namespace UnlockWorld.Core
{
    /// <summary>
    /// UI Automation 控件操作器（支持 WPF、UWP）
    /// </summary>
    public class UIAutomationControlOperator : IControlOperator
    {
        private AutomationElement _element;

        public UIAutomationControlOperator(AutomationElement element)
        {
            _element = element;
        }

        public bool CanHandle(nint hWnd)
        {
            return _element != null;
        }

        public bool EnableControl()
        {
            // 尝试使用 UIA 支持的交互 Pattern 间接启用控件
            try
            {
                if (_element.TryGetCurrentPattern(InvokePattern.Pattern, out var invokeObj))
                {
                    ((InvokePattern)invokeObj).Invoke();
                    return true;
                }

                if (_element.TryGetCurrentPattern(TogglePattern.Pattern, out var toggleObj))
                {
                    var toggle = (TogglePattern)toggleObj;
                    if (toggle.Current.ToggleState != ToggleState.On)
                        toggle.Toggle();
                    return true;
                }

                if (_element.TryGetCurrentPattern(ExpandCollapsePattern.Pattern, out var expandObj))
                {
                    var pattern = (ExpandCollapsePattern)expandObj;
                    if (pattern.Current.ExpandCollapseState == ExpandCollapseState.Collapsed)
                        pattern.Expand();
                    return true;
                }
            }
            catch
            {
                // 控件不支持该操作或触发异常，忽略
            }

            return false;
        }

        public bool SetValue(string value)
        {
            try
            {
                if (_element.TryGetCurrentPattern(ValuePattern.Pattern, out var pattern))
                {
                    ((ValuePattern)pattern).SetValue(value);
                    return true;
                }
            }
            catch
            {
                // 控件可能不支持 ValuePattern
            }

            return false;
        }
    }
}
