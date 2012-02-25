using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace Moai.Platform
{
    public static class PlatformDetection
    {
        public static IPlatform Detect()
        {
            List<Type> types = new List<Type>();
            List<IPlatformProvider> providers = new List<IPlatformProvider>();

            // Attempt to initially load all MOAI.Portability assemblies.
            foreach (FileInfo fi in new DirectoryInfo(".").GetFiles("MOAI.Platform.*.dll"))
            {
                try
                {
                    Assembly a = Assembly.LoadFrom(fi.FullName);
                    foreach (Type t in a.GetTypes())
                        if (typeof(IPlatformProvider).IsAssignableFrom(t))
                            types.Add(t);
                }
                catch (Exception) { }
            }

            // Create instances of all of the types.
            foreach (Type t in types)
            {
                try
                {
                    providers.Add((IPlatformProvider)t.GetConstructor(Type.EmptyTypes).Invoke(null));
                }
                catch (Exception) { }
            }

            // Ask all of the providers whether they support this platform and return the
            // first one that does.
            foreach (IPlatformProvider ip in providers)
            {
                try
                {
                    if (ip.IsCurrentPlatformHandler())
                        return ip.Create();
                }
                catch (Exception) { }
            }

            return null;
        }
    }
}
