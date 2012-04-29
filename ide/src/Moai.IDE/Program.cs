using System;
using System.Collections.Generic;
using System.Linq;
using Moai.Platform;
using Moai.IDE;

namespace Moai
{
    public delegate void E();

    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Central.InitializeLogger();
            Central.InitializeSystem(new Manager(), PlatformDetection.Detect());
            Central.Manager.Initalize();
            Central.Manager.Start();
        }
    }
}
