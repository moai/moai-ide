using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using MOAI.Tools;

namespace MOAI.Menus.Definitions.Solution
{
    class New : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = Properties.Resources.solution_create;
            this.Text = "New Solution";
            this.Enabled = true;

            // Listen for events.
            Program.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Program.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            NewSolutionForm nsf = new NewSolutionForm();
            if (nsf.ShowDialog() == DialogResult.OK)
            {
                // Request that the template create itself with the data provided.
                Management.Solution s = nsf.Result.Template.Create(nsf.Result);
                Program.Manager.LoadSolution(s.SolutionInfo.FullName);
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

    class Open : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = Properties.Resources.solution_open;
            this.Text = "Open Solution";
            this.Enabled = true;

            // Listen for events.
            Program.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Program.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            OpenFileDialog ofd = new OpenFileDialog();
            ofd.CheckFileExists = true;
            ofd.CheckPathExists = true;
            ofd.RestoreDirectory = true;
            ofd.Filter = "MOAI Solutions|*.msln|MOAI Projects|*.mproj";
            if (ofd.ShowDialog() == DialogResult.OK)
            {
                String filename = ofd.FileName;
                Program.Manager.LoadSolution(filename);
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

    class Close : Action
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
            Program.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Program.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            // TODO: Implement asking whether to save changes here.
            Program.Manager.UnloadSolution();
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

    class Build : Action
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
            Compilation.ProjectBuilder pb = new Compilation.ProjectBuilder(Program.MainWindow.CurrentSolution.Projects[0], Program.MainWindow.BuildOutput);
            pb.Build(Roket3D.Compilation.BuildMode.DEBUG);
            */
        }
    }

    class Rebuild : Action
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

    class Clean : Action
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

    class SetStartupProjects : Action
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
}
