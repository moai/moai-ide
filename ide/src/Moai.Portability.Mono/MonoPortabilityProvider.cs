using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Portability.Mono
{
    public class MonoPortabilityProvider : IPortabilityProvider
    {
        public bool IsCurrentPlatformHandler()
        {
            Type t = Type.GetType("Mono.Runtime");
            if (t != null)
                return true;
            return false;
        }

        public void Initalize()
        {
            throw new NotImplementedException();
        }

        public IClipboard Clipboard
        {
            get { throw new NotImplementedException(); }
        }
    }
}
