using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using Moai.Platform.Templates.Files;
using Moai.Platform.Templates.Solutions;
using Moai.Platform.Designers;
using Moai.Platform.Management;

namespace Moai.Platform.UI
{
    public interface IUIProvider
    {
        IImageList CreateImageList();

        void ShowMessage(string message, string title, MessageBoxButtons buttons, MessageBoxIcon icon);
        void ShowMessage(string message, string title, MessageBoxButtons buttons);
        void ShowMessage(string message, string title);
        void ShowMessage(string message);

        string PickExistingFile(PickingData data);
        SolutionCreationData PickNewSolution();
        FileCreationData PickNewFile();
        FileCreationData PickNewFile(string preselected);
    }
}
