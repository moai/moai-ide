using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.UI
{
    public interface IProxable
    {
        T ProxyAs<T>() where T : class;
    }
}
