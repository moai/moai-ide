using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai
{
    public interface IDeletable
    {
        bool CanDelete { get; }

        void Delete();
    }
}
