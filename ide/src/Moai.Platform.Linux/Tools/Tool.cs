using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;
using Qyoto;

namespace Moai.Platform.Linux.Tools
{
    public class Tool : QDockWidget, ITool
    {
        public Tool()
            : base("Docked Area")
        {
        }

        public string Title
        {
            get
            {
                return base.WindowTitle;
            }
            protected set
            {
                base.WindowTitle = value;
            }
        }

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

        #region IHasInvoke Members

        public object Invoke(Delegate method)
        {
            return this.Invoke(method);
        }

        public bool InvokeRequired
        {
            get { return this.InvokeRequired; }
        }

        #endregion
    }
}
