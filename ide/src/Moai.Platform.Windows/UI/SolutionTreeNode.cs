using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moai.Platform.Management;
using Moai.Platform.Windows.Menus;

namespace Moai.Platform.Windows.UI
{
    public class SolutionTreeNode : TreeNode
    {
        public SolutionTreeNode(Solution solution)
        {
            this.Solution = solution;
            this.Solution = solution;
            this.Solution.SyncDataChanged += (sender, e) =>
                {
                    this.Resync();
                };
            this.Resync();
        }

        private void Resync()
        {
            FileSyncData sync = this.Solution.GetSyncData() as FileSyncData;
            this.Text = sync.Text;
            this.ImageKey = sync.ImageKey;
            this.SelectedImageKey = sync.ImageKey;
            this.ContextMenuStrip = ActionWrapper.GetContextMenu(this.Solution.ContextActions);
        }

        public Solution Solution
        {
            get;
            private set;
        }
    }
}
