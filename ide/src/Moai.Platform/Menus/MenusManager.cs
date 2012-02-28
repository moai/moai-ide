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
        /// Retrieves the dynamic action group associated with the main menu system.
        /// </summary>
        public DynamicGroupAction MainMenu
        {
            get
            {
                return this.m_Loader.MainMenu;
            }
        }

        /// <summary>
        /// Retrieves the dynamic action group associated with the toolbar system.
        /// </summary>
        public DynamicGroupAction ToolBar
        {
            get
            {
                return this.m_Loader.ToolBar;
            }
        }
    }
}
