using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Templates.Files
{
    public struct FileCreationData
    {
        public string Name;
        public BaseTemplate Template;

        public FileCreationData(string name, BaseTemplate template)
        {
            this.Name = name;
            this.Template = template;
        }
    }
}
