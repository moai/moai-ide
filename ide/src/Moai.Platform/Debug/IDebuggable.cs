using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Management;

namespace Moai.Platform.Debug
{
    public interface IDebuggable
    {
        void Debug(File file, uint line);
        void EndDebug();
    }
}
