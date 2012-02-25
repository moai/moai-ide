using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Platform.UI
{
    public class PickingData
    {
        public bool CheckFileExists { get; set; }
        public bool CheckPathExists { get; set; }
        public bool RestoreDirectory { get; set; }
        public string Filter { get; set; }
    }
}
