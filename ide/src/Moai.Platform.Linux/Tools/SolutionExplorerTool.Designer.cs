using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using Moai.Platform.Linux.UI;

namespace Moai.Platform.Linux.Tools
{
    public partial class SolutionExplorerTool
    {
        /// <summary>
        /// Required method for initializing the SolutionExplorerTool structure.
        /// </summary>
        private void InitializeComponent()
        {
            this.c_TreeView = new TreeView();
            this.c_TreeViewColumn = new TreeViewColumn();
            this.c_TreeViewCellRenderer = new CellRendererText();
            this.c_TreeStore = new TreeStore(typeof(BaseTreeNode));
            //
            // c_TreeViewColumn
            //
            this.c_TreeViewColumn.Title = null;
            this.c_TreeViewColumn.Reorderable = false;
            this.c_TreeViewColumn.PackStart(this.c_TreeViewCellRenderer, true);
            this.c_TreeViewColumn.SetCellDataFunc(this.c_TreeViewCellRenderer, this.t_DataFunc);
            //
            // c_TreeView
            //
            this.c_TreeView.SetSizeRequest(250, 100);
            this.c_TreeView.AppendColumn(this.c_TreeViewColumn);
            this.c_TreeView.Model = this.c_TreeStore;
            //
            // SolutionExplorerTool
            //
            this.Add(this.c_TreeView);
            this.ShowAll();
        }

        TreeView c_TreeView = null;
        TreeStore c_TreeStore = null;
        TreeViewColumn c_TreeViewColumn = null;
        CellRendererText c_TreeViewCellRenderer = null;
    }
}
