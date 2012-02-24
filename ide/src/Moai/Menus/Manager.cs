using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Moai.Menus
{
    public class Manager
    {
        private Moai.Manager p_Parent = null;
        private MenuLoader m_Loader = null;

        /// <summary>
        /// Creates a new menu manager.
        /// </summary>
        public Manager(Moai.Manager parent)
        {
            this.p_Parent = parent;
            this.m_Loader = new MenuLoader(this);
        }

        /// <summary>
        /// Retrieves the main menu to be associated with the IDE window.
        /// </summary>
        /// <returns>The main menu to be associated with the IDE window.</returns>
        public MenuStrip GetMainMenu()
        {
            return this.m_Loader.MainMenu;
        }

        /// <summary>
        /// Retrieves the toolbar to be associated with the IDE window.
        /// </summary>
        /// <returns>The toolbar to be associated with the IDE window.</returns>
        public ToolStrip GetToolBar()
        {
            return this.m_Loader.ToolBar;
        }

        /// <summary>
        /// Wraps the specified action by assigning it's handlers and returning
        /// the menu item related to it.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        /// <returns>The menu item for the action.</returns>
        public static ToolStripMenuItem WrapAction(Action action)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem();
            action.SetItem(mi, mi);
            action.OnInitialize();
            mi.Text = action.Text;
            mi.ShortcutKeys = action.Shortcut;
            mi.ShowShortcutKeys = false;
            mi.Enabled = action.Enabled;
            if (action.ItemIcon != null)
                mi.Image = action.ItemIcon;
            mi.Click += new EventHandler((sender, e) => { action.OnActivate(); });
            return mi;
        }

        /// <summary>
        /// The main Moai manager that owns this menu manager.
        /// </summary>
        public Moai.Manager Parent
        {
            get
            {
                return this.p_Parent;
            }
        }
    }
}
