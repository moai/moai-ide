using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.UI.Segments
{
    public interface IHasBasics
    {
        event EventHandler Click;

        bool Enabled { get; set; }
        string Text { get; set; }
        object Tag { get; set; }
        IContextMenuStrip ContextMenuStrip { get; set; }
    }
}
