using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Versioning
{
    public abstract class BuildSourceAttribute : Attribute
    {
        public abstract string Source { get; }
    }
}
