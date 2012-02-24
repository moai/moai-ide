using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;

namespace Moai.Utilities
{
    public static class PathUtils
    {
        public static string Sanitize(string path)
        {
            return path.Replace('\\', Path.DirectorySeparatorChar).Replace('/', Path.DirectorySeparatorChar);
        }
    }
}
