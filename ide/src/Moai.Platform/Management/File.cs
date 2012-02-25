using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Moai.Platform.UI;
using Moai.Platform;
using Moai.Platform.Menus;

namespace Moai.Platform.Management
{
    public class File : ICuttable, ICopyable, IRemovable, IRenamable
    {
        private Project p_Project = null;
        private FileInfo p_FileInfo = null;
        protected ITreeNode p_BackingNode = Central.Platform.UI.CreateTreeNode();

        /// <summary>
        /// A protected constructor so that derived classes such as Folder only have
        /// to provide the project as an argument to the constructor.
        /// </summary>
        /// <param name="p">The project that owns this folder.</param>
        protected File(Project p)
        {
            this.p_Project = p;
        }

        /// <summary>
        /// Creates a new File object based on an relative path and a Project object.
        /// </summary>
        /// <param name="p">The project that owns this folder.</param>
        /// <param name="parent">The parent directory (the directory the project file is located in).</param>
        /// <param name="relpath">The relative path to the file.</param>
        public File(Project p, string parent, string relpath)
        {
            if (p != null && parent != null)
            {
                this.p_Project = p;
                this.p_FileInfo = new FileInfo(Path.Combine(parent, relpath));
            }
            else if (p == null && parent == null)
            {
                this.p_Project = null;
                this.p_FileInfo = new FileInfo(relpath);
            }
            else
                throw new NotSupportedException();
        }

        /// <summary>
        /// The FileInfo object that represents this file on-disk.
        /// </summary>
        public FileInfo FileInfo
        {
            get
            {
                return this.p_FileInfo;
            }
        }

        /// <summary>
        /// The project that owns this file.
        /// </summary>
        public Project Project
        {
            get
            {
                return this.p_Project;
            }
        }

        /// <summary>
        /// Performs the FileRenamed event.  Used by external code that renames
        /// files on disk so that any relevant aspects of the IDE get updated.
        /// </summary>
        public void PerformRenamed()
        {
            if (this.p_Project != null)
                this.p_Project.PerformRename(this);
        }

        #region Operation Implementions

        /// <summary>
        /// Boolean value indicating whether this file can be cut.
        /// </summary>
        bool ICuttable.CanCut
        {
            get { return true; }
        }

        /// <summary>
        /// Boolean value indicating whether this file can be copied.
        /// </summary>
        bool ICopyable.CanCopy
        {
            get { return true; }
        }

        /// <summary>
        /// Called when the user wants to cut this file.
        /// </summary>
        void ICuttable.Cut()
        {
            // Add the selected file to the list.
            string[] files = new string[] { this.FileInfo.FullName };

            throw new NotImplementedException();

            Central.Platform.Clipboard.Cut(ClipboardContentType.FileDrop, files);

            // This process is sourced from http://web.archive.org/web/20070218155439/http://blogs.wdevs.com/IDecember/archive/2005/10/27/10979.aspx.
            /*System.Windows.Forms.IDataObject data = new System.Windows.Forms.DataObject(System.Windows.Forms.DataFormats.FileDrop, files);
            MemoryStream stream = new MemoryStream(4);
            byte[] bytes = new byte[] { 2, 0, 0, 0 };
            stream.Write(bytes, 0, bytes.Length);
            data.SetData("Preferred DropEffect", stream);
            Moai.Cache.Clipboard.Contents = data;*/

            // Change the icon to faded, then listen to see if the clipboard
            // gets overridden.
            this.p_BackingNode.ImageKey += ":Faded";
            this.p_BackingNode.SelectedImageKey += ":Faded";
            EventHandler ev = null;
            ev = (sender, e) =>
            {
                string key = this.p_BackingNode.ImageKey;
                if (key.IndexOf(":Faded") == -1)
                    return;
                key = key.Substring(0, key.IndexOf(":Faded"));
                this.p_BackingNode.ImageKey = key;
                this.p_BackingNode.SelectedImageKey = key;
                Central.Platform.Clipboard.ContentsChanged -= ev;
            };
            Central.Platform.Clipboard.ContentsChanged += ev;

            // Now listen to see when the file disappears.
            FileSystemWatcher watcher = new FileSystemWatcher(new FileInfo(files[0]).DirectoryName, new FileInfo(files[0]).Name);
            watcher.Deleted += (sender, e) =>
            {
                // Remove the file from the tree view.
                this.Project.PerformRemove(this);
            };
            watcher.EnableRaisingEvents = true;
        }

        /// <summary>
        /// Called when the user wants to copy this file.
        /// </summary>
        void ICopyable.Copy()
        {
            // Add the selected file or folder to the list.
            string[] files = new string[] { this.FileInfo.FullName };

            throw new NotImplementedException();

            Central.Platform.Clipboard.Copy(ClipboardContentType.FileDrop, files);

            // This process is sourced from http://web.archive.org/web/20070218155439/http://blogs.wdevs.com/IDecember/archive/2005/10/27/10979.aspx.
            /*System.Windows.Forms.IDataObject data = new System.Windows.Forms.DataObject(System.Windows.Forms.DataFormats.FileDrop, files);
            MemoryStream stream = new MemoryStream(4);
            byte[] bytes = new byte[] { 5, 0, 0, 0 };
            stream.Write(bytes, 0, bytes.Length);
            data.SetData("Preferred DropEffect", stream);
            Moai.Cache.Clipboard.Contents = data;*/
        }

        /// <summary>
        /// Called when the user wants to remove this file from it's container.
        /// </summary>
        void IRemovable.Remove()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// Called when the user wants to rename this file.
        /// </summary>
        void IRenamable.Rename()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region Tree Node Functionality

        public object BackingNode
        {
            get
            {
                return this.p_BackingNode;
            }
        }

        /// <summary>
        /// Associates this file with a tree node.
        /// </summary>
        /// <param name="node"></param>
        public virtual void Associate(ITreeNode node)
        {
            // Set properties.
            this.p_BackingNode.Text = this.ToString();
            if (this.p_FileInfo.Extension != "")
                this.p_BackingNode.ImageKey = Associations.GetImageKey(this.p_FileInfo.Extension.Substring(1));
            else
                this.p_BackingNode.ImageKey = null;
            this.p_BackingNode.SelectedImageKey = this.p_BackingNode.ImageKey;

            // Add this file to the node.
            node.Nodes.Add(this.p_BackingNode);
        }

        /// <summary>
        /// Returns the context menu for this file.
        /// </summary>
        public virtual IContextMenuStrip ContextMenuStrip
        {
            get
            {
                // Set the context menu for the node.
                IContextMenuStrip ret = Central.Platform.UI.CreateContextMenuStrip();
                ret.Items.AddRange(new IToolStripItem[] {
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Open(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Views.Code(this)),
                    MenusManager.WrapAction(new Menus.Definitions.Views.Designer(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Exclude(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Cut(this)),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Copy(this)),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Delete(this)),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Rename(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.OpenInWindowsExplorer(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Properties(this))
                });
                return ret;
            }
        }

        /// <summary>
        /// Overrides the ToString() function which is used by the tree view
        /// so that it displays the file name.
        /// </summary>
        public override string ToString()
        {
            return this.FileInfo.Name;
        }

        #endregion
    }
}
