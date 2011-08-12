using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DockPanelSuite;
using MOAI.Management;

namespace MOAI.Designers
{
    public class Designer : DockContent
    {
        private MOAI.Manager p_Manager = null;
        private string p_Path = null;
        private MOAI.Management.File p_File = null;
        private bool p_CanSave = false;

        public Designer()
        {
        }

        /// <summary>
        /// Creates a new Designer base object.
        /// </summary>
        /// <param name="manager">The main MOAI manager object.</param>
        /// <param name="file">The associated file.</param>
        public Designer(MOAI.Manager manager, File file)
        {
            this.p_Manager = manager;
            this.p_File = file;
            if (this.p_File != null)
                this.TabText = this.p_File.FileInfo.Name;
        }

        /// <summary>
        /// This function is called to tell the designer that it should save the file to disk.
        /// </summary>
        public virtual void OnSaveFile() { }

        /// <summary>
        /// This function is called to tell the designer that it should save the file to disk as another name.
        /// </summary>
        public virtual void OnSaveFileAs() { }

        /// <summary>
        /// The path to the file that this editor is currently editing.
        /// </summary>
        public string Path
        {
            get
            {
                return this.p_Path;
            }
            protected set
            {
                this.p_Path = value;
            }
        }

        /// <summary>
        /// The File object that represents the file that this editor is currently editing.
        /// Only valid if the Path points to a file that is currently in the project.
        /// </summary>
        public MOAI.Management.File File
        {
            get
            {
                return this.p_File;
            }
            protected set
            {
                this.p_File = value;
            }
        }

        /// <summary>
        /// The main manager in the application.
        /// </summary>
        protected MOAI.Manager Manager
        {
            get
            {
                return this.p_Manager;
            }
            private set
            {
                this.p_Manager = value;
            }
        }

        /// <summary>
        /// Whether the save operation is valid on this editor.
        /// </summary>
        public bool CanSave
        {
            get
            {
                return this.p_CanSave;
            }
            protected set
            {
                this.p_CanSave = value;
            }
        }
    }
}
