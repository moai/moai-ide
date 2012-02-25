using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.UI
{
    public interface IToolStripDropDownItem : IToolStripItem
    {
        List<IToolStripDropDownItem> DropDownItems { get; }

        IToolStripItem AddByText(string text);
    }
}
