using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;
using Moai.Platform.Linux.Menus;
using Moai.Platform.Linux.UI;
using Moai.Platform.Linux.Tools;
using Qyoto;

namespace Moai.Platform.Linux
{
    public partial class LinuxIDE : QMainWindow, IIDE
    {
        public event EventHandler Opened;
        public event EventHandler Closed;
        public event EventHandler ActiveTabChanged;
        public event EventHandler ResizeEnd;

        public LinuxIDE()
        {
            InitializeComponent();
            this.WindowTitle = "Moai IDE (" + Versioning.Version.GetVersionString() + ")";
            this.WindowIcon = LinuxImageList.ConvertToQIcon(Properties.Resources.MoaiIcon);
        }

        /// <summary>
        /// This event is raised when the window is shown.
        /// </summary>
        protected override void ShowEvent(QShowEvent arg1)
        {
            base.ShowEvent(arg1);

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
            this.ShowDock(new Moai.Platform.Linux.Designers.Start.Designer(), ToolPosition.Document);
        }

        #region IIDE Members

        public void Exit()
        {
            this.c_Documents.Clear();
            QApplication.Quit();
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
                case Moai.Platform.UI.ToolPosition.DockLeft:
                    this.AddDockWidget(Qt.DockWidgetArea.LeftDockWidgetArea, tool as QDockWidget);
                    break;
                case Moai.Platform.UI.ToolPosition.DockTop:
                    this.AddDockWidget(Qt.DockWidgetArea.TopDockWidgetArea, tool as QDockWidget);
                    break;
                case Moai.Platform.UI.ToolPosition.DockRight:
                    this.AddDockWidget(Qt.DockWidgetArea.RightDockWidgetArea, tool as QDockWidget);
                    break;
                case Moai.Platform.UI.ToolPosition.DockBottom:
                    this.AddDockWidget(Qt.DockWidgetArea.BottomDockWidgetArea, tool as QDockWidget);
                    break;
                case Moai.Platform.UI.ToolPosition.Document:
                    if (tool is Moai.Designers.Designer)
                    {
                        (tool as Moai.Designers.Designer).SwitchParent(this.c_Documents);
                        this.c_Documents.AddTab(tool as QWidget, (tool as Moai.Designers.Designer).TabText);
                    }
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
