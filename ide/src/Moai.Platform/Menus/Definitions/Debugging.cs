using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Menus.Definitions.Debugging
{
    class Start : Action
    {
        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = IDE.Resources.Images.debug;
            this.Text = "Start with Debugging";
            this.Enabled = false;

            // Listen for events.
            Central.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Central.Manager.DebugManager.DebugPause += new EventHandler(DebugManager_OnDebugPause);
            Central.Manager.DebugManager.DebugContinue += new EventHandler(DebugManager_OnDebugContinue);
            Central.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            Central.Manager.DebugManager.Start(Central.Manager.ActiveProject);
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
            this.ItemIcon = IDE.Resources.Images.debug_without;
            this.Text = "Start without Debugging";
            this.Enabled = false;

            // Listen for when debugging starts or stops.
            Central.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Central.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            Central.Manager.DebugManager.StartWithout(Central.Manager.ActiveProject);
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
            this.ItemIcon = IDE.Resources.Images.pause;
            this.Text = "Pause";
            this.Enabled = false;

            // Listen for when debugging starts or stops.
            Central.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Central.Manager.DebugManager.DebugPause += new EventHandler(DebugManager_OnDebugPause);
            Central.Manager.DebugManager.DebugContinue += new EventHandler(DebugManager_OnDebugContinue);
            Central.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            Central.Manager.DebugManager.Pause();
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
            this.ItemIcon = IDE.Resources.Images.stop;
            this.Text = "Stop";
            this.Enabled = false;

            // Listen for when debugging starts or stops.
            Central.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Central.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            Central.Manager.DebugManager.Stop();
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
