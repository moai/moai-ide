using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;
using Gtk;
using Moai.Platform.Management;
using Moai.Platform.Linux.UI;

namespace Moai.Platform.Linux.Tools
{
    public partial class SolutionExplorerTool : Tool
    {
        public SolutionExplorerTool()
        {
            InitializeComponent();
            this.Title = "Solution Explorer";
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
            /*if (this.c_SolutionTree.Nodes.Count > 0)
            {
                foreach (TreeNode tn in this.c_SolutionTree.Nodes[0].Nodes)
                {
                    if (tn.Tag is Project)
                    {
                        (tn.Tag as Project).FileAdded -= new EventHandler(OnReloadRequired);
                        (tn.Tag as Project).FileRemoved -= new EventHandler(OnReloadRequired);
                    }
                }
            }*/

            // Fill the solution explorer with the tree nodes.
            try
            {
                this.c_TreeStore.Clear();
                if (Central.Manager.ActiveSolution != null)
                    this.Wrap(Central.Manager.ActiveSolution);
                this.c_TreeView.ExpandAll();
            }
            catch (Exception e)
            {
            }

            // Search the tree node for events to listen to.
            /*if (this.c_SolutionTree.Nodes.Count > 0)
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
             */
        }

        private TreeIter Wrap(Solution solution)
        {
            TreeIter ti = this.c_TreeStore.AppendValues(new SolutionTreeNode(solution));
            foreach (Project p in solution.Projects)
                this.Wrap(ti, p);
            return ti;
        }

        private TreeIter Wrap(TreeIter parent, Project project)
        {
            TreeIter ti = this.c_TreeStore.AppendValues(parent, new ProjectTreeNode(project));
            foreach (File f in project.Files)
            {
                if (f is Folder)
                    this.Wrap(ti, f as Folder);
                else
                    this.c_TreeStore.AppendValues(ti, new FileTreeNode(f));
            }
            return ti;
        }

        private TreeIter Wrap(TreeIter parent, Folder folder)
        {
            TreeIter ti = this.c_TreeStore.AppendValues(parent, new FileTreeNode(folder));
            foreach (File f in folder.Files)
            {
                if (f is Folder)
                    this.Wrap(ti, f as Folder);
                else
                    this.c_TreeStore.AppendValues(ti, new FileTreeNode(f));
            }
            return ti;
        }

        private void t_DataFunc(TreeViewColumn column, CellRenderer cell, TreeModel model, TreeIter iter)
        {
            BaseTreeNode node = model.GetValue(iter, 0) as BaseTreeNode;
            (cell as CellRendererText).Text = node.Text;
        }

        public override ToolPosition DefaultState
        {
            get
            {
                return ToolPosition.DockRight;
            }
        }
    }
}
