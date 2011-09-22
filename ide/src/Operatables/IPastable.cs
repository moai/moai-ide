using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Operatables
{
    interface IPastable
    {
        bool CanPaste { get; }

        void Paste();
    }
}
