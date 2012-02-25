using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Debug.Messages
{
    public class ContinueMessage : Message
    {
        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "continue"; } }
        public override string ID { get { return "continue"; } }
    }
}
