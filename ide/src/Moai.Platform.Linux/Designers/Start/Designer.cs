using System;
using Moai.Platform.Designers;
using Qyoto;
using log4net;
using System.Threading;

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

        public class StartNetworkAccessManager : QNetworkAccessManager
        {
            public StartNetworkAccessManager() : base()
            {
            }

            protected override QNetworkReply CreateRequest(QNetworkAccessManager.Operation op, QNetworkRequest request, QIODevice outgoingData)
            {
                // So Qt by default for SSL requires the most secure settings
                // possible.  This tells Qt not to care about some of the high-security
                // features so that more pages will be supported in the web browser.
                try
                {
                    QSslConfiguration config = request.SslConfiguration();
                    config.SetPeerVerifyMode(QSslSocket.PeerVerifyMode.VerifyNone);
                    config.SetProtocol(QSsl.SslProtocol.TlsV1);
                    QNetworkRequest req = new QNetworkRequest(request);
                    req.SetSslConfiguration(config);
                    QNetworkReply reply = base.CreateRequest(op, req);
                    reply.IgnoreSslErrors();
                    LinuxNativePool.Instance.Retain(req);
                    LinuxNativePool.Instance.Retain(reply);
                    return reply;
                }
                catch (Exception e)
                {
                    m_Log.Error("Unable to generate SSL configuration for request.  Page may not load correctly.", e);
                    return base.CreateRequest(op, request, outgoingData);
                }
            }
        }

        public class StartWebPage : QWebPage
        {
            public StartWebPage(QNetworkAccessManager snam)
            {
                this.SetNetworkAccessManager(snam);
            }

            protected override void JavaScriptConsoleMessage(string message, int lineNumber, string sourceID)
            {
                m_Log.Error("Javascript: " + message);
                m_Log.Error("at line " + lineNumber + " of " + sourceID + ".");
                base.JavaScriptConsoleMessage(message, lineNumber, sourceID);
            }
        }
    }
}

