using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using Moai.Platform.Menus;
using Moai.Platform.Linux.UI;

namespace Moai.Platform.Linux.Menus
{
    public static class ActionWrapper
    {
        /// <summary>
        /// Wraps the specified action by assigning it's handlers and returning
        /// the menu item related to it.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        /// <returns>The menu item for the action.</returns>
        private static MenuItem WrapAction(Moai.Platform.Menus.Action action)
        {
            ImageMenuItem mi = new ImageMenuItem("ERROR! UNSET MENU ITEM");
            if (action == null)
            {
                (mi.Child as Label).Text = "ERROR! UNKNOWN ACTION";
                mi.Sensitive = false;
                return mi;
            }
            action.SyncDataChanged += (sender, e) =>
            {
                ActionWrapper.ActionSyncDataChanged(action.GetSyncData() as Moai.Platform.Menus.Action.ActionSyncData, mi);
            };
            action.OnInitialize();
            ActionWrapper.ActionSyncDataChanged(action.GetSyncData() as Moai.Platform.Menus.Action.ActionSyncData, mi);
            mi.Activated += new EventHandler((sender, e) => { action.OnActivate(); });
            return mi;
        }

        private static void ActionSyncDataChanged(Moai.Platform.Menus.Action.ActionSyncData data, ImageMenuItem mi)
        {
            // Set properties.
            System.Action act = () =>
            {
                (mi.Child as AccelLabel).Text = data.Text;
                //mi.ShortcutKeys = KeyUtil.FromPlatform(data.Shortcut);
                //mi.ShowShortcutKeys = false;
                mi.Sensitive = data.Enabled && data.Implemented;
                //mi.Image = LinuxImageList.ConvertToGtk(data.ItemIcon);
            };
            /*if (mi.Parent != null && (mi.Window.InvokeRequired)
                mi.Parent.Invoke(act);
            else*/
                act();
        }

        public static Menu GetContextMenu(Moai.Platform.Menus.Action[] actions)
        {
            Menu ctx = new Menu();
            foreach (Moai.Platform.Menus.Action a in actions)
            {
                if (a is DynamicGroupAction)
                    ctx.Append(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    ctx.Append(new MenuItem());
                else
                    ctx.Append(ActionWrapper.WrapAction(a));
            }
            return ctx;
        }

        internal static MenuBar GetMainMenu(DynamicGroupAction group)
        {
            MenuBar ms = new MenuBar();
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                    ms.Append(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    ms.Append(new MenuItem());
                else
                    ms.Append(ActionWrapper.WrapAction(a));
            }
            return ms;
        }

        private static MenuItem GetMenuItems(DynamicGroupAction group)
        {
            Menu ctx = new Menu();
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                    ctx.Append(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    ctx.Append(new MenuItem());
                else
                    ctx.Append(ActionWrapper.WrapAction(a));
            }
            MenuItem trigger = new MenuItem(group.Text);
            trigger.Submenu = ctx;
            return trigger;
        }

        internal static Toolbar GetToolBar(DynamicGroupAction group)
        {
            return new Toolbar();
        }
    }
}
