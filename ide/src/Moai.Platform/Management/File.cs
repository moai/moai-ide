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
    public class File : ICuttable, ICopyable, IRemovable, IRenamable, ISyncable
    {
        private Project p_Project = null;
        private FileInfo p_FileInfo = null;
        protected bool m_IsFaded = false;

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

            Central.Platform.Clipboard.Cut(ClipboardContentType.FileDrop, files);

            // Change the icon to faded, then listen to see if the clipboard
            // gets overridden.
            this.m_IsFaded = true;
            this.OnSyncDataChanged();
            EventHandler ev = null;
            ev = (sender, e) =>
            {
                this.m_IsFaded = false;
                this.OnSyncDataChanged();
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

            Central.Platform.Clipboard.Copy(ClipboardContentType.FileDrop, files);
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

        /// <summary>
        /// Returns the context menu for this file.
        /// </summary>
        public virtual Moai.Platform.Menus.Action[] ContextActions
        {
            get
            {
                // Create the context action list.
                return new Moai.Platform.Menus.Action[]
                {
                    new Menus.Definitions.Actions.Open(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Views.Code(this),
                    new Menus.Definitions.Views.Designer(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.Exclude(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.Cut(this),
                    new Menus.Definitions.Actions.Copy(this),
                    new Menus.Definitions.Actions.Delete(this),
                    new Menus.Definitions.Actions.Rename(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.OpenInWindowsExplorer(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.Properties(this)
                };
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

        #region ISyncable Members

        public event EventHandler SyncDataChanged;

        public virtual ISyncData GetSyncData()
        {
            // Set properties.
            string key;
            if (this.p_FileInfo.Extension != "")
            {
                key = Associations.GetImageKey(this.p_FileInfo.Extension.Substring(1));
                if (this.m_IsFaded)
                    key += ":Faded";
            }
            else
                key = null;

            return new FileSyncData { Text = this.ToString(), ImageKey = key };
        }

        protected void OnSyncDataChanged()
        {
            if (this.SyncDataChanged != null)
                this.SyncDataChanged(this, new EventArgs());
        }

        #endregion
    }
}
