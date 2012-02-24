using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Debug.Messages
{
    public class BreakpointSetAlwaysMessage : Message
    {
        private uint p_LineNumber = 0;
        private string p_FileName = null;

        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "break_set_always"; } }
        public override string ID { get { return "break_set_always"; } }

        /// <summary>
        /// The line number the exception occurred on.
        /// </summary>
        public uint LineNumber
        {
            get
            {
                return this.p_LineNumber;
            }
            set
            {
                this.p_LineNumber = value;
            }
        }

        /// <summary>
        /// The filename of the script the exception occurred in.
        /// </summary>
        public string FileName
        {
            get
            {
                return this.p_FileName;
            }
            set
            {
                this.p_FileName = value;
            }
        }
    }
}
