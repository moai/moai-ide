using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Operatables
{
    interface ICuttable
    {
        bool CanCut { get; }

        void Cut();
    }
}
