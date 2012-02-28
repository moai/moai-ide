using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;

namespace Moai.Platform.Menus.Definitions.Help
{
    class HowDoI : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "How Do I";
            this.Implemented = false;
        }
    }

    class Search : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Search";
            this.Implemented = false;
        }
    }

    class Contents : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Contents";
            this.Implemented = false;
        }
    }

    class Index : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Index";
            this.Implemented = false;
        }
    }

    class Forums : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "Moai Forums";
            this.Enabled = true;
        }

        public override void OnActivate()
        {
            Process.Start("http://getmoai.com/forums/");
        }
    }

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

    class AboutEngine : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "About Moai Engine";
            this.Enabled = true;
        }

        public override void OnActivate()
        {
            Process.Start("http://getmoai.com/");
        }
    }

    class AboutIDE : Action
    {
        public override void OnInitialize()
        {
            this.ItemIcon = null;
            this.Text = "About Moai IDE";
            this.Enabled = true;
        }

        public override void OnActivate()
        {
            Process.Start("http://getmoai.com/forums/moai-ide-t23/");
        }
    }
}
