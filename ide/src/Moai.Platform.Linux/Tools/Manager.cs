using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Tools;
using Moai.Platform.UI;

namespace Moai.Platform.Linux.Tools
{
    public class Manager : IToolsManager
    {
        List<Tool> m_Windows;

        /// <summary>
        /// Creates a new Manager class for managing the tool windows which
        /// are shown on the screen.
        /// </summary>
        public Manager()
        {
            this.m_Windows = new List<Tool>();
        }

        /// <summary>
        /// Shows the specified tool window by provide it's type.  This function
        /// has no effect if the tool window is already visible.
        /// </summary>
        /// <param name="window">The type of tool window to be shown.</param>
        public void Show(Type window)
        {
            foreach (Tool d in this.m_Windows)
            {
                if (d.GetType() == window)
                    return;
            }

            System.Reflection.ConstructorInfo constructor = null;
            Tool c = null;

            // Construct the new tool window.
            constructor = window.GetConstructor(new Type[] { });
            if (constructor == null)
            {
                constructor = window.GetConstructor(new Type[] { typeof(Tools.Manager) });
                c = constructor.Invoke(new object[] { this }) as Tool;
            }
            else
                c = constructor.Invoke(null) as Tool;

            // Add the solution loaded/unloaded events.
            Central.Manager.SolutionLoaded += new EventHandler((sender, e) =>
                {
                    c.OnSolutionLoaded();
                });
            Central.Manager.SolutionUnloaded += new EventHandler((sender, e) =>
                {
                    c.OnSolutionUnloaded();
                });

            Central.Manager.IDE.ShowDock(c, c.DefaultState);
            this.m_Windows.Add(c);
        }

        /// <summary>
        /// Retrieves the specified tool window by it's type, or returns null if
        /// the tool window does not exist.
        /// </summary>
        /// <param name="window">The type of tool window to be retrieve.</param>
        /// <returns>The tool window, or null if none exists.</returns>
        public ITool Get(Type window)
        {
            foreach (Tool d in this.m_Windows)
            {
                if (d.GetType() == window)
                    return d as ITool;
            }

            return null;
        }
    }
}
