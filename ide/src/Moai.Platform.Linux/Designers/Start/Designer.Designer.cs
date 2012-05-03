using System;
using Qyoto;

namespace Moai.Platform.Linux.Designers.Start
{
    public partial class Designer
    {
        /// <summary>
        /// Required method for initializing the start page structure.
        /// </summary>
        private void InitializeComponent()
        {
            this.c_Browser = new QWebView(this);
            StartNetworkAccessManager snam = new StartNetworkAccessManager();
            StartWebPage swp = new StartWebPage(snam);
            //
            // snam
            //
            LinuxNativePool.Instance.Retain(snam);
            //
            // swp
            //
            LinuxNativePool.Instance.Retain(swp);
            this.Connect(swp, SIGNAL("loadStarted()"), SLOT("OnLoadStarted()"));
            //
            // c_Browser
            //
            this.Connect(this.c_Browser, SIGNAL("loadStarted()"), SLOT("OnLoadStarted()"));
            this.Connect(this.c_Browser, SIGNAL("loadProgress(int)"), SLOT("OnLoadProgress(int)"));
            this.Connect(this.c_Browser, SIGNAL("loadFinished(bool)"), SLOT("OnLoadFinished(bool)"));
            this.c_Browser.SetPage(swp);
        }

        QWebView c_Browser = null;
    }
}

