using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Cache
{
    public class Context
    {
        private object p_Object = null;

        /// <summary>
        /// Fired when the context object changes.
        /// </summary>
        public event EventHandler<ContextEventArgs> ContextChanged;

        /// <summary>
        /// The current context object, for use with global actions such
        /// as Cut & Copy that do not have a specific context to operate
        /// within.
        /// </summary>
        public object Object
        {
            get
            {
                return this.p_Object;
            }
            set
            {
                if (this.ContextChanged != null)
                    this.ContextChanged(this, new ContextEventArgs(value));
                this.p_Object = value;
            }
        }
    }

    public class ContextEventArgs : EventArgs
    {
        public object Object { get; private set; }

        public ContextEventArgs(object context)
        {
            this.Object = context;
        }
    }
}
