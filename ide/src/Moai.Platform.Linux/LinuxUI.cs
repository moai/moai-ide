using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Gtk;
using Moai.Platform.UI;
using Moai.Platform.Linux.UI;
using System.Threading;

namespace Moai.Platform.Linux
{
    public class LinuxUI : IUIProvider
    {
        #region IUIProvider Members

        public IImageList CreateImageList()
        {
            return new LinuxImageList();
        }

        public void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon)
        {
            ButtonsType nativeButtons;
            MessageType nativeIcon;
            switch (buttons)
            {
                case Moai.Platform.UI.MessageBoxButtons.OK:
                    nativeButtons = ButtonsType.Ok;
                    break;
                case Moai.Platform.UI.MessageBoxButtons.OKCancel:
                    nativeButtons = ButtonsType.OkCancel;
                    break;
                case Moai.Platform.UI.MessageBoxButtons.YesNo:
                    nativeButtons = ButtonsType.YesNo;
                    break;
                default:
                    throw new NotSupportedException();
            }
            switch (icon)
            {
                case Moai.Platform.UI.MessageBoxIcon.None:
                    nativeIcon = MessageType.Other;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Information:
                    nativeIcon = MessageType.Info;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Warning:
                    nativeIcon = MessageType.Warning;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Error:
                    nativeIcon = MessageType.Error;
                    break;
                default:
                    throw new NotSupportedException();
            }

            MessageDialog dialog = new MessageDialog((Central.Manager.IDE as LinuxIDE), DialogFlags.Modal, nativeIcon, nativeButtons, message);
            dialog.Show();
        }

        public void ShowMessage(string message, string title, MessageBoxButtons buttons)
        {
            this.ShowMessage(message, title, buttons, MessageBoxIcon.None);
        }

        public void ShowMessage(string message, string title)
        {
            this.ShowMessage(message, title, MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        public void ShowMessage(string message)
        {
            this.ShowMessage(message, "", MessageBoxButtons.OK, MessageBoxIcon.None);
        }

        public string PickExistingFile(PickingData data)
        {
            FileChooserDialog fcd = new FileChooserDialog(
                "Select File",
                (Central.Manager.IDE as LinuxIDE),
                FileChooserAction.Open,
                "Cancel", ResponseType.Cancel,
                "Open", ResponseType.Accept);
            fcd.Filter = new FileFilter();
            string[] f = data.Filter.Split(new char[] { '|' });
            for (int i = 0; i < f.Length; i += 2)
                fcd.Filter.AddPattern(f[i + 1]);
            string result = null;
            if (fcd.Run() == (int)ResponseType.Accept)
                result = fcd.Filename;
            fcd.Destroy();
            return result;
        }

        public Moai.Platform.Templates.Solutions.SolutionCreationData PickNewSolution()
        {
            throw new NotImplementedException();
        }

        public Moai.Platform.Templates.Files.FileCreationData PickNewFile()
        {
            throw new NotImplementedException();
        }

        public Moai.Platform.Templates.Files.FileCreationData PickNewFile(string preselected)
        {
            throw new NotImplementedException();
        }

        #endregion
    }
}
