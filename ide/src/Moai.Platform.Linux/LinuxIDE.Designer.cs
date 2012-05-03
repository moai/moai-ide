using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Qyoto;

namespace Moai.Platform.Linux
{
    public partial class LinuxIDE
    {
        /// <summary>
        /// Method for replacing the existing menu bar.
        /// </summary>
        private void SetMainMenu(QMenuBar menu)
        {
            base.SetMenuBar(menu);
        }

        /// <summary>
        /// Method for replacing the existing toolbar.
        /// </summary>
        private void SetToolBar(QToolBar toolbar)
        {
            base.AddToolBar(Qt.ToolBarArea.TopToolBarArea, toolbar);
        }

        /// <summary>
        /// Required method for initializing the IDE structure.
        /// </summary>
        private void InitializeComponent()
        {
            this.c_Documents = new QTabWidget(this);
            //
            // c_Documents
            //
            this.c_Documents.TabsClosable = true;
            //
            // IDE
            //
            this.SetCentralWidget(this.c_Documents);
            this.SetMinimumSize(600, 400);
        }

        QTabWidget c_Documents = null;
    }
}
