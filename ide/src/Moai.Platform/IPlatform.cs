using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;
using Moai.Platform.Designers;

namespace Moai.Platform
{
    public interface IPlatform
    {
        IUIProvider UI { get; }
        IClipboard Clipboard { get; }

        IIDE CreateIDE();
        void RunIDE(IIDE ide);

        Type GetDesignerTypeImplementing(Type type);
    }
}
