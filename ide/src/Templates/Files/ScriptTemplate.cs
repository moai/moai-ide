using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using MOAI.Management;
using System.IO;

namespace MOAI.Templates.Files
{
    public class ScriptTemplate : BaseTemplate
    {
        public override string TemplateName
        {
            get { return "Empty Script"; }
        }

        public override string TemplateDescription
        {
            get { return "an empty Lua script"; }
        }

        public override string TemplateExtension
        {
            get { return "lua"; }
        }

        public override Bitmap TemplateIcon
        {
            get { return null; }
        }

        /// <summary>
        /// Creates a new empty Lua script in the specified project, in the specified folder.
        /// </summary>
        /// <param name="data">The solution creation data, usually derived from the user's input in a NewSolutionForm.</param>
        /// <returns>A new solution that can be loaded.</returns>
        public override MOAI.Management.File Create(string name, Project project, Folder folder)
        {
            MOAI.Management.File f = null;

            // Determine where to place the file.
            if (folder == null)
            {
                // Place the file in the root of the project
                f = new MOAI.Management.File(project, project.ProjectInfo.DirectoryName, name);
                project.AddFile(f);
            }
            else
            {
                // Place the file in the specified folder
                f = new MOAI.Management.File(project, project.ProjectInfo.DirectoryName, Path.Combine(folder.FolderInfo.Name, name));
                folder.Add(f);
            }

            // Write the contents of the data into the file.
            using (StreamWriter writer = new StreamWriter(f.FileInfo.FullName))
            {
                writer.WriteLine("-- An empty script file.");
                writer.WriteLine();
            }

            return f;
        }
    }
}
