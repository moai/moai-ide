using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using Moai.Platform.Linux.Menus;
using Moai.Platform.Linux.UI;
using Moai.Platform.Linux.Tools;

namespace Moai.Platform.Linux
{
    public partial class LinuxIDE : Window, IIDE
    {
        public event EventHandler Opened;
        public event EventHandler Closed;
        public event EventHandler ActiveTabChanged;
        public event EventHandler ResizeEnd;

        public LinuxIDE() : base("Moai IDE")
        {
            InitializeComponent();
            this.Title += " (" + Versioning.Version.GetVersionString() + ")";
            this.Icon = LinuxImageList.ConvertToPixbuf(Properties.Resources.MoaiIcon);
        }

        /// <summary>
        /// This event is raised when the window is first shown.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        void IDE_Shown(object sender, EventArgs e)
        {
            // Load the main menu.
            this.SetMainMenu(ActionWrapper.GetMainMenu(Central.Manager.MenuManager.MainMenu));

            // Load the tool bar.
            this.SetToolBar(ActionWrapper.GetToolBar(Central.Manager.MenuManager.ToolBar));

            // Set up the workspace.
            Central.Manager.ToolsManager = new Moai.Platform.Linux.Tools.Manager();
            //Central.Manager.ToolsManager.Show(typeof(Moai.Platform.Linux.Tools.ErrorListTool));
            //Central.Manager.ToolsManager.Show(typeof(Moai.Platform.Linux.Tools.ImmediateWindowTool));
            Central.Manager.ToolsManager.Show(typeof(Moai.Platform.Linux.Tools.SolutionExplorerTool));

            // Show the start page.
            //this.ShowDock(new Moai.Platform.Windows.Designers.Start.Designer(null), ToolPosition.Document);
        }

        #region IIDE Members

        public void Exit()
        {
            Application.Quit();
        }

        public Moai.Platform.UI.ITab ActiveTab
        {
            get { return null; }
        }

        public bool IsDisposed
        {
            get { return this.IsDisposed; }
        }

        public void ShowDock(Moai.Platform.UI.ITool tool, Moai.Platform.UI.ToolPosition position)
        {
            switch (position)
            {
                case Moai.Platform.UI.ToolPosition.DockRight:
                    this.c_RightTools.AppendPage(tool as Widget, new AccelLabel((tool as Tool).Title));
                    break;
                case Moai.Platform.UI.ToolPosition.DockBottom:
                    this.c_BottomTools.AppendPage(tool as Widget, new AccelLabel((tool as Tool).Title));
                    break;
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion

        #region IHasInvoke Members

        public object Invoke(Delegate method)
        {
            return this.Invoke(method);
        }

        public bool InvokeRequired
        {
            get { return this.InvokeRequired; }
        }

        #endregion
    }
}
