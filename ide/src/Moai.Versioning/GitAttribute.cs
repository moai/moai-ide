using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Versioning
{
    public class GitAttribute : RevisionSourceAttribute
    {
        private string p_Revision;

        public override string Type { get { return "Git"; } }
        public override string Revision { get { return this.p_Revision; } }

        public GitAttribute(string revision)
        {
            this.p_Revision = revision;
        }
    }
}
