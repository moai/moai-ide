using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Debug.Messages
{
    public class BreakpointSetConditionalMessage : Message
    {
        private uint p_LineNumber = 0;
        private string p_FileName = null;
        private string p_Evaluation = null;

        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "break_set_conditional"; } }
        public override string ID { get { return "break_set_conditional"; } }

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

        /// <summary>
        /// The Lua expression to evaluate in order for this breakpoint to be triggered.
        /// </summary>
        public string Evaluation
        {
            get
            {
                return this.p_Evaluation;
            }
            set
            {
                this.p_Evaluation = value;
            }
        }
    }
}
