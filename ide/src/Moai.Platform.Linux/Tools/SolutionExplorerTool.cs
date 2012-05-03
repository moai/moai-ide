using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;
using Moai.Platform.Management;
using Moai.Platform.Linux.UI;
using Qyoto;

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

        protected override void MousePressEvent(Qyoto.QMouseEvent arg1)
        {
            //
            //this.SetMinimumSize(16777215, 16777215);
        }

        protected override void ShowEvent(Qyoto.QShowEvent arg1)
        {
            //this.Resize(600, this.Height());

            //this.Widget().Resize(250, 16777215);
            //this.SetSizePolicy(QSizePolicy.Policy.Expanding, QSizePolicy.Policy.Expanding);
            //this.Widget().SetSizePolicy(QSizePolicy.Policy.Expanding, QSizePolicy.Policy.Expanding);
            //this.SetGeometry(0, 0, 250, this.Widget().Height());
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
            /*try
            {
                this.c_TreeStore.Clear();
                if (Central.Manager.ActiveSolution != null)
                    this.Wrap(Central.Manager.ActiveSolution);
                this.c_TreeView.ExpandAll();
            }
            catch (Exception e)
            {
            }*/

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

        /*
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
        */

        public override ToolPosition DefaultState
        {
            get
            {
                return ToolPosition.DockRight;
            }
        }
    }
}
