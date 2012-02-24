using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using MOAI.Debug.Messages;

namespace MOAI.Debug
{
    public partial class ExceptionDialog : Form
    {
        private ExcpInternalMessage p_MessageInternal = null;
        private ExcpUserMessage p_MessageUser = null;
        private IDE p_IDEWindow = null;

        public ExceptionDialog()
        {
            InitializeComponent();
        }

        /// <summary>
        /// The engine exception that was raised (set this to null to indicate
        /// that the exception was not an engine exception).
        /// </summary>
        public ExcpInternalMessage MessageInternal
        {
            get { return this.p_MessageInternal; }
            set { this.p_MessageInternal = value; }
        }

        /// <summary>
        /// The user-defined exception that was raised (set this to null to indicate
        /// that the exception was not a user-defined exception).
        /// </summary>
        public ExcpUserMessage MessageUser
        {
            get { return this.p_MessageUser; }
            set { this.p_MessageUser = value; }
        }

        /// <summary>
        /// The IDE window that this exception dialog should use to
        /// show the file and highlighted line.
        /// </summary>
        public IDE IDEWindow
        {
            get { return this.p_IDEWindow; }
            set { this.p_IDEWindow = value; }
        }

        /// <summary>
        /// This event is raised when the window must be redrawn.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The painting event arguments.</param>
        private void ExceptionDialog_Paint(object sender, PaintEventArgs e)
        {
            if (this.p_MessageInternal != null)
            {
                // Internal engine exception raised.
                e.Graphics.DrawString(this.p_MessageInternal.ExceptionMessage, SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 10));
                e.Graphics.DrawString(this.p_MessageInternal.FileName, SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 30));
                e.Graphics.DrawString(this.p_MessageInternal.FunctionName, SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 50));
                e.Graphics.DrawString(this.p_MessageInternal.LineNumber.ToString(), SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 70));
            }
            else if (this.p_MessageUser != null)
            {
                // User exception raised.
                e.Graphics.DrawString(this.p_MessageUser.ExceptionMessage, SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 10));
                e.Graphics.DrawString(this.p_MessageUser.FileName, SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 30));
                e.Graphics.DrawString(this.p_MessageUser.FunctionName, SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 50));
                e.Graphics.DrawString(this.p_MessageUser.LineNumber.ToString(), SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 70));
                e.Graphics.DrawString(this.p_MessageUser.UserData, SystemFonts.DefaultFont, SystemBrushes.ControlText, new PointF(10, 90));
            }
        }

        /// <summary>
        /// This event is raised when the exception dialog is clicked.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ExceptionDialog_Click(object sender, EventArgs e)
        {
            this.Close();
        }

        /// <summary>
        /// This event is raised when the exception dialog is loaded.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event arguments.</param>
        private void ExceptionDialog_Load(object sender, EventArgs e)
        {
            // Determine the line number and filename based on the exception type.
            uint line = 0;
            string filename = null;
            if (this.p_MessageInternal != null)
            {
                line = this.p_MessageInternal.LineNumber;
                filename = this.p_MessageInternal.FileName;
            }
            else if (this.p_MessageUser != null)
            {
                line = this.p_MessageUser.LineNumber;
                filename = this.p_MessageUser.FileName;
            }

            // Automatically switch the IDE to the correct file
            // and highlight the line which caused the exception.
            this.p_IDEWindow.Focus();
            MOAI.Management.File f = this.p_IDEWindow.Manager.ActiveProject.GetByPath(filename);
            if (f != null)
            {
                // Ask the IDE to open the file that the exception occurred in; we know the designer
                // will be a code designer as well, so we can safely cast it.
                MOAI.Designers.Code.Designer d = this.p_IDEWindow.Manager.DesignersManager.OpenDesigner(f) as MOAI.Designers.Code.Designer;

                // Highlight the line.
                d.HighlightLine((int)line);

                // TODO: This doesn't work if the designer has just been opened!
            }
            this.Focus();
        }
    }
}
