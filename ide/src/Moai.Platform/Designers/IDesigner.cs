using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;
using Moai.Platform.UI.Segments;
using Moai.Platform.Management;

namespace Moai.Platform.Designers
{
    public interface IDesigner : ITab, ITool, IComponent, IHasInvoke
    {
        event EventHandler Opened;
        event EventHandler Closed;

        File File { get; }
    }
}
