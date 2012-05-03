using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.UI;
using Moai.Platform.Linux.UI;
using System.Threading;
using Qyoto;

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
            QMessageBox.StandardButton nativeButtons;
            QMessageBox.Icon nativeIcon;
            switch (buttons)
            {
                case Moai.Platform.UI.MessageBoxButtons.OK:
                    nativeButtons = QMessageBox.StandardButton.Ok;
                    break;
                case Moai.Platform.UI.MessageBoxButtons.OKCancel:
                    nativeButtons = QMessageBox.StandardButton.Ok | QMessageBox.StandardButton.Cancel;
                    break;
                case Moai.Platform.UI.MessageBoxButtons.YesNo:
                    nativeButtons = QMessageBox.StandardButton.Yes | QMessageBox.StandardButton.No;
                    break;
                default:
                    throw new NotSupportedException();
            }
            switch (icon)
            {
                case Moai.Platform.UI.MessageBoxIcon.None:
                    nativeIcon = QMessageBox.Icon.NoIcon;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Information:
                    nativeIcon = QMessageBox.Icon.Information;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Warning:
                    nativeIcon = QMessageBox.Icon.Warning;
                    break;
                case Moai.Platform.UI.MessageBoxIcon.Error:
                    nativeIcon = QMessageBox.Icon.Critical;
                    break;
                default:
                    throw new NotSupportedException();
            }

            QMessageBox dialog = new QMessageBox(nativeIcon, title, message, (uint)nativeButtons, (Central.Manager.IDE as LinuxIDE));
            dialog.Show();
            //MessageDialog dialog = new MessageDialog((Central.Manager.IDE as LinuxIDE), DialogFlags.Modal, nativeIcon, nativeButtons, message);
            //dialog.Show();
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
            return QFileDialog.GetOpenFileName(
                (Central.Manager.IDE as LinuxIDE),
                "Select File",
                Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments),
                "All Files (*.*)"
                );
            /*FileChooserDialog fcd = new FileChooserDialog(
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
            return result;*/
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
