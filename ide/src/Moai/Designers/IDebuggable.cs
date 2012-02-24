using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Management;

namespace Moai.Designers
{
    public interface IDebuggable
    {
        void Debug(File file, uint line);
        void EndDebug();
    }
}
