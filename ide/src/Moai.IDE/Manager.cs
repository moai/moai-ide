using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using System.IO;
using System.Diagnostics;
using Moai.Platform;
using Moai.Platform.Collections;
using Moai.Platform.Menus;
using Moai.Platform.Management;

namespace Moai.IDE
{
    public class Manager : IRootManager
    {
        public event EventHandler SolutionLoaded;
        public event EventHandler SolutionUnloaded;
        public event EventHandler IDEOpened;

        /// <summary>
        /// Creates a new main manager.
        /// </summary>
        public Manager()
        {
            // Load settings.
            this.Settings = new Dictionary<string, string>();
            this.Settings["RootPath"] = Environment.CurrentDirectory;
            this.Settings["DefaultProjectArea"] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                                + Path.DirectorySeparatorChar + "Moai Projects";

            // This is a special check that detects whether we're running underneath a debugger, and adjusts the working directory
            // if required.
            if (Debugger.IsAttached || Directory.Exists("Deployment"))
                this.Settings["RootPath"] = Environment.CurrentDirectory + Path.DirectorySeparatorChar + "Deployment";
        }

        /// <summary>
        /// Initializes all of the sub-managers.
        /// </summary>
        public void Initalize()
        {
            this.ActiveSolution = null;
            this.ActiveProject = null;
            this.CacheManager = new Cache.Manager();
            this.DebugManager = new Debug.Manager();
            this.DesignersManager = new Designers.Manager();
            this.ToolsManager = null; // FIXME: new Tools.Manager();
            this.MenuManager = new MenusManager();

            this.IDE = Central.Platform.CreateIDE();
            if (this.IDEOpened != null)
                this.IDEOpened(this, new EventArgs());
        }

        /// <summary>
        /// Loads a solution from file.
        /// </summary>
        /// <param name="path">The path to the solution file.</param>
        public void LoadSolution(string path)
        {
            this.ActiveSolution = new Moai.Platform.Management.Solution();

            // Read the solution XML document.
            Tree t = null;
            using (XmlReader reader = XmlReader.Create(path))
            {
                t = Tree.FromXml(reader);
            }

            // Read all of the projects.
            Node sn = t.GetChildElement("solution");

            foreach (Node pn in sn.GetChildElements("project"))
            {
                this.ActiveSolution.Projects.Add(new Project(Path.Combine(new FileInfo(path).DirectoryName, pn.GetText().Value)));
            }

            // Set the active project to the first project.
            if (this.ActiveSolution.Projects.Count > 0)
                this.ActiveProject = this.ActiveSolution.Projects[0];

            // Now trigger the load event.
            if (this.SolutionLoaded != null)
                this.SolutionLoaded(this, new EventArgs());
        }

        /// <summary>
        /// Unloads the current solution.
        /// </summary>
        public void UnloadSolution()
        {
            this.ActiveSolution = null;

            // Now trigger the load event.
            if (this.SolutionUnloaded != null)
                this.SolutionUnloaded(this, new EventArgs());
        }

        /// <summary>
        /// Starts the Moai IDE.
        /// </summary>
        public void Start()
        {
            /*SplashScreen splash = new SplashScreen();

            // Preload some scintilla objects.
            splash.SetProgress(0, "Loading Scintilla...");
            bool result = this.p_CacheManager.ScintillaCache.InitialCache(5, (progress) =>
                {
                    splash.SetProgress(100 / 5 * progress, "Preloading editors ( " + progress.ToString() + " / 5 )...");
                }
            );
            splash.SetProgress(100, "Starting IDE...");

            // Check to see whether we should actually quit because
            // we failed to load one of our components.
            if (!result)
            {
                this.Stop();
                return false;
            }

            // Now open and display the IDE.*/
            this.IDE.Opened += (sender, e) =>
                {
                    //splash.Hide();
                };
            this.IDE.Closed += (sender, e) =>
                {
                    this.Stop();
                };
            this.IDE.Show();

            // Now run the application loop with a reference to the IDE.
            Central.Platform.RunIDE(this.IDE);
        }

        /// <summary>
        /// Shuts down Roket3D IDE.NET.
        /// </summary>
        public void Stop()
        {
            // FIXME: System.Windows.Forms.Application.Exit();
        }

        #region IRootManager Members

        public Moai.Platform.Management.Solution ActiveSolution
        {
            get;
            private set;
        }

        public Moai.Platform.Management.Project ActiveProject
        {
            get;
            private set;
        }

        public Moai.Platform.Cache.ICacheManager CacheManager
        {
            get;
            private set;
        }

        public Moai.Platform.Debug.IDebugManager DebugManager
        {
            get;
            private set;
        }

        public Moai.Platform.Designers.IDesignerManager DesignersManager
        {
            get;
            private set;
        }

        public Moai.Platform.Menus.MenusManager MenuManager
        {
            get;
            private set;
        }

        public Moai.Platform.Tools.IToolsManager ToolsManager
        {
            get;
            private set;
        }

        public IIDE IDE
        {
            get;
            private set;
        }

        public Dictionary<string, string> Settings
        {
            get;
            private set;
        }

        #endregion
    }
}
