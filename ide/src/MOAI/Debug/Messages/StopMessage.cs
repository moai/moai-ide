using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Debug.Messages
{
    public class StopMessage : Message
    {
        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "stop"; } }
        public override string ID { get { return "stop"; } }
    }
}
