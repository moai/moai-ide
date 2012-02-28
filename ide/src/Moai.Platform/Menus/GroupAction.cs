using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;

namespace Moai.Platform.Menus
{
    public class GroupAction : Action
    {
        public GroupAction(string name, Image icon, Action[] actions)
        {
            this.Text = name;
            this.ItemIcon = icon;
            this.Actions = actions;
            this.Enabled = true;
        }

        public Action[] Actions
        {
            get;
            private set;
        }
    }
}
