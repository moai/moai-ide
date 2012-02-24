using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Reflection;

namespace MOAI.Portability
{
    public static class PortabilityProvider
    {
        public static IPortabilityProvider Detect()
        {
            List<Type> types = new List<Type>();
            List<IPortabilityProvider> providers = new List<IPortabilityProvider>();

            // Attempt to initially load all MOAI.Portability assemblies.
            foreach (FileInfo fi in new DirectoryInfo(".").GetFiles("MOAI.Portability.*.dll"))
            {
                try
                {
                    Assembly a = Assembly.LoadFrom(fi.FullName);
                    foreach (Type t in a.GetTypes())
                        if (typeof(IPortabilityProvider).IsAssignableFrom(t))
                            types.Add(t);
                }
                catch (Exception) { }
            }

            // Create instances of all of the types.
            foreach (Type t in types)
            {
                try
                {
                    providers.Add((IPortabilityProvider) t.GetConstructor(Type.EmptyTypes).Invoke(null));
                }
                catch (Exception) { }
            }

            // Ask all of the providers whether they support this platform and return the
            // first one that does.
            foreach (IPortabilityProvider ip in providers)
            {
                try
                {
                    if (ip.IsCurrentPlatformHandler())
                        return ip;
                }
                catch (Exception) { }
            }

            return null;
        }
    }
}
