using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DockPanelSuite;
using System.IO;
using Moai.Designers;
using Moai.Platform;
using Moai.Platform.UI;
using Moai.Platform.Windows.Menus;

namespace Moai
{
    public partial class WindowsIDE : Form, IIDE
    {
        /// <summary>
        /// Creates a new main IDE.
        /// </summary>
        public WindowsIDE()
        {
            InitializeComponent();
            this.Text += " (" + Versioning.Version.GetVersionString() + ")";

            // Listen for the creation of new designers.
            Central.Manager.DesignersManager.DesignerCreated += (sender, e) =>
                {
                    this.ShowDock(e.Designer as DockContent, DockState.Document);
                    this.OnActiveTabChanged();
                };
            Central.Manager.DesignersManager.DesignerRefocused += (sender, e) =>
                {
                    (e.Designer as DockContent).Activate();
                    this.OnActiveTabChanged();
                };
        }

        /// <summary>
        /// This function allows DockContents to place themselves within the
        /// DockPanel of the main IDE.
        /// </summary>
        /// <param name="dc">The DockContent to place in the IDE.</param>
        /// <param name="state">Where and how the DockContent should be placed.</param>
        public void ShowDock(DockContent dc, DockState state)
        {
            dc.Show(this.c_DockWorkspace, state);
        }

        /// <summary>
        /// This event is raised when the form is first shown.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void IDE_Load(object sender, EventArgs e)
        {
            // Load the main menu.
            this.MainMenuStrip = ActionWrapper.GetMainMenu(Central.Manager.MenuManager.MainMenu);
            this.Controls.Add(this.MainMenuStrip);

            // Load the tool bar.
            this.c_ToolStripContainer.TopToolStripPanel.Controls.Add(ActionWrapper.GetToolBar(Central.Manager.MenuManager.ToolBar));

            // Set up the workspace.
            Central.Manager.ToolsManager = new Moai.Platform.Windows.Tools.Manager();
            Central.Manager.ToolsManager.Show(typeof(Moai.Platform.Windows.Tools.ErrorListTool));
            Central.Manager.ToolsManager.Show(typeof(Moai.Platform.Windows.Tools.ImmediateWindowTool));
            Central.Manager.ToolsManager.Show(typeof(Moai.Platform.Windows.Tools.SolutionExplorerTool));

            // Show the start page.
            this.ShowDock(new Moai.Platform.Windows.Designers.Start.Designer(null), ToolPosition.Document);
        }

        /// <summary>
        /// This event is raised when the IDE is maximized or restored.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void IDE_StyleChanged(object sender, EventArgs e)
        {
            if (this.WindowState == FormWindowState.Maximized ||
                this.WindowState == FormWindowState.Normal)
            {
                this.OnResizeEnd(e);
            }
        }

        /// <summary>
        /// This event is fired when the active tab changes.
        /// </summary>
        /// <param name="sender">Sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_DockWorkspace_ActiveTabChanged(object sender, EventArgs e)
        {
            this.OnActiveTabChanged();
        }

        /// <summary>
        /// This event is raised when the active tab changes.  DO NOT USE!  Use
        /// the DesignerChanged event in the Designers manager instead as this event
        /// will not fire when a new tab is created!
        /// </summary>
        protected void OnActiveTabChanged()
        {
            if (this.ActiveTabChanged != null)
                this.ActiveTabChanged(this, new EventArgs());
        }

        #region IIDE Members

        public event EventHandler ActiveTabChanged;

        public event EventHandler Opened
        {
            add
            {
                this.Shown += value;
            }
            remove
            {
                this.Shown -= value;
            }
        }

        /// <summary>
        /// The currently active tab.
        /// </summary>
        public ITab ActiveTab
        {
            get
            {
                return this.c_DockWorkspace.ActiveTab as ITab;
            }
        }

        public void ShowDock(ITool tool, ToolPosition position)
        {
            switch (position)
            {
                case ToolPosition.Document:
                    this.ShowDock(tool as DockContent, DockState.Document);
                    break;
                case ToolPosition.DockBottom:
                    this.ShowDock(tool as DockContent, DockState.DockBottom);
                    break;
                case ToolPosition.DockRight:
                    this.ShowDock(tool as DockContent, DockState.DockRight);
                    break;
                case ToolPosition.DockTop:
                    this.ShowDock(tool as DockContent, DockState.DockTop);
                    break;
                case ToolPosition.DockLeft:
                    this.ShowDock(tool as DockContent, DockState.DockLeft);
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        public void Exit()
        {
            Application.Exit();
        }

        #endregion
    }
}
