using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moai.Platform.Management;
using Moai.Platform.Windows.Menus;

namespace Moai.Platform.Windows.UI
{
    public class ProjectTreeNode : TreeNode
    {
        public ProjectTreeNode(Project project)
        {
            this.Project = project;
            this.Tag = project;
            this.Project.SyncDataChanged += (sender, e) =>
                {
                    this.Resync();
                };
            this.Resync();
        }

        private void Resync()
        {
            FileSyncData sync = this.Project.GetSyncData() as FileSyncData;
            this.Text = sync.Text;
            this.ImageKey = sync.ImageKey;
            this.SelectedImageKey = sync.ImageKey;
            this.ContextMenuStrip = ActionWrapper.GetContextMenu(this.Project.ContextActions);
        }

        public Project Project
        {
            get;
            private set;
        }
    }
}
