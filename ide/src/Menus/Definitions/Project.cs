using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOAI.Management;

namespace MOAI.Menus.Definitions.Project
{
    class New : Action
    {
        public New() : base() { }
        public New(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.project_create;
            this.Text = "New Project";
            this.Enabled = false;
        }
    }

    class AddFile : Action
    {
        public AddFile() : base() { }
        public AddFile(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.file_add;
            this.Text = "Add New Item";
            this.Enabled = false;
        }
    }

    class AddModel : Action
    {
        public AddModel() : base() { }
        public AddModel(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.model_add;
            this.Text = "Add Model...";
            this.Enabled = false;
        }

        public override void OnActivate()
        { 
            /*FileTreeNode ftn = new FileTreeNode();
            if (TargetDirectory != null)
                ftn.fileRef = TargetDirectory;
            else
                // TODO: Make this check the solution based on what file is currently has focus.
                ftn.projectRef = Program.MainWindow.CurrentSolution.Projects[0];
            Creation.AddModel(ftn);*/
        }
    }

    class AddImage : Action
    {
        public AddImage() : base() { }
        public AddImage(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.image_add;
            this.Text = "Add Image...";
            this.Enabled = false;
        }

        public override void OnActivate()
        {
            /*FileTreeNode ftn = new FileTreeNode();
            if (TargetDirectory != null)
                ftn.fileRef = TargetDirectory;
            else
                // TODO: Make this check the solution based on what file is currently has focus.
                ftn.projectRef = Program.MainWindow.CurrentSolution.Projects[0];
            Creation.AddImage(ftn);*/
        }
    }

    class AddAudio : Action
    {
        public AddAudio() : base() { }
        public AddAudio(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.audio_add;
            this.Text = "Add Audio...";
            this.Enabled = false;
        }
    }

    class AddTemplate : Action
    {
        public AddTemplate() : base() { }
        public AddTemplate(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null; //Properties.Resources.template_add;
            this.Text = "Add Template...";
            this.Enabled = false;
        }
    }

    class AddArea : Action
    {
        public AddArea() : base() { }
        public AddArea(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.area_add;
            this.Text = "Add Area...";
            this.Enabled = false;
        }
    }

    class AddWorld : Action
    {
        public AddWorld() : base() { }
        public AddWorld(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.world_add;
            this.Text = "Add World...";
            this.Enabled = false;
        }
    }

    class AddClass : Action
    {
        public AddClass() : base() { }
        public AddClass(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.class_add;
            this.Text = "Add Class...";
            this.Enabled = false;
        }
    }

    class AddScript : Action
    {
        public AddScript() : base() { }
        public AddScript(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.script_add;
            this.Text = "Add Script...";
            this.Enabled = false;
        }
    }

    class AddNewItem : Action
    {
        public AddNewItem() : base() { }
        public AddNewItem(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Add New Item";
            this.Enabled = false;
        }
    }

    class AddExistingItem : Action
    {
        public AddExistingItem() : base() { }
        public AddExistingItem(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Add Existing Item";
            this.Enabled = false;
        }
    }

    class AddReference : Action
    {
        public AddReference() : base() { }
        public AddReference(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = Properties.Resources.reference;
            this.Text = "Add Reference...";
            this.Enabled = false;
        }
    }

    class ProjProperties : Action
    {
        public ProjProperties() : base() { }
        public ProjProperties(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Project Properties";
            this.Enabled = false;
        }
    }

    class ProjDependencies : Action
    {
        public ProjDependencies() : base() { }
        public ProjDependencies(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Project Dependencies...";
            this.Enabled = false;
        }
    }

    class ProjBuildOrder : Action
    {
        public ProjBuildOrder() : base() { }
        public ProjBuildOrder(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Project Build Order...";
            this.Enabled = false;
        }
    }

    class Build : Action
    {
        public Build() : base() { }
        public Build(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Build MyProject";
            this.Enabled = false;
        }

        public override void OnActivate()
        {
            /*
            Compilation.ProjectBuilder pb = new Compilation.ProjectBuilder(Program.MainWindow.CurrentSolution.Projects[0], Program.MainWindow.BuildOutput);
            pb.Build(Roket3D.Compilation.BuildMode.DEBUG);
            */
        }
    }

    class Rebuild : Action
    {
        public Rebuild() : base() { }
        public Rebuild(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Rebuild MyProject";
            this.Enabled = false;
        }
    }

    class Clean : Action
    {
        public Clean() : base() { }
        public Clean(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Clean MyProject";
            this.Enabled = false;
        }
    }

    class StartWithDebug : Action
    {
        public StartWithDebug() : base() { }
        public StartWithDebug(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = Properties.Resources.debug;
            this.Text = "Start with Debugging";
            this.Enabled = true;

            // Listen for events.
            Program.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Program.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            // FIXME: Fix this so that it uses the context of the project selected.
            if (this.Context != null && this.Context is Management.Project)
                Program.Manager.DebugManager.Start(this.Context as Management.Project);
            else
                System.Windows.Forms.MessageBox.Show("Unable to start this project with debugging.  Unable to determine the project to run.");
        }

        /// <summary>
        /// This event is raised when debugging starts.
        /// </summary>
        private void DebugManager_OnDebugStart(object sender, EventArgs e)
        {
            this.Enabled = false;
        }

        /// <summary>
        /// This event is raised when debugging stops.
        /// </summary>
        private void DebugManager_OnDebugStop(object sender, EventArgs e)
        {
            this.Enabled = true;
        }
    }

    class SetAsStartupProject : Action
    {
        public SetAsStartupProject() : base() { }
        public SetAsStartupProject(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Set as Startup Project";
            this.Enabled = false;
        }
    }
}
