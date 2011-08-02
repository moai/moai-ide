using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MOAI.Collections;
using System.IO;
using System.Drawing;
using System.Windows.Forms;

namespace MOAI.Management
{
    public class File : System.Windows.Forms.TreeNode
    {
        private Project p_Project = null;
        private FileInfo p_FileInfo = null;

        /// <summary>
        /// A protected constructor so that derived classes such as Folder only have
        /// to provide the project as an argument to the constructor.
        /// </summary>
        /// <param name="p">The project that owns this folder.</param>
        protected File(Project p)
        {
            this.p_Project = p;
        }

        /// <summary>
        /// Creates a new File object based on an relative path and a Project object.
        /// </summary>
        /// <param name="p">The project that owns this folder.</param>
        /// <param name="parent">The parent director (the directory the project file is located in).</param>
        /// <param name="relpath">The relative path to the file.</param>
        public File(Project p, string parent, string relpath)
        {
            if (p != null && parent != null)
            {
                this.p_Project = p;
                this.p_FileInfo = new FileInfo(Path.Combine(parent, relpath));
            }
            else if (p == null && parent == null)
            {
                this.p_Project = null;
                this.p_FileInfo = new FileInfo(relpath);
            }
            else
                throw new NotSupportedException();
        }

        /// <summary>
        /// Associates this file with a tree node.
        /// </summary>
        /// <param name="node"></param>
        public virtual void Associate(System.Windows.Forms.TreeNode node)
        {
            // Set properties.
            this.Text = this.ToString();
            this.ImageKey = Associations.GetImageKey(this.p_FileInfo.Extension.Substring(1));
            this.SelectedImageKey = this.ImageKey;

            // Add this file to the node.
            node.Nodes.Add(this);
        }

        /// <summary>
        /// Informs the developer that the Name property would actually provide the control's name, not
        /// the name of the file or the folder.
        /// </summary>
        public new string Name
        {
            get { throw new InvalidOperationException("The name property should not be used as it indicates the control's name, not the name of " +
"the file or folder.  Use the FileInfo property to access the file information, or ControlName to access " +
"the control's name."); }
        }

        /// <summary>
        /// Provides access to the Name property if desired.
        /// </summary>
        public string ControlName
        {
            get { return base.Name; }
            set { base.Name = value; }
        }

        /// <summary>
        /// The FileInfo object that represents this file on-disk.
        /// </summary>
        public FileInfo FileInfo
        {
            get
            {
                return this.p_FileInfo;
            }
        }

        /// <summary>
        /// The project that owns this file.
        /// </summary>
        public Project Project
        {
            get
            {
                return this.p_Project;
            }
        }

        /// <summary>
        /// Returns the context menu for this file.
        /// </summary>
        public override ContextMenuStrip ContextMenuStrip
        {
            get
            {
                // Set the context menu for the node.
                ContextMenuStrip ret = new ContextMenuStrip();
                ret.Items.AddRange(new ToolStripItem[] {
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Open(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Views.Code(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Views.Designer(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Exclude(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Cut(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Copy(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Paste(this)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Rename(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.OpenInWindowsExplorer(this)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Properties(this))
                });
                return ret;
            }
        }

        /// <summary>
        /// Overrides the ToString() function which is used by the tree view
        /// so that it displays the file name.
        /// </summary>
        public override string ToString()
        {
            return this.FileInfo.Name;
        }
    }
}
