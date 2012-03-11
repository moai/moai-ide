using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace Moai.Platform.Linux
{
    public partial class LinuxIDE
    {
        /// <summary>
        /// Method for replacing the existing menu bar.
        /// </summary>
        private void SetMainMenu(MenuBar menu)
        {
            this.c_MenuBox.Remove(this.c_MainMenu);
            this.c_MainMenu = menu;
            this.c_MenuBox.PackStart(this.c_MainMenu, false, true, 0);
            this.c_MenuBox.ReorderChild(this.c_MainMenu, 0);
        }

        /// <summary>
        /// Method for replacing the existing toolbar.
        /// </summary>
        private void SetToolBar(Toolbar toolbar)
        {
            this.c_MenuBox.Remove(this.c_ToolBar);
            this.c_ToolBar = toolbar;
            this.c_MenuBox.PackStart(this.c_ToolBar, false, true, 0);
            this.c_MenuBox.ReorderChild(this.c_ToolBar, 1);
        }

        /// <summary>
        /// Required method for initializing the IDE structure.
        /// </summary>
        private void InitializeComponent()
        {
            this.c_MenuBox = new VBox();
            this.c_MainMenu = new MenuBar();
            this.c_ToolBar = new Toolbar();
            this.c_FrameBox = new HBox();
            this.c_DocumentBox = new VBox();
            this.c_DocumentNotebook = new Notebook();
            this.c_BottomTools = new Notebook();
            this.c_RightTools = new Notebook();
            //
            // c_DocumentNotebook
            //
            this.c_DocumentNotebook.AppendPage(new TextView(), new Label("Test"));
            //
            // c_MenuBox
            //
            this.c_MenuBox.PackStart(this.c_MainMenu, false, true, 0);
            this.c_MenuBox.PackStart(this.c_ToolBar, false, true, 0);
            this.c_MenuBox.PackStart(this.c_FrameBox, true, true, 0);
            //
            // c_FrameBox
            //
            this.c_FrameBox.PackStart(this.c_DocumentBox, true, true, 0);
            this.c_FrameBox.PackStart(this.c_RightTools, false, true, 0);
            //
            // c_DocumentBox
            //
            this.c_DocumentBox.PackStart(this.c_DocumentNotebook, true, true, 0);
            this.c_DocumentBox.PackStart(this.c_BottomTools, false, true, 0);
            //
            // IDE
            //
            this.Add(this.c_MenuBox);
            this.DeleteEvent += (sender, e) => { this.Exit(); };
            this.Shown += new EventHandler(IDE_Shown);
        }

        VBox c_MenuBox = null;
        MenuBar c_MainMenu = null;
        Toolbar c_ToolBar = null;
        HBox c_FrameBox = null;
        Notebook c_RightTools = null;
        Notebook c_BottomTools = null;
        VBox c_DocumentBox = null;
        Notebook c_DocumentNotebook = null;
    }
}
