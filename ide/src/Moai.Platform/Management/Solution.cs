using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;
using Moai.Platform.UI;
using Moai.Platform.Menus;

namespace Moai.Platform.Management
{
    public class Solution
    {
        private List<Project> p_Projects = new List<Project>();
        private FileInfo p_SolutionInfo = null;

        /// <summary>
        /// Creates a new instance of the Solution class that is not associated
        /// with any on-disk solution.
        /// </summary>
        public Solution()
        {
            this.p_SolutionInfo = null;
        }

        /// <summary>
        /// Loads a new instance of the Solution class from a file on disk.
        /// </summary>
        /// <param name="file">The solution file to be loaded.</param>
        public Solution(FileInfo file)
        {
            this.p_SolutionInfo = file;

            // TODO: Read the solution data from the file.
        }

        #region Disk Operations

        /// <summary>
        /// Creates a new solution on disk with the specified name in the
        /// specified directory.  The resulting location of the file will
        /// be path\name.msln
        /// </summary>
        /// <param name="name">The name of the solution.</param>
        /// <param name="path">The path to the solution.</param>
        public static Solution Create(string name, string path)
        {
            // Create the directory if it does not exist.
            if (!Directory.Exists(path))
                Directory.CreateDirectory(path);

            // Create a new empty instance.
            Solution s = new Solution();

            // Associate a FileInfo instance with it.
            s.p_SolutionInfo = new FileInfo(Path.Combine(path, name + ".msln"));

            // Request that the solution be saved.
            s.Save();

            // Return the new solution.
            return s;
        }

        /// <summary>
        /// Saves the solution file to disk; this solution must have a
        /// solution file associated with it in order to save it to disk.
        /// </summary>
        public void Save()
        {
            if (this.p_SolutionInfo == null)
                throw new InvalidOperationException();

            // Configure the settings for the XmlWriter.
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Encoding = Encoding.UTF8;
            settings.Indent = true;

            // Create the new XmlWriter.
            XmlWriter writer = XmlWriter.Create(this.p_SolutionInfo.FullName, settings);

            // Generate the XML from the project data.
            writer.WriteStartElement("Solution");

            foreach (Project p in this.p_Projects)
            {
                string rel = PathHelpers.GetRelativePath(this.p_SolutionInfo.DirectoryName, p.ProjectInfo.DirectoryName) + p.ProjectInfo.Name;
                writer.WriteStartElement("Project");
                writer.WriteString(rel);
                writer.WriteEndElement();
            }

            writer.WriteEndElement();
            writer.Close();
        }

        #endregion

        /// <summary>
        /// Converts the solution to a tree node for representation in the Solution Explorer.
        /// </summary>
        /// <returns>A new TreeNode.</returns>
        public ITreeNode ToTreeNode()
        {
            // Construct the tree node.
            ITreeNode node = Central.Platform.UI.CreateTreeNode();
            node.Text = "Solution";
            foreach (Project p in this.p_Projects)
                node.Nodes.Add(p.ToTreeNode());
            node.Tag = this;

            // Create the sub-menu for adding items.
            IToolStripMenuItem addsm = Central.Platform.UI.CreateToolStripMenuItem();
            addsm.Text = "Add";
            addsm.Image = null;
            addsm.Items.AddRange(new IToolStripItem[] {
                MenusManager.WrapAction(new Menus.Definitions.Project.New(this)),
                MenusManager.WrapAction(new Menus.Definitions.Project.Existing(this))
            });

            // Set the context menu for the node.
            node.ContextMenuStrip = Central.Platform.UI.CreateContextMenuStrip();
            node.ContextMenuStrip.Items.AddRange(new IToolStripItem[] {
                    MenusManager.WrapAction(new Menus.Definitions.Solution.Build(this)),
                    MenusManager.WrapAction(new Menus.Definitions.Solution.Rebuild(this)),
                    MenusManager.WrapAction(new Menus.Definitions.Solution.Clean(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Project.ProjDependencies(this)),
                    MenusManager.WrapAction(new Menus.Definitions.Project.ProjBuildOrder(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    addsm,
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Solution.SetStartupProjects(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Paste(this)),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Rename(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.OpenInWindowsExplorer(this)),
                    Central.Platform.UI.CreateToolStripSeperator(),
                    MenusManager.WrapAction(new Menus.Definitions.Actions.Properties(this))
                });

            return node;
        }

        /// <summary>
        /// Returns a list of projects within this solution.
        /// </summary>
        public List<Project> Projects
        {
            get
            {
                return this.p_Projects;
            }
            set
            {
                this.p_Projects = value;
            }
        }

        /// <summary>
        /// The FileInfo object that represents the on-disk solution file for
        /// this solution.
        /// </summary>
        public FileInfo SolutionInfo
        {
            get
            {
                return this.p_SolutionInfo;
            }
        }
    }
}
