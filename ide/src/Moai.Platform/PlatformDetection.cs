using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;
using log4net;

namespace Moai.Platform
{
    public static class PlatformDetection
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(PlatformDetection));

        public static IPlatform Detect()
        {
            List<Type> types = new List<Type>();
            List<IPlatformProvider> providers = new List<IPlatformProvider>();
            
            // Get the directory of this assembly.
            string dir = new FileInfo(Assembly.GetExecutingAssembly().Location).Directory.FullName;
            m_Log.Info("Loading platform assemblies from " + dir + ".");

            // Attempt to initially load all Moai.Platform assemblies.
            foreach (FileInfo fi in new DirectoryInfo(dir).GetFiles("Moai.Platform.*.dll"))
            {
                m_Log.Debug("Attempting to load " + fi.Name + ".");
                try
                {
                    Assembly a = Assembly.LoadFrom(fi.FullName);
                    foreach (Type t in a.GetTypes())
                        if (typeof(IPlatformProvider).IsAssignableFrom(t))
                        {
                            m_Log.Debug("Added platform type " + t.Name + ".");
                            types.Add(t);
                        }
                }
                catch (Exception e)
                {
                    m_Log.Debug("Exception occurred while loading types from " + fi.Name + ".", e);
                }
            }

            // Create instances of all of the types.
            foreach (Type t in types)
            {
                m_Log.Debug("Attempting to construct provider " + t.Name + ".");
                try
                {
                    providers.Add((IPlatformProvider)t.GetConstructor(Type.EmptyTypes).Invoke(null));
                }
                catch (Exception e)
                {
                    m_Log.Debug("Exception occurred while constructing provider type " + t.Name + ".", e);
                }
            }

            // Ask all of the providers whether they support this platform and return the
            // first one that does.
            foreach (IPlatformProvider ip in providers)
            {
                m_Log.Debug("Attempting to construct platform provided by " + ip.GetType().Name + ".");
                try
                {
                    if (ip.IsCurrentPlatformHandler())
                        return ip.Create();
                }
                catch (Exception e)
                {
                    m_Log.Debug("Exception occurred while constructing platform provided by " + ip.GetType().Name + ".", e);
                }
            }

            m_Log.Error("Unable to locate a suitable platform provider.");
            throw new ApplicationException("No suitable platform provider found.");
        }
    }
}
