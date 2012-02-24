using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DockPanelSuite;
using Newtonsoft.Json.Linq;

namespace Moai.Tools
{
    public partial class ImmediateWindowTool : Tool
    {
        public ImmediateWindowTool()
        {
            InitializeComponent();

            // Hook the debugging events.
            Program.Manager.DebugManager.DebugStart += new EventHandler(DebugManager_DebugStart);
            Program.Manager.DebugManager.DebugContinue += new EventHandler(DebugManager_DebugContinue);
            Program.Manager.DebugManager.DebugPause += new EventHandler(DebugManager_DebugPause);
            Program.Manager.DebugManager.DebugStop += new EventHandler(DebugManager_DebugStop);
        }

        /// <summary>
        /// This event is raised when the debug manager has started debugging, and
        /// we should (initially) disable the control.
        /// </summary>
        void DebugManager_DebugStart(object sender, EventArgs e)
        {
            this.c_ImmediateTextBox.ReadOnly = true;
        }

        /// <summary>
        /// This event is raised when the debug manager has continued debugging, and
        /// we should disable the control.
        /// </summary>
        void DebugManager_DebugContinue(object sender, EventArgs e)
        {
            this.c_ImmediateTextBox.ReadOnly = true;
        }

        /// <summary>
        /// This event is raised when the debug manager has paused debugging or is ready
        /// to receive evaluation messages, and we should enable the control.
        /// </summary>
        void DebugManager_DebugPause(object sender, EventArgs e)
        {
            this.c_ImmediateTextBox.ReadOnly = false;
        }

        /// <summary>
        /// This event is raised when the debug manager has stopped debugging, and
        /// we should disable the control.
        /// </summary>
        void DebugManager_DebugStop(object sender, EventArgs e)
        {
            this.c_ImmediateTextBox.ReadOnly = true;
        }

        /// <summary>
        /// The default location of the Immediate Window in the workspace.
        /// </summary>
        public override DockState DefaultState
        {
            get
            {
                return DockState.DockBottom;
            }
        }

        /// <summary>
        /// This event is raised when the user attempts to type in the text area
        /// for the immediate window.
        /// </summary>
        private void c_ImmediateTextBox_KeyDown(object sender, KeyEventArgs e)
        {
            // Only pass keystrokes in if the user is typing on the last line of the control.
            if ((this.c_ImmediateTextBox.GetLineFromCharIndex(this.c_ImmediateTextBox.SelectionStart) != this.c_ImmediateTextBox.Lines.Count() - 1 &&
                this.c_ImmediateTextBox.Lines.Count() != 0 )||
                this.c_ImmediateTextBox.SelectionLength != 0 ||
                this.c_ImmediateTextBox.ReadOnly ||
                (e.KeyCode == Keys.Back &&
                this.c_ImmediateTextBox.Lines[this.c_ImmediateTextBox.GetLineFromCharIndex(this.c_ImmediateTextBox.SelectionStart)].Length == 0))
            {
                if (e.KeyCode != Keys.Up &&
                    e.KeyCode != Keys.Down &&
                    e.KeyCode != Keys.Left &&
                    e.KeyCode != Keys.Right &&
                    e.KeyCode != Keys.Home &&
                    e.KeyCode != Keys.End &&
                    e.KeyCode != Keys.PageUp &&
                    e.KeyCode != Keys.PageDown)
                {
                    e.SuppressKeyPress = true;
                    return;
                }
            }

            // If the user pressed enter on the last line, evaluate the expression.
            if (e.KeyCode == Keys.Enter)
            {
                string value = this.c_ImmediateTextBox.Lines[this.c_ImmediateTextBox.GetLineFromCharIndex(this.c_ImmediateTextBox.SelectionStart)];
                if (!Program.Manager.DebugManager.Paused || !Program.Manager.DebugManager.Running)
                    this.c_ImmediateTextBox.AppendText("\r\nYou can't execute statements while the game is not paused.");
                else
                {
                    // Set the immediate window to readonly.
                    this.c_ImmediateTextBox.AppendText("\r\n");
                    this.c_ImmediateTextBox.ReadOnly = true;

                    // Ask for the expression to be evaluated with a callback.
                    Program.Manager.DebugManager.Evaluate(value, (result) =>
                    {
                        this.Invoke(new Action<object>(this.HandleCallback), result);
                    });
                }
                e.SuppressKeyPress = true;
                return;
            }
        }

        private void HandleCallback(object result)
        {
            try
            {
                string data = this.FormatResult(result);
                if (!data.EndsWith("\r\n")) data += "\r\n";
                this.c_ImmediateTextBox.ReadOnly = false;
                this.c_ImmediateTextBox.AppendText(data);
            }
            catch (Exception e)
            {
                this.c_ImmediateTextBox.AppendText("\r\n");
                this.c_ImmediateTextBox.ReadOnly = false;
            }
        }

        /// <summary>
        /// Formats the specified JSON object as a string.
        /// </summary>
        /// <param name="result">The JSON object to format.</param>
        /// <returns></returns>
        private string FormatResult(object result)
        {
            return this.FormatResult(result, "");
        }

        /// <summary>
        /// Formats the specified JSON object as a string.
        /// </summary>
        /// <param name="result">The JSON object to format.</param>
        /// <param name="indent">The indent of the string.</param>
        /// <returns></returns>
        private string FormatResult(object result, string indent)
        {
            if (result == null) return "null";
            JToken json = (result as JToken);
            if (json == null) return result.ToString();

            if (json is JProperty)
            {
                return (json as JProperty).Name + ": " + this.FormatResult((json as JProperty).Value, indent).Trim();
            }
            else if (json is JContainer)
            {
                if ((json as JContainer).Count() == 0)
                {
                    if (indent == "")
                        return "nil";
                    else
                        return "{}";
                }
                int i = 0;
                string s = "";
                s += indent + "{\r\n";
                foreach (object o in (json as JContainer))
                {
                    s += indent + "    " + this.FormatResult(o, indent + "    ");
                    i += 1;
                    if (i < (json as JContainer).Count())
                        s += ",";
                    s += "\r\n";
                }
                s += indent + "}\r\n";
                return s;
            }
            else
                return json.ToString();
        }
    }
}
