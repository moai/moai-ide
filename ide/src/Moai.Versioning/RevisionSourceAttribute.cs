using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Versioning
{
    public abstract class RevisionSourceAttribute : Attribute
    {
        public abstract string Type { get; }
        public abstract string Revision { get; }
    }
}
