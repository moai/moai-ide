using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qyoto;
using log4net;

namespace Moai.Platform.Linux
{
    public class LinuxPlatform : IPlatform
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(LinuxPlatform));

        public LinuxPlatform()
        {
            new QApplication(new string[] { });
            this.UI = new LinuxUI();
        }

        public IIDE CreateIDE()
        {
            return new LinuxIDE();
        }

        public void RunIDE(IIDE ide)
        {
            (ide as LinuxIDE).ShowMaximized();
            try
            {
                QApplication.Exec();
            }
            catch (Exception e)
            {
                m_Log.Error("An exception occurred while running the Linux platform.", e);
                throw new ApplicationException("The application is unable to continue due to an inconsistant Qt state.", e);
            }
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
