using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Debug.Messages
{
    public class ResultMessage : Message
    {
        private object p_Value = null;

        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "result"; } }
        public override string ID { get { return "result"; } }

        /// <summary>
        /// The Lua value attached to this result.
        /// </summary>
        public object Value
        {
            get
            {
                return this.p_Value;
            }
            set
            {
                this.p_Value = value;
            }
        }
    }
}
