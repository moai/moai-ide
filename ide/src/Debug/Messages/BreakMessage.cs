using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Debug.Messages
{
    public class BreakMessage : Message
    {
        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "break"; } }
        public override string ID { get { return "break"; } }
    }
}
