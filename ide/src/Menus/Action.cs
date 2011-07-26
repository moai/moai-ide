using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Windows.Forms;
using MOAI.Designers;

namespace MOAI.Menus
{
    public abstract class Action
    {
        private Image p_ItemIcon = null;
        private string p_Text = null;
        private bool p_Enabled = false;
        private bool p_Implemented = true;
        private Keys p_Shortcut = Keys.None;
        private ToolStripMenuItem m_MenuItem = null;
        private ToolStripItem m_Item = null;
        private object m_Context = null;

        public Action() { }
        public Action(object context) { this.m_Context = context; }
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
            protected set
            {
                this.p_ItemIcon = value;
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
            protected set
            {
                this.p_Text = value;
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
            protected set
            {
                this.p_Enabled = value;
                if (this.p_Implemented)
                {
                    if (this.m_Item != null)
                        if (this.m_Item.Owner != null && this.m_Item.Owner.InvokeRequired)
                            this.m_Item.Owner.Invoke(new E(() => { this.m_Item.Enabled = value; }));
                        else
                            this.m_Item.Enabled = value;
                }
                else
                    if (this.m_Item.Owner != null && this.m_Item.Owner.InvokeRequired)
                        this.m_Item.Owner.Invoke(new E(() => { this.m_Item.Enabled = false; }));
                    else
                        this.m_Item.Enabled = false;
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
            protected set
            {
                this.p_Implemented = value;
                if (this.p_Implemented)
                {
                    if (this.m_Item != null)
                        if (this.m_Item.Owner != null && this.m_Item.Owner.InvokeRequired)
                            this.m_Item.Owner.Invoke(new E(() => { this.m_Item.Enabled = value; }));
                        else
                            this.m_Item.Enabled = value;
                }
                else
                    if (this.m_Item.Owner != null && this.m_Item.Owner.InvokeRequired)
                        this.m_Item.Owner.Invoke(new E(() => { this.m_Item.Enabled = false; }));
                    else
                        this.m_Item.Enabled = false;
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
            protected set
            {
                this.p_Shortcut = value;
                if (this.m_MenuItem != null)
                    this.m_MenuItem.ShortcutKeys = value;
            }
        }

        /// <summary>
        /// The menu item associated with this action.
        /// </summary>
        protected ToolStripMenuItem MenuItem
        {
            get
            {
                return this.m_MenuItem;
            }
        }

        /// <summary>
        /// The tool strip item associated with this action.
        /// </summary>
        protected ToolStripItem Item
        {
            get
            {
                return this.m_Item;
            }
        }

        /// <summary>
        /// The context data associated with this action.
        /// </summary>
        protected object Context
        {
            get
            {
                return this.m_Context;
            }
        }

        /// <summary>
        /// Associates this action with the specified menu item and tool strip item.
        /// </summary>
        /// <param name="menuitem">The menu item.</param>
        /// <param name="item">The tool strip item.</param>
        public void SetItem(ToolStripMenuItem menuitem, ToolStripItem item)
        {
            this.m_MenuItem = menuitem;
            this.m_Item = item;
        }
    }
}
