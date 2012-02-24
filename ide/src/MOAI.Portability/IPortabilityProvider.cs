using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Portability
{
    public interface IPortabilityProvider
    {
        bool IsCurrentPlatformHandler();
        void Initalize();

        IClipboard Clipboard { get; }
    }
}
