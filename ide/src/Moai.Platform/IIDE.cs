using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;

namespace Moai.Platform
{
    public interface IIDE : IForm
    {
        event EventHandler Opened;
        event EventHandler Closed;
        event EventHandler ActiveTabChanged;
        event EventHandler ResizeEnd;

        void Show();

        ITab ActiveTab { get; }
        bool IsDisposed { get; }

        void ShowDock(ITool tool, ToolPosition position);
    }
}
