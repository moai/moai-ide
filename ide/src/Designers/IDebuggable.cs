using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOAI.Management;

namespace MOAI.Designers
{
    public interface IDebuggable
    {
        void Debug(File file, uint line);
        void EndDebug();
    }
}
