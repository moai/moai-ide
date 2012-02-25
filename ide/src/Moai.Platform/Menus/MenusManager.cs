using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform;
using Moai.Platform.UI;
using Moai.Platform.Menus;

namespace Moai.Platform.Menus
{
    public class MenusManager : IManager
    {
        private MenuLoader m_Loader = null;

        /// <summary>
        /// Creates a new menu manager.
        /// </summary>
        public MenusManager()
        {
            this.m_Loader = new MenuLoader(this);
        }

        /// <summary>
        /// Retrieves the main menu to be associated with the IDE window.
        /// </summary>
        /// <returns>The main menu to be associated with the IDE window.</returns>
        public IMenuStrip GetMainMenu()
        {
            return this.m_Loader.MainMenu;
        }

        /// <summary>
        /// Retrieves the toolbar to be associated with the IDE window.
        /// </summary>
        /// <returns>The toolbar to be associated with the IDE window.</returns>
        public IToolStrip GetToolBar()
        {
            return this.m_Loader.ToolBar;
        }

        /// <summary>
        /// Wraps the specified action by assigning it's handlers and returning
        /// the menu item related to it.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        /// <returns>The menu item for the action.</returns>
        public static IToolStripMenuItem WrapAction(Action action)
        {
            IToolStripMenuItem mi = Central.Platform.UI.CreateToolStripMenuItem();
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
    }
}
