using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Moai.Platform.UI;
using Moai.Platform.Menus;

namespace Moai.Platform.Management
{
    public class Folder : File, ICuttable, ICopyable, IRemovable, IRenamable, IPastable, ISyncable
    {
        private DirectoryInfo p_FolderInfo = null;
        private List<Management.File> p_Files = null;

        public event EventHandler FileAdded;
        public event EventHandler FileRemoved;
        public event EventHandler FileRenamed;

        /// <summary>
        /// Creates a new Folder object for representing a folder in the project.
        /// </summary>
        /// <param name="p">The project that owns this folder.</param>
        public Folder(Project p)
            : base(p)
        {
            this.p_Files = new List<Management.File>();
        }

        /// <summary>
        /// Creates a new File object based on an relative path and a Project object.
        /// </summary>
        /// <param name="p">The project that owns this folder.</param>
        /// <param name="parent">The parent director (the directory the project file is located in).</param>
        /// <param name="relpath">The relative path to the file.</param>
        public Folder(Project p, string parent, string relpath)
            : base(p)
        {
            this.p_Files = new List<Management.File>();
            this.p_FolderInfo = new DirectoryInfo(Path.Combine(parent, relpath));
        }

        /// <summary>
        /// The DirectoryInfo object that represents this folder on-disk.
        /// </summary>
        public DirectoryInfo FolderInfo
        {
            get
            {
                return this.p_FolderInfo;
            }
        }

        /// <summary>
        /// A read-only list of the files within the this folder.
        /// </summary>
        public System.Collections.ObjectModel.ReadOnlyCollection<Management.File> Files
        {
            get
            {
                return this.p_Files.AsReadOnly();
            }
        }

        /// <summary>
        /// Adds a file to the list of files contained within this folder.
        /// </summary>
        public void Add(Management.File file)
        {
            this.p_Files.Add(file);
            if (file is Management.Folder)
            {
                Management.Folder ff = file as Management.Folder;
                ff.FileAdded += new EventHandler(ff_FileAdded);
                ff.FileRemoved += new EventHandler(ff_FileRemoved);
            }
            if (this.FileAdded != null)
                this.FileAdded(this, new EventArgs());
        }

        /// <summary>
        /// Adds a file to the list of files within this folder without 
        /// raising the FileAdded event (useful when initially loading a
        /// project so we don't accidently autosave over the top of the
        /// file we're currently loading from).
        /// </summary>
        public void AddWithoutEvent(Management.File file)
        {
            this.p_Files.Add(file);
        }

        /// <summary>
        /// Searches for and propagates a removal request for a file within
        /// this folder, or any of it's subfolders.
        /// </summary>
        public void Remove(Management.File file)
        {
            if (this.p_Files.Contains(file))
            {
                this.p_Files.Remove(file);
                if (file is Management.Folder)
                {
                    Management.Folder ff = file as Management.Folder;
                    ff.FileAdded -= new EventHandler(ff_FileAdded);
                    ff.FileRemoved -= new EventHandler(ff_FileRemoved);
                }
                if (this.FileRemoved != null)
                    this.FileRemoved(this, new EventArgs());
            }
            else
            {
                foreach (Management.File f in this.p_Files.ToList())
                {
                    if (f is Management.Folder)
                    {
                        // Request the subfolder to remove the file if they
                        // have it.
                        (f as Management.Folder).Remove(file);
                    }
                }
            }
        }

        /// <summary>
        /// This function propagates FileAdded events from folders as
        /// FileAdded events on the project itself.
        /// </summary>
        private void ff_FileAdded(object sender, EventArgs e)
        {
            if (this.FileAdded != null)
                this.FileAdded(this, new EventArgs());
        }

        /// <summary>
        /// This function propagates FileRemoved events from folders as
        /// FileRemoved events on the project itself.
        /// </summary>
        private void ff_FileRemoved(object sender, EventArgs e)
        {
            if (this.FileRemoved != null)
                this.FileRemoved(this, new EventArgs());
        }

