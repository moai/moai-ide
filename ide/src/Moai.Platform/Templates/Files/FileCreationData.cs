using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Templates.Files
{
    public class FileCreationData
    {
        public string Name;
        public BaseTemplate Template;

        public FileCreationData()
        {
        }

        public FileCreationData(string name, BaseTemplate template)
        {
            this.Name = name;
            this.Template = template;
        }
    }
}
