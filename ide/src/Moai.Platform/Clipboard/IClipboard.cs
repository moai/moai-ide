using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform
{
    public interface IClipboard
    {
        event EventHandler ContentsChanged;

        ClipboardContentType Type { get; }
        object Contents { get; }

        void Cut(ClipboardContentType type, object contents);
        void Copy(ClipboardContentType type, object contents);
        void Clear();
    }
}
