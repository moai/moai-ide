using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Menus.Definitions.Project
{
    public class New : Action
    {
        public New() : base() { }
        public New(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.project_create;
            this.Text = "New Project";
            this.Enabled = false;
        }
    }

    public class Existing : Action
    {
        public Existing() : base() { }
        public Existing(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null;
            this.Text = "Existing Project...";
            this.Enabled = false;
        }
    }

    public class AddNewItem : Action
    {
        public AddNewItem() : base() { }
        public AddNewItem(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = IDE.Resources.Images.file_add;
            this.Text = "Add New Item...";
            this.Enabled = true;

            // Listen for events.
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.Project)
                (this.Context as Management.Project).AddFileInteractive();
            else if (this.Context is Management.Folder)
                (this.Context as Management.Folder).Project.AddFileInteractive(this.Context as Management.Folder);
            else
                Central.Manager.ActiveProject.AddFileInteractive();
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

    public class AddFile : Action
    {
        public AddFile() : base() { }
        public AddFile(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = IDE.Resources.Images.file_add;
            this.Text = "Add File...";
            this.Enabled = true;
        }
    }

    public class AddModel : Action
    {
        public AddModel() : base() { }
        public AddModel(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.model_add;
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
                ftn.projectRef = Central.MainWindow.CurrentSolution.Projects[0];
            Creation.AddModel(ftn);*/
        }
    }

    public class AddImage : Action
    {
        public AddImage() : base() { }
        public AddImage(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.image_add;
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
                ftn.projectRef = Central.MainWindow.CurrentSolution.Projects[0];
            Creation.AddImage(ftn);*/
        }
    }

    public class AddAudio : Action
    {
        public AddAudio() : base() { }
        public AddAudio(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.audio_add;
            this.Text = "Add Audio...";
            this.Enabled = false;
        }
    }

    public class AddTemplate : Action
    {
        public AddTemplate() : base() { }
        public AddTemplate(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = null; //IDE.Resources.Images.template_add;
            this.Text = "Add Template...";
            this.Enabled = false;
        }
    }

    public class AddArea : Action
    {
        public AddArea() : base() { }
        public AddArea(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.area_add;
            this.Text = "Add Area...";
            this.Enabled = false;
        }
    }

    public class AddWorld : Action
    {
        public AddWorld() : base() { }
        public AddWorld(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.world_add;
            this.Text = "Add World...";
            this.Enabled = false;
        }
    }

    public class AddClass : Action
    {
        public AddClass() : base() { }
        public AddClass(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.class_add;
            this.Text = "Add Class...";
            this.Enabled = false;
        }
    }

    public class AddScript : Action
    {
        public AddScript() : base() { }
        public AddScript(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = IDE.Resources.Images.script_add;
            this.Text = "Add Script...";
            this.Enabled = true;

            // Listen for events.
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.Project)
                (this.Context as Management.Project).AddFileInteractive("ScriptTemplate");
            else if (this.Context is Management.Folder)
                (this.Context as Management.Folder).Project.AddFileInteractive("ScriptTemplate", this.Context as Management.Folder);
            else
                Central.Manager.ActiveProject.AddFileInteractive("ScriptTemplate");
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

    public class AddExistingItem : Action
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
            this.Text = "Add Existing Item...";
            this.Enabled = false;
        }
    }

    public class AddFolder : Action
    {
        public AddFolder() : base() { }
        public AddFolder(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = IDE.Resources.Images.folder;
            this.Text = "Add Folder...";
            this.Enabled = true;

            // Listen for events.
            Central.Manager.SolutionLoaded += new EventHandler(Manager_OnSolutionLoaded);
            Central.Manager.SolutionUnloaded += new EventHandler(Manager_OnSolutionUnloaded);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            if (this.Context is Management.Project)
                (this.Context as Management.Project).AddFileInteractive("FolderTemplate");
            else if (this.Context is Management.Folder)
                (this.Context as Management.Folder).Project.AddFileInteractive("FolderTemplate", this.Context as Management.Folder);
            else
                Central.Manager.ActiveProject.AddFileInteractive("FolderTemplate");
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

    public class AddReference : Action
    {
        public AddReference() : base() { }
        public AddReference(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.reference;
            this.Text = "Add Reference...";
            this.Enabled = false;
        }
    }

    public class ProjProperties : Action
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

    public class ProjDependencies : Action
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

    public class ProjBuildOrder : Action
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

    public class Build : Action
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
            Compilation.ProjectBuilder pb = new Compilation.ProjectBuilder(Central.MainWindow.CurrentSolution.Projects[0], Central.MainWindow.BuildOutput);
            pb.Build(Roket3D.Compilation.BuildMode.DEBUG);
            */
        }
    }

    public class Rebuild : Action
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

    public class Clean : Action
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

    public class StartWithDebug : Action
    {
        public StartWithDebug() : base() { }
        public StartWithDebug(object context) : base(context) { }

        /// <summary>
        /// This event is raied when the menu item is to be initalized.
        /// </summary>
        public override void OnInitialize()
        {
            this.ItemIcon = IDE.Resources.Images.debug;
            this.Text = "Start with Debugging";
            this.Enabled = true;

            // Listen for events.
            Central.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_OnDebugStart);
            Central.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_OnDebugStop);
        }

        /// <summary>
        /// This event is raised when the menu item is clicked or otherwise activated.
        /// </summary>
        public override void OnActivate()
        {
            // FIXME: Fix this so that it uses the context of the project selected.
            if (this.Context != null && this.Context is Management.Project)
                Central.Manager.DebugManager.Start(this.Context as Management.Project);
            else
                Central.Platform.UI.ShowMessage("Unable to start this project with debugging.  Unable to determine the project to run.");
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

    public class SetAsStartupProject : Action
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
