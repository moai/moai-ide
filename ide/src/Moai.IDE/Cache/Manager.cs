using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Menus;
using Moai.Platform.Cache;

namespace Moai.IDE.Cache
{
    public class Manager : ICacheManager
    {
        //private Scintilla p_ScintillaCache = null;
        private AutoComplete p_AutoComplete = null;
        private UndoRedoCache p_UndoRedo = null;

        /// <summary>
        /// Creates a new cache manager, which manages all of the sub-caches.
        /// </summary>
        public Manager()
        {
            //this.p_ScintillaCache = new Scintilla();
            this.p_AutoComplete = new AutoComplete();
            this.p_AutoComplete.Import();
            this.Context = new Context();
            this.p_UndoRedo = new UndoRedoCache();
        }

        /// <summary>
        /// The scintilla caching object.
        /// </summary>
        /*public Scintilla ScintillaCache
        {
            get
            {
                return this.p_ScintillaCache;
            }
        }*/

        /// <summary>
        /// The autocomplete caching object.
        /// </summary>
        public IAutoComplete AutoComplete
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
            get;
            private set;
        }

        /// <summary>
        /// The undo-redo caching object.
        /// </summary>
        public UndoRedoCache UndoRedo
        {
            get
            {
                return this.p_UndoRedo;
            }
        }
    }
}
