using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;

namespace Moai.Platform.Linux
{
    public class LinuxPlatform : IPlatform
    {
        public LinuxPlatform()
        {
            this.UI = new LinuxUI();
            Application.Init();
        }

        public IIDE CreateIDE()
        {
            return new LinuxIDE();
        }

        public void RunIDE(IIDE ide)
        {
            (ide as LinuxIDE).Maximize();
            (ide as LinuxIDE).ShowAll();
            Application.Run();
        }

        public Type GetDesignerTypeImplementing(Type type)
        {
            throw new NotImplementedException();
        }

        public Moai.Platform.UI.IUIProvider UI
        {
            get;
            private set;
        }

        public IClipboard Clipboard
        {
            get { throw new NotImplementedException(); }
        }
    }
}
