using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Debug.Messages
{
    public class WaitMessage : Message
    {
        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "wait"; } }
        public override string ID { get { return "wait"; } }
    }
}
