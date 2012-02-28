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
            System.Windows.Forms.Keys modifiers = 0;
            if ((keys & Moai.Platform.UI.Keys.Control) != 0)
            {
                modifiers |= System.Windows.Forms.Keys.Control;
                keys &= ~Moai.Platform.UI.Keys.Control;
            }
            if ((keys & Moai.Platform.UI.Keys.Shift) != 0)
            {
                modifiers |= System.Windows.Forms.Keys.Shift;
                keys &= ~Moai.Platform.UI.Keys.Shift;
            }
            if ((keys & Moai.Platform.UI.Keys.Alt) != 0)
            {
                modifiers |= System.Windows.Forms.Keys.Alt;
                keys &= ~Moai.Platform.UI.Keys.Alt;
            }
            return (System.Windows.Forms.Keys)Enum.Parse(typeof(System.Windows.Forms.Keys), Enum.GetName(typeof(Moai.Platform.UI.Keys), keys)) | modifiers;
        }

        public static Moai.Platform.UI.Keys ToPlatform(System.Windows.Forms.Keys keys)
        {
            Moai.Platform.UI.Keys modifiers = 0;
            if ((keys & System.Windows.Forms.Keys.Control) != 0)
            {
                modifiers |= Moai.Platform.UI.Keys.Control;
                keys &= ~System.Windows.Forms.Keys.Control;
            }
            if ((keys & System.Windows.Forms.Keys.Shift) != 0)
            {
                modifiers |= Moai.Platform.UI.Keys.Shift;
                keys &= ~System.Windows.Forms.Keys.Shift;
            }
            if ((keys & System.Windows.Forms.Keys.Alt) != 0)
            {
                modifiers |= Moai.Platform.UI.Keys.Alt;
                keys &= ~System.Windows.Forms.Keys.Alt;
            }
            return (Moai.Platform.UI.Keys)Enum.Parse(typeof(Moai.Platform.UI.Keys), Enum.GetName(typeof(System.Windows.Forms.Keys), keys)) | modifiers;
        }
    }
}
