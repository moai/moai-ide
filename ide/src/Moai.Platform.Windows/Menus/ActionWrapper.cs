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
        private static T WrapAction<T>(Moai.Platform.Menus.Action action) where T : ToolStripItem, new()
        {
            T mi = new T();
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

        private static void ActionSyncDataChanged(Moai.Platform.Menus.Action.ActionSyncData data, ToolStripItem mi)
        {
            // Set properties.
            System.Action act = () =>
                {
                    if (mi is ToolStripMenuItem && !(mi is ToolStripDropDownButton))
                    {
                        mi.Text = data.Text;
                        (mi as ToolStripMenuItem).ShortcutKeys = KeyUtil.FromPlatform(data.Shortcut);
                        (mi as ToolStripMenuItem).ShowShortcutKeys = false;
                    }
                    else
                        mi.ToolTipText = data.Text;
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
                    ctx.Items.Add(ActionWrapper.WrapAction<ToolStripMenuItem>(a));
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
                    ms.Items.Add(ActionWrapper.WrapAction<ToolStripMenuItem>(a));
            }
            return ms;
        }

        private static ToolStripMenuItem GetMenuItems(DynamicGroupAction group)
        {
            ToolStripMenuItem mi = ActionWrapper.WrapAction<ToolStripMenuItem>(group);
            mi.DropDown.Items.Clear();
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                    mi.DropDown.Items.Add(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    mi.DropDown.Items.Add("-");
                else
                    mi.DropDown.Items.Add(ActionWrapper.WrapAction<ToolStripMenuItem>(a));
            }
            return mi;
        }

        internal static ToolStrip GetToolBar(DynamicGroupAction group)
        {
            ToolStrip ts = new ToolStrip();
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                    ts.Items.Add(ActionWrapper.GetToolItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    ts.Items.Add("-");
                else
                    ts.Items.Add(ActionWrapper.WrapAction<ToolStripButton>(a));
            }
            return ts;
        }

        private static ToolStripDropDownButton GetToolItems(DynamicGroupAction group)
        {
            ToolStripDropDownButton mi = ActionWrapper.WrapAction<ToolStripDropDownButton>(group);
            mi.DropDown.Items.Clear();
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                    mi.DropDown.Items.Add(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    mi.DropDown.Items.Add("-");
                else
                    mi.DropDown.Items.Add(ActionWrapper.WrapAction<ToolStripMenuItem>(a));
            }
            return mi;
        }
    }
}
