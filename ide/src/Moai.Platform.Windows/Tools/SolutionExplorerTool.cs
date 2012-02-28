using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DockPanelSuite;
using Moai.Platform.UI;
using Moai.Platform.Management;
using Moai.Platform.Windows.UI;

namespace Moai.Platform.Windows.Tools
{
    public partial class SolutionExplorerTool : Tool
    {
        private Tools.Manager m_Manager = null;

        public SolutionExplorerTool(Tools.Manager manager)
        {
            InitializeComponent();
            this.m_Manager = manager;

            // Set the image list.
            this.c_SolutionTree.ImageList = Associations.ImageList.ConvertTo<ImageList>();

            // Fill the solution explorer with the tree nodes.
            this.c_SolutionTree.Nodes.Clear();
            this.c_SolutionTree.ExpandAll();
        }

        public override void OnSolutionLoaded()
        {
            this.ReloadTree();
        }

        public override void OnSolutionUnloaded()
        {
            this.ReloadTree();
        }

        public void ReloadTree()
        {
            // Search the tree node for events to disconnect from.
            if (this.c_SolutionTree.Nodes.Count > 0)
            {
                foreach (TreeNode tn in this.c_SolutionTree.Nodes[0].Nodes)
                {
                    if (tn.Tag is Project)
                    {
                        (tn.Tag as Project).FileAdded -= new EventHandler(OnReloadRequired);
                        (tn.Tag as Project).FileRemoved -= new EventHandler(OnReloadRequired);
                    }
                }
            }

            // Fill the solution explorer with the tree nodes.
            c_SolutionTree.Nodes.Clear();
            if (Central.Manager.ActiveSolution != null)
                c_SolutionTree.Nodes.Add(this.Wrap(Central.Manager.ActiveSolution));
            c_SolutionTree.ExpandAll();

            // Search the tree node for events to listen to.
            if (this.c_SolutionTree.Nodes.Count > 0)
            {
                foreach (TreeNode tn in this.c_SolutionTree.Nodes[0].Nodes)
                {
                    if (tn.Tag is Project)
                    {
                        (tn.Tag as Project).FileAdded += new EventHandler(OnReloadRequired);
                        (tn.Tag as Project).FileRemoved += new EventHandler(OnReloadRequired);
                    }
                }
            }
        }

        #region TreeNode Wrapping

        private TreeNode Wrap(Solution solution)
        {
            TreeNode tn = new SolutionTreeNode(solution);
            foreach (Project p in solution.Projects)
                tn.Nodes.Add(this.Wrap(p));
            return tn;
        }

        private TreeNode Wrap(Project project)
        {
            TreeNode tn = new ProjectTreeNode(project);
            foreach (File f in project.Files)
            {
                if (f is Folder)
                    tn.Nodes.Add(this.Wrap(f as Folder));
                else
                    tn.Nodes.Add(new FileTreeNode(f));
            }
            return tn;
        }

        private TreeNode Wrap(Folder folder)
        {
            TreeNode tn = new FileTreeNode(folder);
            foreach (File f in folder.Files)
            {
                if (f is Folder)
                    tn.Nodes.Add(this.Wrap(f as Folder));
                else
                    tn.Nodes.Add(new FileTreeNode(f));
            }
            return tn;
        }

        #endregion

        void OnReloadRequired(object sender, EventArgs e)
        {
            if (this.c_SolutionTree.InvokeRequired)
                this.c_SolutionTree.Invoke(new Action(() => { this.ReloadTree(); }));
            else
                this.ReloadTree();
        }

        public override ToolPosition DefaultState
        {
            get
            {
                return ToolPosition.DockRight;
            }
        }

        private void ToolSolutionExplorer_Load(object sender, EventArgs e)
        {
            // Fully expand the tree.
            c_SolutionTree.ExpandAll();
        }

        /// <summary>
        /// This event is raised when the user has double clicked an item in
        /// the solution tree.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_SolutionTree_DoubleClick(object sender, EventArgs e)
        {
            if (this.c_SolutionTree.SelectedNode.Tag is File)
                Central.Manager.DesignersManager.OpenDesigner(this.c_SolutionTree.SelectedNode.Tag as File);
        }

        /// <summary>
        /// This event is raised when the user selects a node.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_SolutionTree_AfterSelect(object sender, System.Windows.Forms.TreeViewEventArgs e)
        {
            if (e.Node.Tag is Solution || 
                e.Node.Tag is Project ||
                e.Node.Tag is Folder ||
                e.Node.Tag is File)
                Central.Manager.CacheManager.Context.Object = e.Node.Tag;
        }

        /// <summary>
        /// This event is raised when the user releases the mouse over the control.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_SolutionTree_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                c_SolutionTree.SelectedNode = c_SolutionTree.GetNodeAt(e.X, e.Y);
            }
        }

        /// <summary>
        /// This event is raised when the tree loses focus.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_SolutionTree_LostFocus(object sender, System.EventArgs e)
        {
            Central.Manager.CacheManager.Context.Object = null;
        }

        /// <summary>
        /// This event is raised when the tree gains focus.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_SolutionTree_GotFocus(object sender, System.EventArgs e)
        {
            // Simulate a tree node select when the tree gains focus if
            // there was a tree node selected.
            if (this.c_SolutionTree.SelectedNode != null)
                this.c_SolutionTree_AfterSelect(this, new TreeViewEventArgs(this.c_SolutionTree.SelectedNode, TreeViewAction.Unknown));
        }
    }
}
