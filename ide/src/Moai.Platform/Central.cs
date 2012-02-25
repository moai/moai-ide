using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform;

namespace Moai
{
    public static class Central
    {
        private static bool m_Initialized = false;
        public static IRootManager Manager { get; private set; }
        public static IPlatform Platform { get; private set; }

        public static void Initialize(IRootManager manager, IPlatform platform)
        {
            if (Central.m_Initialized)
                throw new InvalidOperationException("Moai central management system can not be initialized twice.");
            Central.m_Initialized = true;
            Central.Manager = manager;
            Central.Platform = platform;
        }
    }
}
