using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Versioning
{
    public abstract class BuildSourceAttribute : Attribute
    {
        public abstract string Source { get; }
    }
}
