using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;

namespace Moai.Platform.Tools
{
    public interface IToolsManager : IManager
    {
        ITool Get(Type type);

        void Show(Type type);
    }
}
