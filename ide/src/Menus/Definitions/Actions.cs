using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using MOAI.Designers;

namespace MOAI.Menus.Definitions.Actions
{
    class Open : Action
    {
        public Open() : base() { }
        public Open(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = MOAI.Properties.Resources.actions_open;
            this.Text = "Open File";
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.File)
                Program.Manager.DesignersManager.OpenDesigner(this.Context as Management.File);
            else
            {
                System.Windows.Forms.OpenFileDialog ofd = new System.Windows.Forms.OpenFileDialog();
                ofd.Filter = "Lua Scripts;*.lua|All Files;*.*";
                if (ofd.ShowDialog() == System.Windows.Forms.DialogResult.OK)
                    Program.Manager.DesignersManager.OpenDesigner(new Management.File(null, null, ofd.FileName));
            }
        }
    }

    class OpenInWindowsExplorer : Action
    {
        public OpenInWindowsExplorer() : base() { }
        public OpenInWindowsExplorer(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = MOAI.Properties.Resources.actions_open;
            this.Text = "Open in Windows Explorer";
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.Project)
                System.Diagnostics.Process.Start((this.Context as Management.Project).ProjectInfo.DirectoryName);
            else if (this.Context is Management.Folder)
                System.Diagnostics.Process.Start((this.Context as Management.Folder).FolderInfo.FullName);
            else if (this.Context is Management.File)
                System.Diagnostics.Process.Start((this.Context as Management.File).FileInfo.FullName);
        }
    }

    class Close : Action
    {
        public Close() : base() { }
        public Close(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Close";
            this.Enabled = false;
        }
    }

    class Save : Action
    {
        private Designer m_CurrentEditor = null;

        public Save() : base() { }
        public Save(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = MOAI.Properties.Resources.actions_save;
            this.Text = "Save";
            this.Enabled = false;
            this.Shortcut = System.Windows.Forms.Keys.Control | System.Windows.Forms.Keys.S;

            // Listen for events.
            Program.Manager.DesignersManager.DesignerChanged += new MOAI.Designers.Manager.DesignerEventHandler(DesignersManager_DesignerChanged);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            m_CurrentEditor.OnSaveFile();
        }

        /// <summary>
        /// This event is raised when the active tab (designer) changes.
        /// </summary>
        private void DesignersManager_DesignerChanged(object sender, MOAI.Designers.Manager.DesignerEventArgs e)
        {
            if (e.Designer == null || e.Designer.File == null)
            {
                this.Enabled = false;
                this.Text = "Save";
                return;
            }

            this.Enabled = e.Designer.CanSave;
            this.m_CurrentEditor = e.Designer;
            this.Text = "Save " + e.Designer.File.FileInfo.Name;
        }
    }

    class SaveAs : Action
    {
        private Designer m_CurrentEditor = null;

        public SaveAs() : base() { }
        public SaveAs(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Save as...";
            this.Enabled = false;

            // Listen for events.
            Program.Manager.DesignersManager.DesignerChanged += new MOAI.Designers.Manager.DesignerEventHandler(DesignersManager_DesignerChanged);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            m_CurrentEditor.OnSaveFileAs();
            Program.Manager.DesignersManager.EmulateDesignerChange();
        }

        /// <summary>
        /// This event is raised when the active tab (designer) changes.
        /// </summary>
        private void DesignersManager_DesignerChanged(object sender, MOAI.Designers.Manager.DesignerEventArgs e)
        {
            if (e.Designer == null || e.Designer.File == null)
            {
                this.Enabled = false;
                this.Text = "Save as...";
                return;
            }

            this.Enabled = e.Designer.CanSave;
            this.m_CurrentEditor = e.Designer;
            this.Text = "Save " + e.Designer.File.FileInfo.Name + " as...";
        }
    }

    class SaveAll : Action
    {
        public SaveAll() : base() { }
        public SaveAll(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_save_all;
            this.Text = "Save All";
            this.Enabled = false;
        }
    }

    class Exit : Action
    {
        public Exit() : base() { }
        public Exit(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Exit";
            this.Enabled = true;

            // Listen for events.
            Program.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Program.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            // TODO: Add proper unsaved changes checking etc.. here
            Program.Manager.Stop();
        }

        /// <summary>
        /// This event is raised when a solution is loaded (opened).
        /// </summary>
        private void Manager_OnSolutionLoaded(object sender, EventArgs e)
        {
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when a solution is unloaded (closed).
        /// </summary>
        private void Manager_OnSolutionUnloaded(object sender, EventArgs e)
        {
            this.Enabled = false;
        }
    }

    class Undo : Action
    {
        public Undo() : base() { }
        public Undo(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_undo;
            this.Text = "Undo";
            this.Enabled = false;
        }
    }

    class Redo : Action
    {
        public Redo() : base() { }
        public Redo(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_redo;
            this.Text = "Redo";
            this.Enabled = false;
        }
    }

    class Cut : Action
    {
        public Cut() : base() { }
        public Cut(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_cut;
            this.Text = "Cut";
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.Folder || this.Context is Management.File)
            {
                // Add the selected file or folder to the list.
                string[] files = null;
                if (this.Context is Management.Folder)
                    files = new string[] { (this.Context as Management.Folder).FolderInfo.FullName };
                else if (this.Context is Management.File)
                    files = new string[] { (this.Context as Management.File).FileInfo.FullName };

                // This process is sourced from http://web.archive.org/web/20070218155439/http://blogs.wdevs.com/IDecember/archive/2005/10/27/10979.aspx.
                System.Windows.IDataObject data = new System.Windows.DataObject(System.Windows.DataFormats.FileDrop, files);
                MemoryStream stream = new MemoryStream(4);
                byte[] bytes = new byte[] { 2, 0, 0, 0 };
                memo.Write(bytes, 0, bytes.Length);
                data.SetData("Preferred DropEffect", memo);
                MOAI.Cache.Clipboard.Contents = data;
            }
        }
    }

    class Copy : Action
    {
        public Copy() : base() { }
        public Copy(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = MOAI.Properties.Resources.actions_copy;
            this.Text = "Copy";
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.Folder || this.Context is Management.File)
            {
                // Add the selected file or folder to the list.
                string[] files = null;
                if (this.Context is Management.Folder)
                    files = new string[] { (this.Context as Management.Folder).FolderInfo.FullName };
                else if (this.Context is Management.File)
                    files = new string[] { (this.Context as Management.File).FileInfo.FullName };

                // This process is sourced from http://web.archive.org/web/20070218155439/http://blogs.wdevs.com/IDecember/archive/2005/10/27/10979.aspx.
                System.Windows.IDataObject data = new System.Windows.DataObject(System.Windows.DataFormats.FileDrop, files);
                MemoryStream stream = new MemoryStream(4);
                byte[] bytes = new byte[] { 5, 0, 0, 0 };
                memo.Write(bytes, 0, bytes.Length);
                data.SetData("Preferred DropEffect", memo);
                MOAI.Cache.Clipboard.Contents = data;
            }
        }
    }

    class Paste : Action
    {
        public Paste() : base() { }
        public Paste(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = MOAI.Properties.Resources.actions_paste;
            this.Text = "Paste";
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.Project || this.Context is Management.Folder)
            {
                // We are copying a set of files or folders into a project using the solution
                // explorer.
                System.Windows.IDataObject data = MOAI.Cache.Clipboard.Contents;
                if (!data.GetDataPresent(System.Windows.DataFormats.FileDrop))
                    return;

                /* REWRITE
                System.Collections.Specialized.StringCollection sc = MOAI.Cache.Clipboard.ContentsFileDropList;
                if (sc == null)
                    return;

                foreach (string s in sc)
                {
                    // Get a FileInfo object representing the file to be copied.
                    FileInfo ff = new FileInfo(s);

                    if (this.Context is Management.Project)
                    {
                        ff.CopyTo(Path.Combine((this.Context as Management.Project).ProjectInfo.DirectoryName, ff.Name));
                        (this.Context as Management.Project).AddFile(
                            new Management.File(
                                this.Context as Management.Project,
                                (this.Context as Management.Project).ProjectInfo.DirectoryName,
                                ff.Name
                                )
                            );
                    }
                    else if (this.Context is Management.Folder)
                    {
                        ff.CopyTo(Path.Combine((this.Context as Management.Folder).FolderInfo.FullName, ff.Name));
                        (this.Context as Management.Folder).Add(
                            new Management.File(
                                (this.Context as Management.Folder).Project,
                                (this.Context as Management.Folder).Project.ProjectInfo.DirectoryName,
                                ff.FullName.Substring((this.Context as Management.Folder).FolderInfo.FullName.Length + 1)
                                )
                            );
                    }
                }
                */
            }
        }
    }

    class Delete : Action
    {
        public Delete() : base() { }
        public Delete(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Delete";
            this.Enabled = false;
        }
    }

    class Remove : Action
    {
        public Remove() : base() { }
        public Remove(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Remove";
            this.Enabled = false;
        }
    }

    class Rename : Action
    {
        public Rename() : base() { }
        public Rename(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Rename";
            this.Enabled = true;
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.File)
            {
                System.Windows.Forms.NodeLabelEditEventHandler handler = null;
                handler = new System.Windows.Forms.NodeLabelEditEventHandler((sender, e) =>
                {
                    // Check this is the node we are interested in.
                    if (e.Node == this.Context)
                    {
                        // Check to see if the new label is null (indiciating don't change).
                        if (e.Label == null)
                        {
                            (this.Context as Management.File).Text = (this.Context as Management.File).FileInfo.Name;
                            (this.Context as Management.File).TreeView.AfterLabelEdit -= handler;
                            (this.Context as Management.File).TreeView.LabelEdit = false;
                            return;
                        }
                        
                        // Check to see if the label is valid.
                        if (e.Label.IndexOfAny(new char[] { '/', '\\', ':', '*', '"', '<', '>', '|' }) != -1)
                        {
                            (this.Context as Management.File).Text = (this.Context as Management.File).FileInfo.Name;
                            System.Windows.Forms.MessageBox.Show(@"Please enter a filename; it must not contain any
    " + "of the following characters: / \\ : * \" < > |", "Rename Failed", System.Windows.Forms.MessageBoxButtons.OK,
                                System.Windows.Forms.MessageBoxIcon.Error);
                            (this.Context as Management.File).BeginEdit();
                        }
                        else
                        {
                            // Attempt to rename the file.
                            try
                            {
                                (this.Context as Management.File).FileInfo.MoveTo(
                                    Path.Combine(
                                        (this.Context as Management.File).FileInfo.DirectoryName,
                                        e.Label
                                    )
                                );
                                (this.Context as Management.File).TreeView.AfterLabelEdit -= handler;
                                (this.Context as Management.File).TreeView.LabelEdit = false;
                                (this.Context as Management.File).Text = e.Label;
                                (this.Context as Management.File).PerformRenamed();

                                // Force the solution explorer to refresh.
                                MOAI.Tools.SolutionExplorerTool s = Program.Manager.ToolsManager.Get(typeof(MOAI.Tools.SolutionExplorerTool)) as MOAI.Tools.SolutionExplorerTool;
                                if (s != null)
                                    s.ReloadTree();
                            }
                            catch (IOException)
                            {
                                (this.Context as Management.File).Text = (this.Context as Management.File).FileInfo.Name;
                                System.Windows.Forms.MessageBox.Show("Unable to rename file.", "Rename Failed", System.Windows.Forms.MessageBoxButtons.OK,
                                    System.Windows.Forms.MessageBoxIcon.Error);
                                (this.Context as Management.File).BeginEdit();
                            }
                        }
                    }
                });
                (this.Context as Management.File).TreeView.AfterLabelEdit += handler;
                (this.Context as Management.File).TreeView.LabelEdit = true;
                (this.Context as Management.File).BeginEdit();
            }
        }
    }

    class Exclude : Action
    {
        public Exclude() : base() { }
        public Exclude(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Exclude From Project";
            this.Enabled = false;
        }
    }

    class SelectAll : Action
    {
        public SelectAll() : base() { }
        public SelectAll(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Select All";
            this.Enabled = false;
        }
    }

    class QuickFind : Action
    {
        public QuickFind() : base() { }
        public QuickFind(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Quick Find";
            this.Enabled = false;
        }
    }

    class QuickReplace : Action
    {
        public QuickReplace() : base() { }
        public QuickReplace(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Quick Replace";
            this.Enabled = false;
        }
    }

    class FindInFiles : Action
    {
        public FindInFiles() : base() { }
        public FindInFiles(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_find_in_files;
            this.Text = "Find in Files";
            this.Enabled = false;
        }
    }

    class ReplaceInFiles : Action
    {
        public ReplaceInFiles() : base() { }
        public ReplaceInFiles(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Replace in Files";
            this.Enabled = false;
        }
    }

    class GoTo : Action
    {
        public GoTo() : base() { }
        public GoTo(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Go To...";
            this.Enabled = false;
        }
    }

    class Preferences : Action
    {
        public Preferences() : base() { }
        public Preferences(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Preferences";
            this.Enabled = false;
        }
    }

    class Properties : Action
    {
        public Properties() : base() { }
        public Properties(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Properties";
            this.Enabled = false;
        }
    }
}
