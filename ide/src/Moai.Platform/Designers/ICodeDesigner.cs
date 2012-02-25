using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Debug;

namespace Moai.Platform.Designers
{
    public interface ICodeDesigner : IDesigner, ICuttable, ICopyable, IPastable, IDeletable, ISavable, IDebuggable
    {
    }
}
