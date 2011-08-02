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
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_open;
            this.Text = "Open File";
            this.Enabled = false;
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
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_open;
            this.Text = "Open in Windows Explorer";
            this.Enabled = false;
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
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_copy;
            this.Text = "Copy";
            this.Enabled = false;
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
            this.Implemented = false;
            this.ItemIcon = MOAI.Properties.Resources.actions_paste;
            this.Text = "Paste";
            this.Enabled = false;
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
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Rename";
            this.Enabled = false;
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
