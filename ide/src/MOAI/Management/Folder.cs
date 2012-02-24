using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Windows.Forms;
using MOAI.Operatables;

namespace MOAI.Management
{
    public class Folder : File, ICuttable, ICopyable, IRemovable, IRenamable, IPastable
    {
        private DirectoryInfo p_FolderInfo = null;
        private List<File> p_Files = null;

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
            this.p_Files = new List<File>();
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
            this.p_Files = new List<File>();
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
        public System.Collections.ObjectModel.ReadOnlyCollection<File> Files
        {
            get
            {
                return this.p_Files.AsReadOnly();
            }
        }

        /// <summary>
        /// Adds a file to the list of files contained within this folder.
        /// </summary>
        public void Add(File file)
        {
            this.p_Files.Add(file);
            if (file is Folder)
            {
                Folder ff = file as Folder;
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
        public void AddWithoutEvent(File file)
        {
            this.p_Files.Add(file);
        }

        /// <summary>
        /// Searches for and propagates a removal request for a file within
        /// this folder, or any of it's subfolders.
        /// </summary>
        public void Remove(File file)
        {
            if (this.p_Files.Contains(file))
            {
                this.p_Files.Remove(file);
                if (file is Folder)
                {
                    Folder ff = file as Folder;
                    ff.FileAdded -= new EventHandler(ff_FileAdded);
                    ff.FileRemoved -= new EventHandler(ff_FileRemoved);
                }
                if (this.FileRemoved != null)
                    this.FileRemoved(this, new EventArgs());
            }
            else
            {
                foreach (File f in this.p_Files.ToList())
                {
                    if (f is Folder)
                    {
                        // Request the subfolder to remove the file if they
                        // have it.
                        (f as Folder).Remove(file);
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
            // Add the selected folder to the list.
            string[] files = new string[] { this.FolderInfo.FullName };

            // This process is sourced from http://web.archive.org/web/20070218155439/http://blogs.wdevs.com/IDecember/archive/2005/10/27/10979.aspx.
            System.Windows.Forms.IDataObject data = new System.Windows.Forms.DataObject(System.Windows.Forms.DataFormats.FileDrop, files);
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
                // Remove the folder from the tree view.
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
            string[] files = new string[] { this.FolderInfo.FullName };

            // This process is sourced from http://web.archive.org/web/20070218155439/http://blogs.wdevs.com/IDecember/archive/2005/10/27/10979.aspx.
            System.Windows.Forms.IDataObject data = new System.Windows.Forms.DataObject(System.Windows.Forms.DataFormats.FileDrop, files);
            MemoryStream stream = new MemoryStream(4);
            byte[] bytes = new byte[] { 5, 0, 0, 0 };
            stream.Write(bytes, 0, bytes.Length);
            data.SetData("Preferred DropEffect", stream);
            MOAI.Cache.Clipboard.Contents = data;
        }

        /// <summary>
        /// Called when the user wants to paste into this folder.
        /// </summary>
        void IPastable.Paste()
        {
            // We are copying a set of files or folders into a project using the solution
            // explorer.
            System.Windows.Forms.IDataObject data = MOAI.Cache.Clipboard.Contents;
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
            this.Project.Save();
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
        /// Associates this folder with a tree node.
        /// </summary>
        /// <param name="node"></param>
        public override void Associate(System.Windows.Forms.TreeNode node)
        {
            // Set properties.
            this.Text = this.ToString();
            this.ImageKey = Associations.GetImageKey("folder");
            this.SelectedImageKey = this.ImageKey;

            // We also need to loop through the children of this folder
            // to add them to.
            this.Nodes.Clear();
            foreach (File f in this.p_Files)
                f.Associate(this);

            // Add this file to the node.
            node.Nodes.Add(this);
        }

        /// <summary>
        /// Returns the context menu for this folder.
        /// </summary>
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                // Set the context menu for the node.
                ContextMenuStrip ret = new ContextMenuStrip();
                ret.Items.AddRange(new ToolStripItem[] {
                    new ToolStripMenuItem("Add", null, new ToolStripItem[] {
                        Menus.Manager.WrapAction(new Menus.Definitions.Project.AddNewItem(this)),
                        Menus.Manager.WrapAction(new Menus.Definitions.Project.AddExistingItem(this)),
                        Menus.Manager.WrapAction(new Menus.Definitions.Project.AddFolder(this)),
                        new ToolStripSeparator(),
                        Menus.Manager.WrapAction(new Menus.Definitions.Project.AddScript(this)),
                        Menus.Manager.WrapAction(new Menus.Definitions.Project.AddClass(this))
                    }),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Exclude(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Cut(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Copy(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Paste(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Remove(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Rename(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.OpenInWindowsExplorer(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Properties(this))
                });
                return ret;
            }
        }

        #endregion
    }
}
