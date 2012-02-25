using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Debug.Messages
{
    public class EvaluateMessage : Message
    {
        private string p_Evaluation = null;

        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "evaluate"; } }
        public override string ID { get { return "evaluate"; } }

        /// <summary>
        /// The Lua expression to evaluate.
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
