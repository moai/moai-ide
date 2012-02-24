using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Drawing;
using MOAI.Tools;

namespace MOAI.Menus.Definitions.Debugging
{
    class Start : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = Properties.Resources.debug;
            this.Text = "Start with Debugging";
            this.Enabled = false;

            // Listen for events.
            Program.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Program.Manager.DebugManager.DebugPause += new EventHandler(DebugManager_OnDebugPause);
            Program.Manager.DebugManager.DebugContinue += new EventHandler(DebugManager_OnDebugContinue);
            Program.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
            Program.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Program.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            Program.Manager.DebugManager.Start(Program.Manager.ActiveProject);
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

        /// <summary>
        /// This event is raised when debugging starts.
        /// </summary>
        private void DebugManager_OnDebugStart(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when debugging pauses.
        /// </summary>
        private void DebugManager_OnDebugPause(object sender, EventArgs e)
        {
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when debugging continues.
        /// </summary>
        private void DebugManager_OnDebugContinue(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when debugging stops.
        /// </summary>
        private void DebugManager_OnDebugStop(object sender, EventArgs e)
        {
            this.Enabled = true;
        }
    }

    class StartWithout : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = Properties.Resources.debug_without;
            this.Text = "Start without Debugging";
            this.Enabled = false;

            // Listen for when debugging starts or stops.
            Program.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Program.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
            Program.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Program.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            Program.Manager.DebugManager.StartWithout(Program.Manager.ActiveProject);
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

        /// <summary>
        /// This event is raised when debugging starts.
        /// </summary>
        private void DebugManager_OnDebugStart(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when debugging stops.
        /// </summary>
        private void DebugManager_OnDebugStop(object sender, EventArgs e)
        {
            this.Enabled = true;
        }
    }

    class Pause : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = Properties.Resources.pause;
            this.Text = "Pause";
            this.Enabled = false;

            // Listen for when debugging starts or stops.
            Program.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Program.Manager.DebugManager.DebugPause += new EventHandler(DebugManager_OnDebugPause);
            Program.Manager.DebugManager.DebugContinue += new EventHandler(DebugManager_OnDebugContinue);
            Program.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
            Program.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Program.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            Program.Manager.DebugManager.Pause();
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
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when debugging starts.
        /// </summary>
        private void DebugManager_OnDebugStart(object sender, EventArgs e)
        {
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when debugging pauses.
        /// </summary>
        private void DebugManager_OnDebugPause(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when debugging continues.
        /// </summary>
        private void DebugManager_OnDebugContinue(object sender, EventArgs e)
        {
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when debugging stops.
        /// </summary>
        private void DebugManager_OnDebugStop(object sender, EventArgs e)
        {
            this.Enabled = false;
        }
    }

    class Stop : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = Properties.Resources.stop;
            this.Text = "Stop";
            this.Enabled = false;

            // Listen for when debugging starts or stops.
            Program.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Program.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
            Program.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Program.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            Program.Manager.DebugManager.Stop();
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
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when debugging starts.
        /// </summary>
        private void DebugManager_OnDebugStart(object sender, EventArgs e)
        {
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when debugging stops.
        /// </summary>
        private void DebugManager_OnDebugStop(object sender, EventArgs e)
        {
            this.Enabled = false;
        }
    }
}