        /// <summary>
        /// Overrides the ToString() function which is used by the tree view
        /// so that it displays the folder name.
        /// </summary>
        public override string ToString()
        {
            return this.FolderInfo.Name;
        }

        #region Operation Implementions

        /// <summary>
        /// Boolean value indicating whether this folder can be cut.
        /// </summary>
        bool ICuttable.CanCut
        {
            get { return true; }
        }

        /// <summary>
        /// Boolean value indicating whether this folder can be copied.
        /// </summary>
        bool ICopyable.CanCopy
        {
            get { return true; }
        }

        /// <summary>
        /// Boolean value indicating whether this folder can be pasted into.
        /// </summary>
        bool IPastable.CanPaste
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
        /// Called when the user wants to paste into this folder.
        /// </summary>
        void IPastable.Paste()
        {
            throw new NotImplementedException();
            // We are copying a set of files or folders into a project using the solution
            // explorer.
            /* FIXME: Implement this.
            System.Windows.Forms.IDataObject data = Moai.Cache.Clipboard.Contents;
            if (!data.GetDataPresent(System.Windows.Forms.DataFormats.FileDrop))
                return;

            // Check to see whether we are doing a cut or copy.
            bool iscut = false;
            if (data.GetDataPresent("Preferred DropEffect"))
                iscut = ((data.GetData("Preferred DropEffect") as MemoryStream).ReadByte() == 2);

            // Get the target folder.
            string folder = this.FolderInfo.FullName;

            // Move or copy the selected files.
            string[] files = data.GetData(System.Windows.Forms.DataFormats.FileDrop) as string[];
            foreach (FileInfo f in files.Select(input => new FileInfo(input)))
            {
                // Check to make sure the file doesn't already exist in the destination.
                if (System.IO.File.Exists(Path.Combine(folder, f.Name)))
                {
                    System.Windows.Forms.MessageBox.Show(
                        f.Name + " already exists in the destination folder.  It will not be copied or moved.",
                        "File Already Exists",
                        System.Windows.Forms.MessageBoxButtons.OK,
                        System.Windows.Forms.MessageBoxIcon.Error
                    );
                    continue;
                }

                if (iscut)
                    f.MoveTo(Path.Combine(folder, f.Name));
                else
                    f.CopyTo(Path.Combine(folder, f.Name));

                this.AddWithoutEvent(new Management.File(
                    this.Project,
                    this.Project.ProjectInfo.DirectoryName,
                    PathHelpers.GetRelativePath(
                        this.Project.ProjectInfo.DirectoryName,
                        Path.Combine(folder, f.Name)
                        )
                    ));
            }

            // Force the project to be saved now.
            this.Project.Save(); */
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
        /// Returns the context menu for this folder.
        /// </summary>
        public override Moai.Platform.Menus.Action[] ContextActions
        {
            get
            {
                // Create the context action list.
                return new Moai.Platform.Menus.Action[]
                {
                    new GroupAction("Add", null, new Moai.Platform.Menus.Action[]
                    {
                        new Menus.Definitions.Project.AddNewItem(this),
                        new Menus.Definitions.Project.AddExistingItem(this),
                        new Menus.Definitions.Project.AddFolder(this),
                        new SeperatorAction(),
                        new Menus.Definitions.Project.AddScript(this),
                        new Menus.Definitions.Project.AddClass(this)
                    }),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.Exclude(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.Cut(this),
                    new Menus.Definitions.Actions.Copy(this),
                    new Menus.Definitions.Actions.Paste(this),
                    new Menus.Definitions.Actions.Remove(this),
                    new Menus.Definitions.Actions.Rename(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.OpenInWindowsExplorer(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.Properties(this)
                };
            }
        }

        #region ISyncable Members

        public override ISyncData GetSyncData()
        {
            return new FileSyncData { Text = this.ToString(), ImageKey = "folder" };
        }

        #endregion
    }
}
