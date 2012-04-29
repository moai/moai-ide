using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Reflection;

namespace Moai.Versioning
{
    public static class Version
    {
        public static string GetVersionString()
        {
            object[] attrs = Assembly.GetEntryAssembly().GetCustomAttributes(true);
            foreach (object o in attrs)
            {
                if (o is JenkinsAttribute)
                    return "Jenkins Build #" + (o as JenkinsAttribute).BuildNumber + Version.GetRevisionAppendingString();
                else if (o is VisualStudio2008Attribute)
                    return "Visual Studio 2008 Build";
                else if (o is MonoAttribute)
                    return "Mono Build";
            }
            return "Unknown Build";
        }

        private static string GetRevisionAppendingString()
        {
            object[] attrs = Assembly.GetEntryAssembly().GetCustomAttributes(true);
            RevisionSourceAttribute rsa = attrs.DefaultIfEmpty(null).FirstOrDefault(value => value is RevisionSourceAttribute) as RevisionSourceAttribute;
            if (rsa == null)
                return "";
            else
                return "; " + rsa.Type + " Revision #" + rsa.Revision;
        }
    }
}
