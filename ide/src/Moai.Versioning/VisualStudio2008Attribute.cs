﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Versioning
{
    public class VisualStudio2008Attribute : BuildSourceAttribute
    {
        public override string Source { get { return "vs2008"; } }
    }
}
