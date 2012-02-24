using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOAI.Collections;
using System.Xml;
using MOAI.Management;
using MOAI.Tools;
using System.IO;
using System.Diagnostics;

namespace MOAI
{
    public class Manager
    {
        Portability.IPortabilityProvider p_PortabilityProvider;
        Management.Solution p_ActiveSolution;
        Management.Project p_ActiveProject;
        Cache.Manager p_CacheManager;
        Build.Manager p_BuildManager;
        Debug.Manager p_DebugManager;
        Designers.Manager p_DesignersManager;
        Menus.Manager p_MenuManager;
        Tools.Manager p_ToolsManager;
        IDE p_IDEWindow;
        Dictionary<string, string> p_Settings;

        public event EventHandler SolutionLoaded;
        public event EventHandler SolutionUnloaded;
        public event EventHandler IDEOpened;

        public delegate void VoidLambda();

        /// <summary>
        /// Creates a new main manager.
        /// </summary>
        public Manager()
        {
            // Load portability provider.
            this.p_PortabilityProvider = Portability.PortabilityProvider.Detect();

            // Load settings.
            this.p_Settings = new Dictionary<string, string>();
            this.p_Settings["RootPath"] = Environment.CurrentDirectory;
            this.p_Settings["DefaultProjectArea"] = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments)
                                                + "\\MOAI Projects";

            // This is a special check that detects whether we're running underneath a debugger, and adjusts the working directory
            // if required.
            if (Debugger.IsAttached || Directory.Exists("Deployment"))
                this.p_Settings["RootPath"] = Environment.CurrentDirectory + "\\Deployment";
        }

        /// <summary>
        /// Initializes all of the sub-managers.
        /// </summary>
        public void Initalize()
        {
            this.p_ActiveSolution = null;
            this.p_ActiveProject = null;
            this.p_CacheManager = new Cache.Manager(this);
            this.p_BuildManager = new Build.Manager(this);
            this.p_DebugManager = new Debug.Manager(this);
            this.p_DesignersManager = new Designers.Manager(this);
            this.p_ToolsManager = new Tools.Manager(this);
            this.p_MenuManager = new Menus.Manager(this);

            this.p_IDEWindow = new IDE(this);
            if (this.IDEOpened != null)
                this.IDEOpened(this, new EventArgs());
        }

        /// <summary>
        /// Loads a solution from file.
        /// </summary>
        /// <param name="path">The path to the solution file.</param>
        public void LoadSolution(string path)
        {
            this.p_ActiveSolution = new Management.Solution();

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
                this.p_ActiveSolution.Projects.Add(new Project(Path.Combine(new FileInfo(path).DirectoryName, pn.GetText().Value)));
            }

            // Set the active project to the first project.
            if (this.p_ActiveSolution.Projects.Count > 0)
                this.p_ActiveProject = this.p_ActiveSolution.Projects[0];

            // Now trigger the load event.
            if (this.SolutionLoaded != null)
                this.SolutionLoaded(this, new EventArgs());
        }

        /// <summary>
        /// Unloads the current solution.
        /// </summary>
        public void UnloadSolution()
        {
            this.p_ActiveSolution = null;

            // Now trigger the load event.
            if (this.SolutionUnloaded != null)
                this.SolutionUnloaded(this, new EventArgs());
        }

        /// <summary>
        /// The currently opened solution.
        /// </summary>
        public Management.Solution ActiveSolution
        {
            get
            {
                return this.p_ActiveSolution;
            }
        }

        /// <summary>
        /// The start-up project in the solution.  This is the project
        /// that will be started during debugging.
        /// </summary>
        public Management.Project ActiveProject
        {
            get
            {
                return this.p_ActiveProject;
            }
        }

        /// <summary>
        /// The object which manages building the project.
        /// </summary>
        public Build.Manager BuildManager
        {
            get
            {
                return this.p_BuildManager;
            }
        }

        /// <summary>
        /// The object which caches various information about the project.
        /// </summary>
        public Cache.Manager CacheManager
        {
            get
            {
                return this.p_CacheManager;
            }
        }

        /// <summary>
        /// The object which manages debugging the project.
        /// </summary>
        public Debug.Manager DebugManager
        {
            get
            {
                return this.p_DebugManager;
            }
        }

        /// <summary>
        /// The object which manages the designer windows.
        /// </summary>
        public Designers.Manager DesignersManager
        {
            get
            {
                return this.p_DesignersManager;
            }
        }

        /// <summary>
        /// The object which manages the menus and toolbars.
        /// </summary>
        public Menus.Manager MenuManager
        {
            get
            {
                return this.p_MenuManager;
            }
        }

        /// <summary>
        /// The object which manages the tool windows.
        /// </summary>
        public Tools.Manager ToolsManager
        {
            get
            {
                return this.p_ToolsManager;
            }
        }

        /// <summary>
        /// The object which provides portable interfaces.
        /// </summary>
        public Portability.IPortabilityProvider PortabilityProvider
        {
            get
            {
                return this.p_PortabilityProvider;
            }
        }

        /// <summary>
        /// The main IDE window.
        /// </summary>
        public IDE IDEWindow
        {
            get
            {
                return this.p_IDEWindow;
            }
        }

        /// <summary>
        /// A dictionary of string settings.
        /// </summary>
        public Dictionary<string, string> Settings
        {
            get
            {
                return this.p_Settings;
            }
        }

        /// <summary>
        /// Starts the MOAI IDE.
        /// </summary>
        public bool Start()
        {
            SplashScreen splash = new SplashScreen();

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

            // Now open and display the IDE.
            this.p_IDEWindow.Load += (sender, e) =>
                {
                    splash.Hide();
                };
            this.p_IDEWindow.FormClosed += (sender, e) =>
                {
                    this.Stop();
                };
            this.p_IDEWindow.Show();
            return true;
        }

        /// <summary>
        /// Shuts down Roket3D IDE.NET.
        /// </summary>
        public void Stop()
        {
            System.Windows.Forms.Application.Exit();
        }
    }
}
