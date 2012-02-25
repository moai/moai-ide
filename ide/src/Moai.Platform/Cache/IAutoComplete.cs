using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Cache
{
    public interface IAutoComplete
    {
        List<string> GetList(string s);
    }
}
