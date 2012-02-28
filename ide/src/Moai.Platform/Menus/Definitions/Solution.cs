using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Moai.Platform.UI;
using Moai.Platform.Templates.Solutions;

namespace Moai.Platform.Menus.Definitions.Solution
{
    public class New : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = IDE.Resources.Images.solution_create;
            this.Text = "New Solution";
            this.Enabled = true;

            // Listen for events.
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            SolutionCreationData result = Central.Platform.UI.PickNewSolution();
            if (result != null)
            {
                Management.Solution s = result.Template.Create(result);
                Central.Manager.LoadSolution(s.SolutionInfo.FullName);
            }
        }

        /// <summary>
        /// This event is raised when a solution is loaded (opened).
        /// </summary>
        private void Manager_OnSolutionLoaded(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when a solution is unloaded (closed).
        /// </summary>
        private void Manager_OnSolutionUnloaded(object sender, EventArgs e)
        {
            this.Enabled = true;
        }
    }

    public class Open : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = IDE.Resources.Images.solution_open;
            this.Text = "Open Solution";
            this.Enabled = true;

            // Listen for events.
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            string file = Central.Platform.UI.PickExistingFile(new PickingData
            {
                CheckFileExists = true,
                CheckPathExists = true,
                RestoreDirectory = true,
                Filter = "Moai Solutions|*.msln|MOAI Projects|*.mproj"
            });
            if (file != null)
                Central.Manager.LoadSolution(file);
        }

        /// <summary>
        /// This event is raised when a solution is loaded (opened).
        /// </summary>
        private void Manager_OnSolutionLoaded(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when a solution is unloaded (closed).
        /// </summary>
        private void Manager_OnSolutionUnloaded(object sender, EventArgs e)
        {
            this.Enabled = true;
        }
    }

    public class Close : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Close Solution";
            this.Enabled = false;

            // Listen for events.
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            // TODO: Implement asking whether to save changes here.
            Central.Manager.UnloadSolution();
        }

        /// <summary>
        /// This event is raised when a solution is loaded (opened).
        /// </summary>
        private void Manager_OnSolutionLoaded(object sender, EventArgs e)
        {
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when a solution is unloaded (closed).
        /// </summary>
        private void Manager_OnSolutionUnloaded(object sender, EventArgs e)
        {
            this.Enabled = false;
        }
    }

    public class Build : Action
    {
        public Build() : base() { }
        public Build(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Build Solution";
            this.Enabled = false;
        }

        public override void OnActivate()
        {
            /*
            Compilation.ProjectBuilder pb = new Compilation.ProjectBuilder(Central.MainWindow.CurrentSolution.Projects[0], Central.MainWindow.BuildOutput);
            pb.Build(Roket3D.Compilation.BuildMode.DEBUG);
            */
        }
    }

    public class Rebuild : Action
    {
        public Rebuild() : base() { }
        public Rebuild(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Rebuild Solution";
            this.Enabled = false;
        }
    }

    public class Clean : Action
    {
        public Clean() : base() { }
        public Clean(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Clean Solution";
            this.Enabled = false;
        }
    }

    public class SetStartupProjects : Action
    {
        public SetStartupProjects() : base() { }
        public SetStartupProjects(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Set Startup Projects...";
            this.Enabled = false;
        }
    }

    public class Publish : Action
    {
        public Publish() : base() { }
        public Publish(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Publish...";
            this.Enabled = false;
        }
    }
}
