using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using DockPanelSuite;

namespace MOAI.Tools
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
            this.c_ImmediateTextBox.Enabled = false;
        }

        /// <summary>
        /// This event is raised when the debug manager has continued debugging, and
        /// we should disable the control.
        /// </summary>
        void DebugManager_DebugContinue(object sender, EventArgs e)
        {
            this.c_ImmediateTextBox.Enabled = false;
        }

        /// <summary>
        /// This event is raised when the debug manager has paused debugging or is ready
        /// to receive evaluation messages, and we should enable the control.
        /// </summary>
        void DebugManager_DebugPause(object sender, EventArgs e)
        {
            this.c_ImmediateTextBox.Enabled = true;
        }

        /// <summary>
        /// This event is raised when the debug manager has stopped debugging, and
        /// we should disable the control.
        /// </summary>
        void DebugManager_DebugStop(object sender, EventArgs e)
        {
            this.c_ImmediateTextBox.Enabled = false;
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
            if (this.c_ImmediateTextBox.GetLineFromCharIndex(this.c_ImmediateTextBox.SelectionStart) != this.c_ImmediateTextBox.Lines.Count() - 1 ||
                this.c_ImmediateTextBox.SelectionLength != 0)
            {
                e.SuppressKeyPress = true;
                return;
            }

            // If the user pressed enter on the last line, evaluate the expression.
            if (e.KeyCode == Keys.Enter)
            {
                string value = this.c_ImmediateTextBox.Lines[this.c_ImmediateTextBox.GetLineFromCharIndex(this.c_ImmediateTextBox.SelectionStart)];
                string result = Program.Manager.DebugManager.Evaluate(value);
                this.c_ImmediateTextBox.Text += "\r\n";
                if (result == null)
                    this.c_ImmediateTextBox.Text += "Evaluation timed out.";
                else
                    this.c_ImmediateTextBox.Text += result;
                e.SuppressKeyPress = true;
                return;
            }
        }
    }
}
