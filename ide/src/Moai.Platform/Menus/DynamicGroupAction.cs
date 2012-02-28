using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Collections.ObjectModel;
using System.Drawing;

namespace Moai.Platform.Menus
{
    public class DynamicGroupAction : Action
    {
        private List<Action> p_Actions = new List<Action>();

        public DynamicGroupAction(string name, Image icon)
        {
            this.Text = name;
            this.ItemIcon = icon;
            this.Enabled = false;
        }

        public ReadOnlyCollection<Action> Actions
        {
            get
            {
                return this.p_Actions.AsReadOnly();
            }
        }

        public void Add(Action action)
        {
            this.p_Actions.Add(action);
            this.Enabled = true;
        }
    }
}
