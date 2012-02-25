using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Cache
{
    public interface ICacheManager : IManager
    {
        /// <summary>
        /// The context object.
        /// </summary>
        Context Context { get; }

        /// <summary>
        /// The autocompletion.
        /// </summary>
        IAutoComplete AutoComplete { get; }
    }
}
