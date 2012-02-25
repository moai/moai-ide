using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Moai.Platform.Menus.Definitions.Help
{
    class ReportBug : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Report a Bug";
            this.Enabled = true;
        }

        public override void OnActivate()
        {
            Process.Start("https://github.com/moai/moai-ide/issues/new");
        }
    }

    class CheckUpdates : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Check for Updates";
            this.Enabled = true;
        }

        public override void OnActivate()
        {
            Process.Start("http://build.redpointsoftware.com.au/job/Moai%20IDE/");
        }
    }
}
