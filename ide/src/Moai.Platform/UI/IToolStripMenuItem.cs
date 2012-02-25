using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Moai.Platform.UI
{
    public interface IToolStripMenuItem : IToolStripDropDownItem
    {
        Keys ShortcutKeys { get; set; }
        bool ShowShortcutKeys { get; set; }
        Image Image { get; set; }
        bool Checked { get; set; }
    }
}
