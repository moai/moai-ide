using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DockPanelSuite;
using MOAI.Properties;
using MOAI.Menus.Definitions.Project;
using MOAI.Management;

namespace MOAI.Tools
{
    public partial class SolutionExplorerTool : Tool
    {
        private Tools.Manager m_Manager = null;

        public SolutionExplorerTool(Tools.Manager manager)
        {
            InitializeComponent();
            this.m_Manager = manager;

            // Set the image list.
            this.c_SolutionTree.ImageList = Associations.ImageList;

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
            if (this.m_Manager.Parent.ActiveSolution != null)
                c_SolutionTree.Nodes.Add(this.m_Manager.Parent.ActiveSolution.ToTreeNode());
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

        void OnReloadRequired(object sender, EventArgs e)
        {
            this.ReloadTree();
        }

        public override DockState DefaultState
        {
            get
            {
                return DockState.DockRight;
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
            if (this.c_SolutionTree.SelectedNode is File)
                this.m_Manager.Parent.DesignersManager.OpenDesigner(this.c_SolutionTree.SelectedNode as File);
        }

        private void c_SolutionTree_MouseUp(object sender, MouseEventArgs e)
        {
            if(e.Button == MouseButtons.Right)
            {
                // Select the clicked node
                c_SolutionTree.SelectedNode = c_SolutionTree.GetNodeAt(e.X, e.Y);
            }
        }
    }
}
