using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Linux
{
    public class LinuxPlatformProvider : IPlatformProvider
    {
        #region IPlatformProvider Members

        public IPlatform Create()
        {
            return new LinuxPlatform();
        }

        public bool IsCurrentPlatformHandler()
        {
            Type t = Type.GetType("Mono.Runtime");
            if (t != null)
                return true;
            return false;
        }

        #endregion
    }
}
