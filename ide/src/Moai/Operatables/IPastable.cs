using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Operatables
{
    interface IPastable
    {
        bool CanPaste { get; }

        void Paste();
    }
}
