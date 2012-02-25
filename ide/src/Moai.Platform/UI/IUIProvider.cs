using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Moai.Platform.Templates.Solutions;

namespace Moai.Platform.UI
{
    public interface IUIProvider
    {
        IMenuStrip CreateMenuStrip();
        IToolStrip CreateToolStrip();
        IToolStripMenuItem CreateToolStripMenuItem();
        IToolStripMenuItem CreateToolStripMenuItem(string text, Image image, IToolStripItem[] children);
        IImageList CreateImageList();
        ITreeNode CreateTreeNode();
        IContextMenuStrip CreateContextMenuStrip();
        IToolStripItem CreateToolStripSeperator();
        IToolStripComboBox CreateToolStripComboBox();

        void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        void ShowMessage(string message, string title, MessageBoxButtons buttons);
        void ShowMessage(string message, string title);
        void ShowMessage(string message);

        string PickExistingFile(PickingData data);
        SolutionCreationData PickNewSolution();
    }
}
