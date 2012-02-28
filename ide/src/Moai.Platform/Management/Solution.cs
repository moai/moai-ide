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
    public class Solution : ISyncable
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
        /// Returns the context menu for this project.
        /// </summary>
        public virtual Moai.Platform.Menus.Action[] ContextActions
        {
            get
            {
                // Create the context action list.
                return new Moai.Platform.Menus.Action[]
                {
                    new Menus.Definitions.Solution.Build(this),
                    new Menus.Definitions.Solution.Rebuild(this),
                    new Menus.Definitions.Solution.Clean(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Project.ProjDependencies(this),
                    new Menus.Definitions.Project.ProjBuildOrder(this),
                    new SeperatorAction(),
                    new GroupAction("Add", null, new Moai.Platform.Menus.Action[] {
                        new Menus.Definitions.Project.New(this),
                        new Menus.Definitions.Project.Existing(this)
                    }),
                    new SeperatorAction(),
                    new Menus.Definitions.Solution.SetStartupProjects(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.Paste(this),
                    new Menus.Definitions.Actions.Rename(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.OpenInWindowsExplorer(this),
                    new SeperatorAction(),
                    new Menus.Definitions.Actions.Properties(this)
                };
            }
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

        #region ISyncable Members

        public event EventHandler SyncDataChanged;

        public ISyncData GetSyncData()
        {
            return new FileSyncData { Text = (this.SolutionInfo != null) ? this.SolutionInfo.Name : "Solution", ImageKey = "solution" };
        }

        #endregion
    }
}
