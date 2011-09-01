using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Cache
{
    public class Manager
    {
        private MOAI.Manager p_Manager = null;
        private Scintilla p_ScintillaCache = null;
        private AutoComplete p_AutoComplete = null;
        private Context p_Context = null;

        /// <summary>
        /// Creates a new cache manager, which manages all of the sub-caches.
        /// </summary>
        public Manager(MOAI.Manager manager)
        {
            this.p_Manager = manager;

            this.p_ScintillaCache = new Scintilla();
            this.p_AutoComplete = new AutoComplete();
            this.p_AutoComplete.Import();
            this.p_Context = new Context();

            MOAI.Cache.Clipboard.Register(this.p_Manager);
        }

        /// <summary>
        /// The scintilla caching object.
        /// </summary>
        public Scintilla ScintillaCache
        {
            get
            {
                return this.p_ScintillaCache;
            }
        }

        /// <summary>
        /// The autocomplete caching object.
        /// </summary>
        public AutoComplete AutoComplete
        {
            get
            {
                return this.p_AutoComplete;
            }
        }

        /// <summary>
        /// The context object.
        /// </summary>
        public Context Context
        {
            get
            {
                return this.p_Context;
            }
        }
    }

    public delegate void ProgressCallback(int progress);
}
