using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.UI.Segments
{
    public interface IHasInvoke
    {
        object Invoke(Delegate method);
        bool InvokeRequired { get; }
    }
}
