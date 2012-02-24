using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Portability.Windows
{
    public class WindowsClipboard : IClipboard
    {
        public ClipboardContentType Type
        {
            get { throw new NotImplementedException(); }
        }

        public object Contents
        {
            get { throw new NotImplementedException(); }
        }

        public void Set(ClipboardContentType type, object contents)
        {
            throw new NotImplementedException();
        }
    }
}
