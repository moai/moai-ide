using System;
using Moai.Platform.Designers;
using Qyoto;
using log4net;
using System.Threading;
using System.Collections.Generic;

namespace Moai.Platform.Linux.Designers.Start
{
    public partial class Designer : Moai.Designers.Designer, IStartDesigner
    {
        private static readonly ILog m_Log = LogManager.GetLogger(typeof(Designer));

        public Designer()
        {
            InitializeComponent();
            this.TabText = "Cloud Dashboard";
            this.c_Browser.Load("https://dashboard.moaicloud.com/login.php");
            this.Connect(this.c_Browser.Page().NetworkAccessManager(), SIGNAL("sslErrors(QNetworkReply*,const QList<QSslError>&)"), SLOT("OnSslErrors(QNetworkReply*)"));
        }

        protected override void ResizeEvent(QResizeEvent arg1)
        {
            base.ResizeEvent(arg1);
            this.c_Browser.SetSizePolicy(QSizePolicy.Policy.Expanding, QSizePolicy.Policy.Expanding);
            this.c_Browser.SetGeometry(0, 0, this.Width(), this.Height());
            this.c_Browser.Resize(this.Width(), this.Height());
            this.c_Browser.Update();
        }

        [Q_SLOT]
        void OnLoadStarted()
        {
            this.TabText = "Loading... ";
        }

        [Q_SLOT]
        void OnLoadProgress(int progress)
        {
            if (progress != 100)
                this.TabText = "Loading... " + progress + "%";
        }

        [Q_SLOT]
        void OnLoadFinished(bool ok)
        {
            if (!ok)
                this.TabText = "Unable to load page.";
            else
            {
                this.TabText = this.c_Browser.Title;
                this.TabIcon = this.c_Browser.icon;
            }
        }

        [Q_SLOT]
        void OnSslErrors(QNetworkReply reply)
        {
            LinuxNativePool.Instance.Retain(reply);
            reply.IgnoreSslErrors();
        }

        public class StartWebPage : QWebPage
        {
            protected override void JavaScriptConsoleMessage(string message, int lineNumber, string sourceID)
            {
                m_Log.Error("Javascript: " + message);
                m_Log.Error("at line " + lineNumber + " of " + sourceID + ".");
                base.JavaScriptConsoleMessage(message, lineNumber, sourceID);
            }
        }
    }
}

