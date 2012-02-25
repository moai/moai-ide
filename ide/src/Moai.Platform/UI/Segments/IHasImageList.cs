using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.UI.Segments
{
    public interface IHasImageList
    {
        int ImageIndex { get; set; }
        string ImageKey { get; set; }
        int SelectedImageIndex { get; set; }
        string SelectedImageKey { get; set; }
        int StateImageIndex { get; set; }
        string StateImageKey { get; set; }
    }
}
