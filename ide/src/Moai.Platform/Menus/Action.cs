using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Moai.Platform.UI;

namespace Moai.Platform.Menus
{
    public abstract class Action : ISyncable
    {
        private Image p_ItemIcon = null;
        private string p_Text = null;
        private bool p_Enabled = false;
        private bool p_Checked = false;
        private bool p_Implemented = true;
        private Keys p_Shortcut = Keys.None;
        private object p_Context = null;
        private ActionUserData p_UserData = new ActionUserData();

        public Action() { }
        public Action(object context) { this.p_Context = context; }
        public virtual void OnInitialize() { }
        public virtual void OnActivate() { }

        /// <summary>
        /// The icon to be used for this action.
        /// </summary>
        public virtual Image ItemIcon
        {
            get
            {
                return this.p_ItemIcon;
            }
            protected internal set
            {
                this.p_ItemIcon = value;
                this.OnSyncDataChanged();
            }
        }

        /// <summary>
        /// The text to be displayed for this action.
        /// </summary>
        public virtual string Text
        {
            get
            {
                return this.p_Text;
            }
            protected internal set
            {
                this.p_Text = value;
                this.OnSyncDataChanged();
            }
        }

        /// <summary>
        /// Whether this action is currently enabled.
        /// </summary>
        public virtual bool Enabled
        {
            get
            {
                return this.p_Enabled;
            }
            protected internal set
            {
                this.p_Enabled = value;
                this.OnSyncDataChanged();
            }
        }

        /// <summary>
        /// Whether this action is currently selected.
        /// </summary>
        public virtual bool Checked
        {
            get
            {
                return this.p_Checked;
            }
            protected internal set
            {
                this.p_Checked = value;
                this.OnSyncDataChanged();
            }
        }

        /// <summary>
        /// Whether this action is currently implemented.
        /// </summary>
        public virtual bool Implemented
        {
            get
            {
                return this.p_Implemented;
            }
            protected internal set
            {
                this.p_Implemented = value;
                this.OnSyncDataChanged();
            }
        }

        /// <summary>
        /// The keyboard shortcut for this action.
        /// </summary>
        public virtual Keys Shortcut
        {
            get
            {
                return this.p_Shortcut;
            }
            protected internal set
            {
                this.p_Shortcut = value;
                this.OnSyncDataChanged();
            }
        }

        /// <summary>
        /// The platform userdata associated with this action.
        /// </summary>
        public virtual object UserData
        {
            get
            {
                return this.p_UserData.Object;
            }
            protected internal set
            {
                this.p_UserData.Object = value;
                this.OnSyncDataChanged();
            }
        }

        /// <summary>
        /// The context data associated with this action.
        /// </summary>
        protected object Context
        {
            get
            {
                return this.p_Context;
            }
            set
            {
                this.p_Context = value;
            }
        }

        #region ISyncable Members

        public event EventHandler SyncDataChanged;

        public ISyncData GetSyncData()
        {
            return new ActionSyncData
            {
                ItemIcon = this.ItemIcon,
                Text = this.Text,
                Enabled = this.Enabled,
                Implemented = this.Implemented,
                Shortcut = this.Shortcut,
                Checked = this.Checked,
                UserData = this.p_UserData
            };
        }

        protected void OnSyncDataChanged()
        {
            if (this.SyncDataChanged != null)
                this.SyncDataChanged(this, new EventArgs());
        }

        public class ActionSyncData : ISyncData
        {
            public Image ItemIcon;
            public string Text;
            public bool Enabled;
            public bool Implemented;
            public Keys Shortcut;
            public bool Checked;
            public ActionUserData UserData;
        }

        public class ActionUserData
        {
            public object Object;
        }

        #endregion
    }
}
