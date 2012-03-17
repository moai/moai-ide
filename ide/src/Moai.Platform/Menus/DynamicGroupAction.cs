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

        public DynamicGroupAction()
        {
            this.Base = null;
            this.Text = null;
            this.Enabled = false;
        }

        public DynamicGroupAction(string name)
        {
            this.Base = null;
            this.Text = name;
            this.Enabled = false;
        }

        public DynamicGroupAction(Action baseAction)
        {
            this.Base = baseAction;
        }

        public Action Base
        {
            get;
            private set;
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

        public override void OnInitialize()
        {
            if (this.Base != null)
                this.Base.OnInitialize();
            else
                base.OnInitialize();
        }

        public override void OnActivate()
        {
            if (this.Base != null)
                this.Base.OnActivate();
            else
                base.OnActivate();
        }

        #region Passthrough Properties

        public override string Text
        {
            get
            {
                if (this.Base != null)
                    return this.Base.Text;
                else
                    return base.Text;
            }
            protected internal set
            {
                if (this.Base != null)
                    this.Base.Text = value;
                else
                    base.Text = value;
            }
        }

        public override Image ItemIcon
        {
            get
            {
                if (this.Base != null)
                    return this.Base.ItemIcon;
                else
                    return base.ItemIcon;
            }
            protected internal set
            {
                if (this.Base != null)
                    this.Base.ItemIcon = value;
                else
                    base.ItemIcon = value;
            }
        }

        public override bool Checked
        {
            get
            {
                if (this.Base != null)
                    return this.Base.Checked;
                else
                    return base.Checked;
            }
            protected internal set
            {
                if (this.Base != null)
                    this.Base.Checked = value;
                else
                    base.Checked = value;
            }
        }

        public override bool Enabled
        {
            get
            {
                if (this.Base != null)
                    return this.Base.Enabled;
                else
                    return base.Enabled;
            }
            protected internal set
            {
                if (this.Base != null)
                    this.Base.Enabled = value;
                else
                    base.Enabled = value;
            }
        }

        public override bool Implemented
        {
            get
            {
                if (this.Base != null)
                    return this.Base.Implemented;
                else
                    return base.Implemented;
            }
            protected internal set
            {
                if (this.Base != null)
                    this.Base.Implemented = value;
                else
                    base.Implemented = value;
            }
        }

        public override Moai.Platform.UI.Keys Shortcut
        {
            get
            {
                if (this.Base != null)
                    return this.Base.Shortcut;
                else
                    return base.Shortcut;
            }
            protected internal set
            {
                if (this.Base != null)
                    this.Base.Shortcut = value;
                else
                    base.Shortcut = value;
            }
        }

        #endregion
    }
}
