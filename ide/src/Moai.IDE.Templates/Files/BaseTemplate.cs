using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Moai.Platform.Management;

namespace Moai.IDE.Templates.Files
{
    public abstract class BaseTemplate
    {
        public abstract string TemplateName { get; }
        public abstract string TemplateDescription { get; }
        public abstract string TemplateExtension { get; }
        public abstract Bitmap TemplateIcon { get; }

        public abstract File Create(string name, Project project, Folder folder);
    }
}
