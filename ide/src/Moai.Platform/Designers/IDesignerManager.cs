using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Management;

namespace Moai.Platform.Designers
{
    public interface IDesignerManager
    {
        event DesignerEventHandler DesignerCreated;
        event DesignerEventHandler DesignerOpened;
        event DesignerEventHandler DesignerRefocused;
        event DesignerEventHandler DesignerClosed;
        event DesignerEventHandler DesignerChanged;

        IDesigner OpenDesigner(File file);

        void EmulateDesignerChange();
    }

    public delegate void DesignerEventHandler(object sender, DesignerEventArgs e);

    public class DesignerEventArgs : EventArgs
    {
        public DesignerEventArgs(IDesigner designer)
        {
            this.Designer = designer;
        }

        public IDesigner Designer
        {
            get;
            private set;
        }
    }
}
