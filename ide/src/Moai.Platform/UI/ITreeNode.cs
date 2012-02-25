using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI.Segments;

namespace Moai.Platform.UI
{
    public interface ITreeNode : IProxable, IHasImageList, IHasBasics
    {
        List<ITreeNode> Nodes { get; }
    }
}
