using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Management;
using Moai.Platform.Linux.Menus;

namespace Moai.Platform.Linux.UI
{
    public class ProjectTreeNode : BaseTreeNode
    {
        public ProjectTreeNode(Project project)
        {
            this.Project = project;
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
            this.ContextMenu = ActionWrapper.GetContextMenu(this.Project.ContextActions);
        }

        public Project Project
        {
            get;
            private set;
        }
    }
}
