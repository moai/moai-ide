using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using Moai.Platform.UI;

namespace Moai.Platform.Linux.Tools
{
    public class Tool : Table, ITool
    {
        public Tool()
            : base(1, 1, true)
        {
        }

        public string Title
        {
            get;
            protected set;
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
