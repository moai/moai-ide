using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Windows
{
    public class WindowsClipboard : IClipboard
    {
        public event EventHandler ContentsChanged;

        public ClipboardContentType Type
        {
            get { throw new NotImplementedException(); }
        }

        public object Contents
        {
            get { throw new NotImplementedException(); }
        }

        public void Cut(ClipboardContentType type, object contents)
        {
            throw new NotImplementedException();
        }

        public void Copy(ClipboardContentType type, object contents)
        {
            throw new NotImplementedException();
        }

        public void Clear()
        {
            throw new NotImplementedException();
        }
    }
}
