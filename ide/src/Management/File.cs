using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOAI.Collections;
using System.IO;
using System.Drawing;
using System.Windows.Forms;
using MOAI.Operatables;

namespace MOAI.Management
{
    public class File : System.Windows.Forms.TreeNode, ICuttable, ICopyable, IRemovable, IRenamable
    {
        private Project p_Project = null;
        private FileInfo p_FileInfo = null;

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

            // This process is sourced from http://web.archive.org/web/20070218155439/http://blogs.wdevs.com/IDecember/archive/2005/10/27/10979.aspx.
            System.Windows.IDataObject data = new System.Windows.DataObject(System.Windows.DataFormats.FileDrop, files);
            MemoryStream stream = new MemoryStream(4);
            byte[] bytes = new byte[] { 2, 0, 0, 0 };
            stream.Write(bytes, 0, bytes.Length);
            data.SetData("Preferred DropEffect", stream);
            MOAI.Cache.Clipboard.Contents = data;

            // Change the icon to faded, then listen to see if the clipboard
            // gets overridden.
            this.ImageKey += ":Faded";
            this.SelectedImageKey += ":Faded";
            EventHandler<MOAI.Cache.ClipboardEventArgs> ev = null;
            ev = (sender, e) =>
            {
                string key = this.ImageKey;
                if (key.IndexOf(":Faded") == -1)
                    return;
                key = key.Substring(0, key.IndexOf(":Faded"));
                this.ImageKey = key;
                this.SelectedImageKey = key;
                MOAI.Cache.Clipboard.ClipboardChanged -= ev;
            };
            MOAI.Cache.Clipboard.ClipboardChanged += ev;

            // Now listen to see when the file disappears.
            FileSystemWatcher watcher = new FileSystemWatcher(new FileInfo(files[0]).DirectoryName, new FileInfo(files[0]).Name);
            watcher.Deleted += (sender, e) =>
            {
                // Remove the file from the tree view.
                this.Project.RemoveFile(this);
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

            // This process is sourced from http://web.archive.org/web/20070218155439/http://blogs.wdevs.com/IDecember/archive/2005/10/27/10979.aspx.
            System.Windows.IDataObject data = new System.Windows.DataObject(System.Windows.DataFormats.FileDrop, files);
            MemoryStream stream = new MemoryStream(4);
            byte[] bytes = new byte[] { 5, 0, 0, 0 };
            stream.Write(bytes, 0, bytes.Length);
            data.SetData("Preferred DropEffect", stream);
            MOAI.Cache.Clipboard.Contents = data;
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

        /// <summary>
        /// Associates this file with a tree node.
        /// </summary>
        /// <param name="node"></param>
        public virtual void Associate(System.Windows.Forms.TreeNode node)
        {
            // Set properties.
            this.Text = this.ToString();
            if (this.p_FileInfo.Extension != "")
                this.ImageKey = Associations.GetImageKey(this.p_FileInfo.Extension.Substring(1));
            else
                this.ImageKey = null;
            this.SelectedImageKey = this.ImageKey;

            // Add this file to the node.
            node.Nodes.Add(this);
        }

        /// <summary>
        /// Informs the developer that the Name property would actually provide the control's name, not
        /// the name of the file or the folder.
        /// </summary>
        public new string Name
        {
            get { throw new InvalidOperationException("The name property should not be used as it indicates the control's name, not the name of " +
"the file or folder.  Use the FileInfo property to access the file information, or ControlName to access " +
"the control's name."); }
        }

        /// <summary>
        /// Provides access to the Name property if desired.
        /// </summary>
        public string ControlName
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// Returns the context menu for this file.
        /// </summary>
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                // Set the context menu for the node.
                ContextMenuStrip ret = new ContextMenuStrip();
                ret.Items.AddRange(new ToolStripItem[] {
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Open(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Views.Code(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Views.Designer(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Exclude(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Cut(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Copy(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Delete(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Rename(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.OpenInWindowsExplorer(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Properties(this))
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
