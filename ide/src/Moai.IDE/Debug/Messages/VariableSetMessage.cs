using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Moai.Debug.Messages
{
    public class VariableSetMessage : Message
    {
        private string p_VariableName = null;
        private string p_VariableValue = null;
        private string[] p_TableNames = null;

        /// <summary>
        /// The identifier for this type of message.
        /// </summary>
        public static string StaticID { get { return "variable_set"; } }
        public override string ID { get { return "variable_set"; } }

        /// <summary>
        /// The name of the variable to set.
        /// </summary>
        public string VariableName
        {
            get
            {
                return this.p_VariableName;
            }
            set
            {
                this.p_VariableName = value;
            }
        }
        /// <summary>
        /// The new value of the variable.
        /// </summary>
        public string VariableValue
        {
            get
            {
                return this.p_VariableValue;
            }
            set
            {
                this.p_VariableValue = value;
            }
        }

        /// <summary>
        /// The tables that the variable is located in.
        /// </summary>
        public string[] TableNames
        {
            get
            {
                return this.p_TableNames;
            }
            set
            {
                this.p_TableNames = value;
            }
        }
    }
}
