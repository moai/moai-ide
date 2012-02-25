using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.Menus.Definitions.Views
{
    public class Code : Action
    {
        public Code() : base() { }
        public Code(object context) : base(context) { }

        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.view_code;
            this.Text = "Code";
            this.Enabled = false;
            this.MenuItem.Checked = true;
        }
    }

    public class Designer : Action
    {
        public Designer() : base() { }
        public Designer(object context) : base(context) { }

        public override void OnInitialize()
        {
            this.Implemented = false;
            this.ItemIcon = IDE.Resources.Images.view_designer;
            this.Text = "Designer";
            this.Enabled = false;
            this.MenuItem.Checked = false;
        }
    }
}
