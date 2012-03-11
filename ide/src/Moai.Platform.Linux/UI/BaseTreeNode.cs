using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace Moai.Platform.Linux.UI
{
    public class BaseTreeNode
    {
        public string Text { get; protected set; }
        public string ImageKey { get; protected set; }
        public Menu ContextMenu { get; protected set; }
    }
}
