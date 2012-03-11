using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DockPanelSuite;
using Moai.Platform;
using Moai.Platform.Management;
using Moai.Platform.Designers;

namespace Moai.Designers
{
    public class Designer : DockContent, IDesigner
    {
        public Designer()
        {
        }

        /// <summary>
        /// Creates a new Designer base object.
        /// </summary>
        /// <param name="file">The associated file.</param>
        public Designer(File file)
        {
            this.File = file;
            if (this.File != null)
                this.TabText = this.File.FileInfo.Name;
        }

        /// <summary>
        /// The path to the file that this editor is currently editing.
        /// </summary>
        public string Path
        {
            get;
            protected set;
        }

        /// <summary>
        /// The File object that represents the file that this editor is currently editing.
        /// Only valid if the Path points to a file that is currently in the project.
        /// </summary>
        public File File
        {
            get;
            protected set;
        }

        #region IDesigner Members

        public event EventHandler Opened
        {
            add
            {
                this.Shown += value;
            }
            remove
            {
                this.Shown -= value;
            }
        }

        #endregion
    }
}
