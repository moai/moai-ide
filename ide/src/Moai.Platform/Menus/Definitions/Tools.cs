using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Menus.Definitions.Tools
{
    class BuildOutput : Action
    {
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.tools_build_output;
            this.Text = "Build Output";
            this.Enabled = false;
        }
    }

    class ErrorList : Action
    {
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.tools_error_list;
            this.Text = "Error List";
            this.Enabled = false;
        }
    }

    class PropertiesWindow : Action
    {
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.tools_properties_window;
            this.Text = "Properties Window";
            this.Enabled = false;
        }
    }

    class SolutionExplorer : Action
    {
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.tools_solution_explorer;
            this.Text = "Solution Explorer";
            this.Enabled = false;
        }
    }

    class LuaConsole : Action
    {
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.tools_console;
            this.Text = "Lua Console";
            this.Enabled = false;
        }
    }

    class ObjectBrowser : Action
    {
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.tools_object_browser;
            this.Text = "Object Browser";
            this.Enabled = false;
        }
    }

    class Toolbox : Action
    {
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.tools_toolbox;
            this.Text = "Toolbox";
            this.Enabled = false;
        }
    }

    class Manage : Action
    {
        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.tools_addons_manage;
            this.Text = "Manage Tools";
            this.Enabled = false;
        }
    }
}
