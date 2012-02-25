using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform.Management;

namespace Moai
{
    public class FileEventArgs : EventArgs
    {
        public File File { get; private set; }

        public FileEventArgs(File file)
        {
            this.File = file;
        }
    }
}
