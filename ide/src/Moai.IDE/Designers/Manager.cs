using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform;
using Moai.Platform.UI;
using Moai.Platform.Designers;
using Moai.Platform.Management;

namespace Moai.Designers
{
    public class Manager : IDesignerManager
    {
        private List<IDesigner> m_Designers = new List<IDesigner>();

        public event DesignerEventHandler DesignerCreated;
        public event DesignerEventHandler DesignerOpened;
        public event DesignerEventHandler DesignerRefocused;
        public event DesignerEventHandler DesignerClosed;
        public event DesignerEventHandler DesignerChanged;

        /// <summary>
        /// Creates a new designer manager.
        /// </summary>
        public Manager()
        {
            // Listen for tab changes.
            this.DesignerOpened += new DesignerEventHandler((sender, e) =>
            {
                if (Central.Manager.IDE.ActiveTab is IDesigner)
                {
                    if (this.DesignerChanged != null)
                        this.DesignerChanged(this, new DesignerEventArgs(Central.Manager.IDE.ActiveTab as IDesigner));
                }
            });
            Central.Manager.IDEOpened += new EventHandler((_1, _2) =>
            {
                Central.Manager.IDE.ActiveTabChanged += new EventHandler((sender, e) =>
                {
                    if (Central.Manager.IDE.ActiveTab is IDesigner)
                    {
                        if (this.DesignerChanged != null)
                            this.DesignerChanged(this, new DesignerEventArgs(Central.Manager.IDE.ActiveTab as IDesigner));
                    }
                });
            });
        }

        /// <summary>
        /// Fires the OnDesignerChanged event to update any menu items which use file-related
        /// information within their text or designer-related statuses.
        /// </summary>
        public void EmulateDesignerChange()
        {
            if (this.DesignerChanged != null)
                this.DesignerChanged(this, new DesignerEventArgs(Central.Manager.IDE.ActiveTab as IDesigner));
        }

        /// <summary>
        /// Opens a new designer for the specified file, or returns an existing
        /// editor for this file.
        /// </summary>
        /// <param name="file"></param>
        /// <returns></returns>
        public IDesigner OpenDesigner(File file)
        {
            // Search through our existing designers to see if there's
            // already a designer open for this file.
            foreach (IDesigner d in this.m_Designers)
            {
                if (d.File.FileInfo.FullName == file.FileInfo.FullName)
                {
                    if (this.DesignerRefocused != null)
                        this.DesignerRefocused(this, new DesignerEventArgs(d));
                    return d;
                }
            }

            // Ensure that FileInfo property is valid (if it is not, then we
            // are dealing with a non-file).
            if (file.FileInfo == null)
                return null;

            // Detect the type of designer to create by the file's extension.
            Type t = Associations.GetDesignerType(file.FileInfo.Extension.Substring(1));
            if (t == null)
            {
                Central.Platform.UI.ShowMessage("There is no designer associated with this file type.", "No Designer", MessageBoxButtons.OK);
                return null;
            }

            // Invoke the constructor.
            IDesigner ds = t.GetConstructor(new Type[] { typeof(File) }).Invoke(new object[] { file as object }) as IDesigner;
            ds.Closed += (sender, e) =>
                {
                    if (this.DesignerClosed != null)
                        this.DesignerClosed(this, new DesignerEventArgs(ds));
                    this.m_Designers.Remove(ds);
                };
            ds.Opened += (sender, e) =>
                {
                    if (this.DesignerOpened != null)
                        this.DesignerOpened(this, new DesignerEventArgs(ds));
                };
            this.m_Designers.Add(ds);
            if (this.DesignerCreated != null)
                this.DesignerCreated(this, new DesignerEventArgs(ds));

            return ds;
        }
    }
}
