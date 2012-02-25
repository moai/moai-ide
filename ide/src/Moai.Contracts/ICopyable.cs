using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai
{
    public interface ICopyable
    {
        bool CanCopy { get; }

        void Copy();
    }
}
