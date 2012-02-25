using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Windows.UI
{
    public static class KeyUtil
    {
        public static System.Windows.Forms.Keys FromPlatform(Moai.Platform.UI.Keys keys)
        {
            return (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), Enum.GetName(typeof(Moai.Platform.UI.Keys), keys));
        }

        public static Moai.Platform.UI.Keys ToPlatform(System.Windows.Forms.Keys keys)
        {
            return (Moai.Platform.UI.Keys)Enum.Parse(typeof(Moai.Platform.UI.Keys), Enum.GetName(typeof(System.Windows.Forms.Keys), keys));
        }
    }
}
