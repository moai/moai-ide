using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Moai.Portability.Windows
{
    public class WindowsPortabilityProvider : IPortabilityProvider
    {
        public bool IsCurrentPlatformHandler()
        {
            // Ensures the PresentationCore library is available.
            Assembly.Load("PresentationCore");

            // Ensures we are not running on Mono (and are running straight Windows).
            Type t = Type.GetType("Mono.Runtime");
            if (t == null && (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows))
                return true;
            return false;
        }

        public void Initalize()
        {
            this.Clipboard = new WindowsClipboard();
        }

        public IClipboard Clipboard
        {
            get;
            private set;
        }
    }
}
