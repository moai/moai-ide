using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI.Segments;

namespace Moai.Platform.UI
{
    public interface IToolStripItem : IComponent, IHasBasics
    {
        IToolStrip Owner { get; set; }
        IToolStripItem OwnerItem { get; set; }
        List<object> Items { get; }
    }
}
