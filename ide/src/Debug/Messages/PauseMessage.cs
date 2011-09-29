using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MOAI.Debug.Messages
{
    public class PauseMessage : Message
    {
        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "pause"; } }
        public override string ID { get { return "pause"; } }
    }
}
