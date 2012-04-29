using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform;
using log4net;
using log4net.Config;
using System.IO;

namespace Moai
{
    public static class Central
    {
        private static bool m_Initialized = false;
        private static bool m_LogInitialized = false;
        public static IRootManager Manager { get; private set; }
        public static IPlatform Platform { get; private set; }
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(Central));
        
        public static void InitializeLogger()
        {
            if (Central.m_LogInitialized)
                return;
            if (File.Exists("Moai.Platform.Logging.xml"))
            {
                XmlConfigurator.Configure(new FileInfo("Moai.Platform.Logging.xml"));
                m_Log.Debug("Logging system is using XML file for configuration.");
            }
            else
            {
                BasicConfigurator.Configure();
                m_Log.Warn("Unable to locate Moai.Platform.Logging.xml.  Logging system will be set to defaults.");
            }
            Central.m_LogInitialized = true;
        }

        public static void InitializeSystem(IRootManager manager, IPlatform platform)
        {
            if (!Central.m_LogInitialized)
                throw new InvalidOperationException("Moai central management system does not have logging infrastructure initialized before starting main application execution.");
            if (Central.m_Initialized)
                throw new InvalidOperationException("Moai central management system can not be initialized twice.");
            Central.m_Initialized = true;
            Central.Manager = manager;
            Central.Platform = platform;
        }
    }
}
