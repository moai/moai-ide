using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moai.Platform.UI;
using Moai.Platform.Designers;

namespace Moai.Platform.Windows
{
    public class WindowsPlatform : IPlatform
    {
        public WindowsPlatform()
        {
            this.Clipboard = new WindowsClipboard();
            this.UI = new WindowsUI();
        }

        public IIDE CreateIDE()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);

            return new WindowsIDE();
        }

        public void RunIDE(IIDE ide)
        {
            Application.Run(ide as WindowsIDE);
        }

        public Type GetDesignerTypeImplementing(Type type)
        {
            if (type == typeof(ICodeDesigner))
                return typeof(Designers.Code.Designer);
            else if (type == typeof(IStartDesigner))
                return typeof(Designers.Start.Designer);
            return null;
        }

        public IUIProvider UI
        {
            get;
            private set;
        }

        public IClipboard Clipboard
        {
            get;
            private set;
        }
    }
}
