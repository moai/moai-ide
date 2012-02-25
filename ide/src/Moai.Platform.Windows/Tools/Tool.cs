using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DockPanelSuite;
using Moai.Platform.UI;

namespace Moai.Platform.Windows.Tools
{
    public class Tool : DockContent, ITool
	{
        public virtual ToolPosition DefaultState
        {
            get
            {
                return ToolPosition.Document;
            }
        }

        public virtual void OnSolutionLoaded()
        {
        }

        public virtual void OnSolutionUnloaded()
        {
        }

        #region IProxable Members

        public T ProxyAs<T>() where T : class
        {
            throw new NotSupportedException();
        }

        #endregion
    }
}
