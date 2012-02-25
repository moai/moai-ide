using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moai.Platform.UI;
using Moai.Platform.Windows.UI;

namespace Moai.Platform.Windows
{
    public class WindowsUI : IUIProvider
    {
        public IMenuStrip CreateMenuStrip()
        {
            throw new NotImplementedException();
        }

        public IToolStrip CreateToolStrip()
        {
            throw new NotImplementedException();
        }

        public IToolStripMenuItem CreateToolStripMenuItem()
        {
            throw new NotImplementedException();
        }

        public IToolStripMenuItem CreateToolStripMenuItem(string text, System.Drawing.Image image, IToolStripItem[] children)
        {
            throw new NotImplementedException();
        }

        public IImageList CreateImageList()
        {
            return new WindowsImageList();
        }

        public ITreeNode CreateTreeNode()
        {
            throw new NotImplementedException();
        }

        public IContextMenuStrip CreateContextMenuStrip()
        {
            throw new NotImplementedException();
        }

        public IToolStripItem CreateToolStripSeperator()
        {
            throw new NotImplementedException();
        }

        public IToolStripComboBox CreateToolStripComboBox()
        {
            throw new NotImplementedException();
        }

        public void ShowMessage(string message, string title, Moai.Platform.UI.MessageBoxButtons buttons, Moai.Platform.UI.MessageBoxIcon icon)
        {
            System.Windows.Forms.MessageBoxButtons nativeButtons;
            System.Windows.Forms.MessageBoxIcon nativeIcon;
            switch (buttons)
            {
                case Moai.Platform.UI.MessageBoxButtons.OK:
                    nativeButtons = System.Windows.Forms.MessageBoxButtons.OK;
                    break;
                case Moai.Platform.UI.MessageBoxButtons.OKCancel:
                    nativeButtons = System.Windows.Forms.MessageBoxButtons.OKCancel;
                    break;
                case Moai.Platform.UI.MessageBoxButtons.YesNo:
                    nativeButtons = System.Windows.Forms.MessageBoxButtons.YesNo;
                    break;
                default:
                    throw new NotSupportedException();
            }
            switch (icon)
            {
                case Moai.Platform.UI.MessageBoxIcon.None:
                    nativeIcon = System.Windows.Forms.MessageBoxIcon.None;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Information:
                    nativeIcon = System.Windows.Forms.MessageBoxIcon.Information;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Warning:
                    nativeIcon = System.Windows.Forms.MessageBoxIcon.Warning;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Error:
                    nativeIcon = System.Windows.Forms.MessageBoxIcon.Error;
                    break;
                default:
                    throw new NotSupportedException();
            }

            MessageBox.Show(message, title, nativeButtons, nativeIcon);
        }

        public void ShowMessage(string message, string title, Moai.Platform.UI.MessageBoxButtons buttons)
        {
            this.ShowMessage(message, "", Moai.Platform.UI.MessageBoxButtons.OK, Moai.Platform.UI.MessageBoxIcon.None);
        }

        public void ShowMessage(string message, string title)
        {
            this.ShowMessage(message, "", Moai.Platform.UI.MessageBoxButtons.OK, Moai.Platform.UI.MessageBoxIcon.None);
        }

        public void ShowMessage(string message)
        {
            this.ShowMessage(message, "", Moai.Platform.UI.MessageBoxButtons.OK, Moai.Platform.UI.MessageBoxIcon.None);
        }

        public string PickExistingFile(PickingData data)
        {
            throw new NotImplementedException();
        }

        public Moai.Platform.Templates.Solutions.SolutionCreationData PickNewSolution()
        {
            throw new NotImplementedException();
        }
    }
}
