using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace UnlockWorld.Core
{
    public interface IControlOperator
    {
        bool CanHandle(nint hWnd);
        bool EnableControl();
        bool SetValue(string value);
    }

}
