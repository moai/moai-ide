using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Management;
using Moai.Platform.Linux.Menus;

namespace Moai.Platform.Linux.UI
{
    public class FileTreeNode : BaseTreeNode
    {
        public FileTreeNode(File file)
        {
            this.File = file;
            this.File.SyncDataChanged += (sender, e) =>
                {
                    this.Resync();
                };
            this.Resync();
        }

        private void Resync()
        {
            FileSyncData sync = this.File.GetSyncData() as FileSyncData;
            this.Text = sync.Text;
            this.ImageKey = sync.ImageKey;
            this.ContextMenu = ActionWrapper.GetContextMenu(this.File.ContextActions);
        }

        public File File
        {
            get;
            private set;
        }
    }
}
