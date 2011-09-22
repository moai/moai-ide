using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Operatables
{
    interface ICopyable
    {
        bool CanCopy { get; }

        void Copy();
    }
}
