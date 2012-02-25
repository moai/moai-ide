using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;

namespace Moai.Platform.Tools
{
    public interface IOutputTool : IControl
    {
        void ClearLog();

        void AddLogEntry(string p);
    }
}
