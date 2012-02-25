using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Moai.Platform.Management;

namespace Moai.Platform.Templates.Solutions
{
    public abstract class BaseTemplate
    {
        public abstract string TemplateName { get; }
        public abstract string TemplateDescription { get; }
        public abstract Bitmap TemplateIcon { get; }

        public abstract Solution Create(SolutionCreationData data);
    }
}
