using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Debug.Messages
{
    public class ExcpUserMessage : Message
    {
        private string p_ExceptionMessage = null;
        private string p_FunctionName = null;
        private uint p_LineNumber = 0;
        private string p_FileName = null;
        private string p_UserData = null;

        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "excp_user"; } }
        public override string ID { get { return "excp_user"; } }

        /// <summary>
        /// The exception message.
        /// </summary>
        public string ExceptionMessage
        {
            get
            {
                return this.p_ExceptionMessage;
            } 
            set
            {
                this.p_ExceptionMessage = value;
            }
        }

        /// <summary>
        /// The function name that the exception occurred in (if applicable).
        /// </summary>
        public string FunctionName
        {
            get
            {
                return this.p_FunctionName;
            }
            set
            {
                this.p_FunctionName = value;
            }
        }

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
        /// The associated user data.
        /// </summary>
        public string UserData
        {
            get
            {
                return this.p_UserData;
            }
            set
            {
                this.p_UserData = value;
            }
        }
    }
}
