using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moai.Platform.Menus;
using Moai.Platform.Windows.UI;

namespace Moai.Platform.Windows.Menus
{
    public static class ActionWrapper
    {
        /// <summary>
        /// Wraps the specified action by assigning it's handlers and returning
        /// the menu item related to it.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        /// <returns>The menu item for the action.</returns>
        private static ToolStripMenuItem WrapAction(Moai.Platform.Menus.Action action)
        {
            ToolStripMenuItem mi = new ToolStripMenuItem();
            if (action == null)
            {
                mi.Text = "ERROR! UNKNOWN ACTION";
                mi.Enabled = false;
                return mi;
            }
            action.SyncDataChanged += (sender, e) =>
                {
                    ActionWrapper.ActionSyncDataChanged(action.GetSyncData() as Moai.Platform.Menus.Action.ActionSyncData, mi);
                };
            action.OnInitialize();
            ActionWrapper.ActionSyncDataChanged(action.GetSyncData() as Moai.Platform.Menus.Action.ActionSyncData, mi);
            mi.Click += new EventHandler((sender, e) => { action.OnActivate(); });
            return mi;
        }

        private static void ActionSyncDataChanged(Moai.Platform.Menus.Action.ActionSyncData data, ToolStripMenuItem mi)
        {
            // Set properties.
            System.Action act = () =>
                {
                    mi.Text = data.Text;
                    mi.ShortcutKeys = KeyUtil.FromPlatform(data.Shortcut);
                    mi.ShowShortcutKeys = false;
                    mi.Enabled = data.Enabled && data.Implemented;
                    if (data.ItemIcon != null)
                        mi.Image = data.ItemIcon;
                };
            if (mi.Owner != null && mi.Owner.InvokeRequired)
                mi.Owner.Invoke(act);
            else
                act();
        }

        public static ContextMenuStrip GetContextMenu(Moai.Platform.Menus.Action[] actions)
        {
            ContextMenuStrip ctx = new ContextMenuStrip();
            foreach (Moai.Platform.Menus.Action a in actions)
            {
                if (a is DynamicGroupAction)
                    ctx.Items.Add(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    ctx.Items.Add("-");
                else
                    ctx.Items.Add(ActionWrapper.WrapAction(a));
            }
            return ctx;
        }

        internal static MenuStrip GetMainMenu(DynamicGroupAction group)
        {
            MenuStrip ms = new MenuStrip();
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                    ms.Items.Add(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    ms.Items.Add("-");
                else
                    ms.Items.Add(ActionWrapper.WrapAction(a));
            }
            return ms;
        }

        private static ToolStripItem GetMenuItems(DynamicGroupAction group)
        {
            ToolStripMenuItem mi = ActionWrapper.WrapAction(group);
            mi.DropDown.Items.Clear();
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                    mi.DropDown.Items.Add(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    mi.DropDown.Items.Add("-");
                else
                    mi.DropDown.Items.Add(ActionWrapper.WrapAction(a));
            }
            return mi;
        }

        internal static ToolStrip GetToolBar(DynamicGroupAction group)
        {
            return new ToolStrip();
        }
    }
}
