using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Debug
{
    public interface IBreakpoint
    {
        string SourceFile { get; }
        uint SourceLine { get; }
    }
}
