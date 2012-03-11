using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Management;
using Moai.Platform.Linux.Menus;

namespace Moai.Platform.Linux.UI
{
    public class SolutionTreeNode : BaseTreeNode
    {
        public SolutionTreeNode(Solution solution)
        {
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
            this.ContextMenu = ActionWrapper.GetContextMenu(this.Solution.ContextActions);
        }

        public Solution Solution
        {
            get;
            private set;
        }
    }
}
