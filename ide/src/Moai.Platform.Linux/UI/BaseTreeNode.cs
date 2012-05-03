using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qyoto;

namespace Moai.Platform.Linux.UI
{
    public class BaseTreeNode
    {
        public string Text { get; protected set; }
        public string ImageKey { get; protected set; }
        public QMenu ContextMenu { get; protected set; }
    }
}
