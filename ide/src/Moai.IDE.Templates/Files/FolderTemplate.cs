using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Drawing;
using Moai.Platform.Management;

namespace Moai.IDE.Templates.Files
{
    public class FolderTemplate : BaseTemplate
    {
        public override string TemplateName
        {
            get { return "Empty Folder"; }
        }

        public override string TemplateDescription
        {
            get { return "an empty folder for storing files"; }
        }

        public override string TemplateExtension
        {
            get { return ""; }
        }

        public override Bitmap TemplateIcon
        {
            get { return null; }
        }

        /// <summary>
        /// Creates a new empty folder in the specified project, in the specified folder.
        /// </summary>
        /// <param name="data">The solution creation data, usually derived from the user's input in a NewSolutionForm.</param>
        /// <returns>A new solution that can be loaded.</returns>
        public override Moai.Platform.Management.File Create(string name, Project project, Folder folder)
        {
            Folder ff = null;

            // Determine where to place the folder.
            if (folder == null)
            {
                // Place the folder in the root of the project
                ff = new Folder(project, project.ProjectInfo.DirectoryName, name);
                project.AddFile(ff);
            }
            else
            {
                // Place the folder in the specified folder
                ff = new Folder(project, project.ProjectInfo.DirectoryName, Path.Combine(folder.FolderInfo.Name, name));
                folder.Add(ff);
            }

            // Create the new folder on disk.
            if (!Directory.Exists(ff.FolderInfo.FullName))
                Directory.CreateDirectory(ff.FolderInfo.FullName);

            return ff;
        }
    }
}
