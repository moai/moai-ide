using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Menus;
using Moai.Platform.Linux.UI;
using Qyoto;
using log4net;

namespace Moai.Platform.Linux.Menus
{
    public static class ActionWrapper
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(ActionWrapper));

        private class QSyncableAction : QAction
        {
            public Moai.Platform.Menus.Action Target
            {
                get;
                private set;
            }

            public QSyncableAction(Moai.Platform.Menus.Action target) : base((QObject)null)
            {
                this.Target = target;
                this.Connect(this, SIGNAL("triggered(bool)"), SLOT("OnTriggered(bool)"));
            }

            [Q_SLOT]
            public void Resync()
            {
                if (this.Target == null)
                    return;
                Moai.Platform.Menus.Action.ActionSyncData data = this.Target.GetSyncData() as Moai.Platform.Menus.Action.ActionSyncData;

                this.Text = data.Text;
                //this.ReleaseShortcut((int)data.UserData.Object);
                //data.UserData.Object = this.GrabShortcut(KeyUtil.FromPlatform(data.Shortcut));
                this.Enabled = data.Enabled && data.Implemented;
                if (data.ItemIcon != null)
                {
                    this.icon = LinuxImageList.ConvertToQIcon(data.ItemIcon);
                    this.IconVisibleInMenu = true;
                }
                else
                    this.IconVisibleInMenu = false;
            }

            [Q_SLOT]
            public void OnTriggered(bool isChecked)
            {
                if (this.Target == null)
                    return;
                this.Target.OnActivate();
            }
        }

        /// <summary>
        /// Wraps the specified action by assigning it's handlers and returning
        /// the menu item related to it.
        /// </summary>
        /// <param name="action">The action to wrap.</param>
        /// <returns>The menu item for the action.</returns>
        private static QAction WrapAction(Moai.Platform.Menus.Action action)
        {
            m_Log.Debug("Wrapping action " + action.GetType().FullName + ".");
            QSyncableAction mi = new QSyncableAction(action);
            LinuxNativePool.Instance.Retain(mi);
            if (action == null)
            {
                mi.Text = "ERROR! UNKNOWN ACTION";
                mi.Enabled = false;
                return mi;
            }
            action.SyncDataChanged += (sender, e) =>
            {
                mi.Resync();
            };
            action.OnInitialize();
            mi.Resync();
            return mi;
        }

        public static QMenu GetContextMenu(Moai.Platform.Menus.Action[] actions)
        {
            m_Log.Debug("Getting context menu.");
            QMenu ctx = new QMenu();
            LinuxNativePool.Instance.Retain(ctx);
            foreach (Moai.Platform.Menus.Action a in actions)
            {
                if (a is DynamicGroupAction)
                    ctx.AddMenu(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                else if (a is SeperatorAction)
                    ctx.AddSeparator();
                else
                    ctx.AddAction(ActionWrapper.WrapAction(a));
            }
            return ctx;
        }

        internal static QMenuBar GetMainMenu(DynamicGroupAction group)
        {
            m_Log.Debug("Getting main menu for " + group.Text + ".");
            QMenuBar ms = new QMenuBar();
            LinuxNativePool.Instance.Retain(ms);
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                {
                    m_Log.Debug("Recursive menu add for " + a.GetType().FullName + ".");
                    ms.AddMenu(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                }
                else if (a is SeperatorAction)
                    ms.AddSeparator();
                else
                {
                    m_Log.Debug("Normal menu add for " + a.GetType().FullName + ".");
                    ms.AddAction(ActionWrapper.WrapAction(a));
                }
            }
            return ms;
        }

        private static QMenu GetMenuItems(DynamicGroupAction group)
        {
            m_Log.Debug("Getting menu items for " + group.Text + ".");
            QMenu ctx = new QMenu(group.GetType().FullName);
            LinuxNativePool.Instance.Retain(ctx);
            foreach (Moai.Platform.Menus.Action a in group.Actions)
            {
                if (a is DynamicGroupAction)
                {
                    m_Log.Debug("Recursive menu add for " + a.GetType().FullName + ".");
                    ctx.AddMenu(ActionWrapper.GetMenuItems(a as DynamicGroupAction));
                }
                else if (a is SeperatorAction)
                    ctx.AddSeparator();
                else
                {
                    m_Log.Debug("Normal menu add for " + a.GetType().FullName + ".");
                    ctx.AddAction(ActionWrapper.WrapAction(a));
                }
            }
            ctx.Title = group.Text;
            return ctx;
        }

        internal static QToolBar GetToolBar(DynamicGroupAction group)
        {
            return new QToolBar();
        }
    }
}
