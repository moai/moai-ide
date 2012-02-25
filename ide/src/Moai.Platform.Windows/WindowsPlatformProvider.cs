using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Moai.Platform.Windows
{
    public class WindowsPlatformProvider : IPlatformProvider
    {
        public IPlatform Create()
        {
            return new WindowsPlatform();
        }

        public bool IsCurrentPlatformHandler()
        {
            // Ensures we are not running on Mono (and are running straight Windows).
            Type t = Type.GetType("Mono.Runtime");
            if (t == null && (Environment.OSVersion.Platform == PlatformID.Win32NT || Environment.OSVersion.Platform == PlatformID.Win32Windows))
                return true;
            return false;
        }
    }
}
