using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moai.Platform.Management;
using Moai.Platform.Windows.Menus;

namespace Moai.Platform.Windows.UI
{
    public class FileTreeNode : TreeNode
    {
        public FileTreeNode(File file)
        {
            this.File = file;
            this.Tag = file;
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
            this.SelectedImageKey = sync.ImageKey;
            this.ContextMenuStrip = ActionWrapper.GetContextMenu(this.File.ContextActions);
        }

        public File File
        {
            get;
            private set;
        }
    }
}
