using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnlockWorld.Core
{
    /// <summary>
    /// 模拟操作 + OCR 兜底方案
    /// </summary>
    public class FallbackControlOperator : IControlOperator
    {
        private nint _hWnd;

        public FallbackControlOperator(nint hWnd)
        {
            _hWnd = hWnd;
        }

        public bool CanHandle(nint hWnd)
        {
            return true; // 始终可用的兜底方案
        }

        public bool EnableControl()
        {
            // 未来可集成模拟鼠标点击等方式
            Console.WriteLine("Fallback: 无法直接启用控件，考虑模拟点击或发送消息");
            return false;
        }

        public bool SetValue(string value)
        {
            // 未来可集成 SendInput 或 OCR + 输入框定位
            Console.WriteLine("Fallback: 无法直接设置值，考虑模拟输入");
            return false;
        }
    }
}
