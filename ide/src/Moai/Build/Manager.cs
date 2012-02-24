using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Build
{
    public class Manager
    {
        Moai.Manager p_Parent = null;

        /// <summary>
        /// Creates a new Manager class for managing the build process.
        /// </summary>
        /// <param name="parent">The main Moai manager which owns this build manager.</param>
        public Manager(Moai.Manager parent)
        {
            this.p_Parent = parent;
        }

        /// <summary>
        /// The main Moai manager that owns this build manager.
        /// </summary>
        public Moai.Manager Parent
        {
            get
            {
                return this.p_Parent;
            }
        }
    }
}
