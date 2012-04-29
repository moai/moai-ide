using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Versioning
{
    public class MonoAttribute : BuildSourceAttribute
    {
        public override string Source { get { return "mono"; } }
    }
}
