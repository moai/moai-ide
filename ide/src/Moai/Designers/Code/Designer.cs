using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moai.Management;
using Moai.Operatables;
using Moai.Debug;

namespace Moai.Designers.Code
{
    public partial class Designer : Moai.Designers.Designer, ICuttable, ICopyable, IPastable, IDeletable, ISavable, IDebuggable
    {
        private List<LuaError> m_Errors = new List<LuaError>();
        private CodeEditor c_CodeEditor = null;
        private ToolTip c_ToolTip = new ToolTip();
        private string m_SavedText = null;
        private bool p_CanSave = false;
        private int m_ActiveLine = 0;

        /// <summary>
        /// Creates a new code editor.
        /// </summary>
        /// <param name="manager">The main Moai manager.</param>
        /// <param name="file">The associated file.</param>
        public Designer(Moai.Manager manager, File file)
            : base(manager, file)
        {
            InitializeComponent();

            // Listen for events.
            file.Project.FileRenamed += new EventHandler<FileEventArgs>((sender, e) =>
            {
                if (e.File == this.File)
                {
                    if (this.c_CodeEditor.Text != this.m_SavedText)
                        this.TabText = this.File.FileInfo.Name + " *";
                    else
                        this.TabText = this.File.FileInfo.Name;
                }
            });

            // Initialize the code editor.
            this.c_CodeEditor = new CodeEditor(
                manager.CacheManager,
                (this.File.FileInfo.Extension.Substring(1) == "lua" ||
                 this.File.FileInfo.Extension.Substring(1) == "rks" ||
                 this.File.FileInfo.Extension.Substring(1) == "rs")
                );
            this.c_CodeEditor.DwellStart += new EventHandler<ScintillaNet.ScintillaMouseEventArgs>(c_CodeEditor_DwellStart);
            this.c_CodeEditor.DwellEnd += new EventHandler<ScintillaNet.ScintillaMouseEventArgs>(c_CodeEditor_DwellEnd);
            this.c_CodeEditor.KeyUp += new KeyEventHandler(c_CodeEditor_KeyUp);
            this.c_CodeEditor.SyntaxCheckRequested += new EventHandler(c_CodeEditor_SyntaxCheckRequested);
            this.c_CodeEditor.SelectionChanged += new EventHandler(c_CodeEditor_SelectionChanged);
            this.c_CodeEditor.GotFocus += new EventHandler(c_CodeEditor_GotFocus);
            this.c_CodeEditor.LostFocus += new EventHandler(c_CodeEditor_LostFocus);
            this.c_CodeEditor.MarginClick += new EventHandler<ScintillaNet.MarginClickEventArgs>(c_CodeEditor_MarginClick);

            // Initalize the context menu for the code editor.
            this.c_CodeEditor.ContextMenuStrip = new ContextMenuStrip();
            this.c_CodeEditor.ContextMenuStrip.Items.AddRange(new ToolStripItem[] {
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Undo(this.c_CodeEditor)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Redo(this.c_CodeEditor)),
                    new ToolStripSeparator(),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Cut(this.c_CodeEditor)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Copy(this.c_CodeEditor)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Paste(this.c_CodeEditor)),
                    Menus.Manager.WrapAction(new Menus.Definitions.Actions.Delete(this.c_CodeEditor))
                });

            // Now load the file data.
            using (System.IO.StreamReader reader = new System.IO.StreamReader(this.File.FileInfo.FullName))
            {
                this.c_CodeEditor.Text = reader.ReadToEnd();
            }
            this.m_SavedText = this.c_CodeEditor.Text;

            // Load the breakpoints.
            string relname = PathHelpers.GetRelativePath(this.File.Project.ProjectInfo.DirectoryName, this.File.FileInfo.FullName);
            List<Breakpoint> breakpoints = Program.Manager.DebugManager.Breakpoints.Where(result => result.SourceFile == relname).ToList();
            foreach (Breakpoint b in breakpoints)
            {
                this.c_CodeEditor.Lines[(int)b.SourceLine - 1].AddMarker(0);
            }

            // Detect if this file is read-only.
            this.p_CanSave = !this.File.FileInfo.IsReadOnly;
            if (this.p_CanSave)
                this.TabText = this.File.FileInfo.Name;
            else
            {
                this.TabText = this.File.FileInfo.Name + " (Read Only)";
                this.c_CodeEditor.NativeInterface.SetReadOnly(true);
            }

            // Add the code editor to the tab.
            this.Controls.Add(this.c_CodeEditor);
        }

        /// <summary>
        /// Highlights the specified line according to the specified style.
        /// </summary>
        /// <param name="line">The line number.</param>
        public void HighlightLine(int line)
        {
            this.c_CodeEditor.GoTo.Line(line);
            this.c_CodeEditor.GetRange(this.c_CodeEditor.Lines[line - 1].StartPosition, this.c_CodeEditor.Lines[line - 1].EndPosition).SetIndicator(0);
        }

        #region Event Handling

        /// <summary>
        /// This event is raised when the user has placed their mouse in the same position
        /// for a specified amount of time.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The mouse event information.</param>
        private void c_CodeEditor_DwellStart(object sender, ScintillaNet.ScintillaMouseEventArgs e)
        {
            // The user may want to find out what the error is.
            foreach (LuaError err in this.m_Errors)
            {
                if (e.Position >= err.IndicatorIndex && e.Position <= err.Index + err.Length)
                {
                    this.c_ToolTip.UseFading = false;
                    this.c_ToolTip.UseAnimation = false;
                    this.c_ToolTip.Show(err.ErrorMsg, this.c_CodeEditor, e.X + 20, e.Y + 10);
                }
            }
        }

        /// <summary>
        /// This event is raised when the user starts moving their mouse again after
        /// DwellStart has been previously raised.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The mouse event information.</param>
        private void c_CodeEditor_DwellEnd(object sender, ScintillaNet.ScintillaMouseEventArgs e)
        {
            this.c_ToolTip.Hide(this.c_CodeEditor);
        }

        /// <summary>
        /// This event is raised when the user releases a key while the code editor
        /// has focus.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The keyboard event information.</param>
        void c_CodeEditor_KeyUp(object sender, KeyEventArgs e)
        {
            if (this.c_CodeEditor.Text != this.m_SavedText)
                this.TabText = this.File.FileInfo.Name + " *";
            else
                this.TabText = this.File.FileInfo.Name;
        }

        /// <summary>
        /// This event is raised when a syntax check needs to be performed on the
        /// current document.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void c_CodeEditor_SyntaxCheckRequested(object sender, EventArgs e)
        {
            // Prevent flicker.
            if (this.c_CodeEditor.CanSyntaxCheck)
            {
                // TODO: Implement a refresh call to the Error List tool window.
            }

            // Clear all the indicators.
            this.c_CodeEditor.GetRange(0, this.c_CodeEditor.Text.Length).ClearIndicator(0);

            // Get errors.
            m_Errors = SyntaxChecker.Check(this.c_CodeEditor.Text, this.c_CodeEditor);

            // Apply highlighting to the errors.
            // TODO: Use a class variable to store the current filename.
            if (this.c_CodeEditor.CanSyntaxCheck)
            {
                // TODO: Clear all of the errors from the Error List tool window for
                //       this specific file.
            }
            foreach (LuaError err in m_Errors)
            {
                // Indicators have an offset by one.
                this.c_CodeEditor.GetRange(err.IndicatorIndex, err.Index + err.Length).SetIndicator(0);

                // TODO: Add the error to the error list.
                if (this.c_CodeEditor.CanSyntaxCheck)
                {
                }
            }

            // Prevent flicker.
            if (this.c_CodeEditor.CanSyntaxCheck)
            {
                // TODO: Implement a refresh call to the Error List tool window.
            }
        }

        /// <summary>
        /// This event is raised when the selected text area has been changed.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        void c_CodeEditor_SelectionChanged(object sender, EventArgs e)
        {
            this.Manager.CacheManager.Context.Object = this;
        }

        /// <summary>
        /// This event is raised when the code editor gains focus.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        void c_CodeEditor_GotFocus(object sender, EventArgs e)
        {
            this.Manager.CacheManager.Context.Object = this;
        }

        /// <summary>
        /// This event is raised when the code editor loses focus.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        void c_CodeEditor_LostFocus(object sender, EventArgs e)
        {
            this.Manager.CacheManager.Context.Object = null;
        }

        /// <summary>
        /// This event is raised when the left-side margin is clicked (i.e.
        /// the user wants to toggle breakpoint status of that line).
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The margin event information.</param>
        void c_CodeEditor_MarginClick(object sender, ScintillaNet.MarginClickEventArgs e)
        {
            // Check to make sure we are not the fold margin.
            if (e.Margin.IsFoldMargin)
                return;

            // Get the relative filename path.
            string relname = PathHelpers.GetRelativePath(this.File.Project.ProjectInfo.DirectoryName, this.File.FileInfo.FullName);

            // Check to see whether we are currently debugging (we can't add breakpoints during
            // execution at this time).
            if (Program.Manager.DebugManager.Running)
                return;

            // Check to see whether we should add or remove a breakpoint from this line.
            if (Program.Manager.DebugManager.Breakpoints.Count(result => result.SourceFile == relname && result.SourceLine == e.Line.Number + 1) == 0)
            {
                // Add breakpoint.
                Program.Manager.DebugManager.Breakpoints.Add(new Breakpoint(relname, (uint)e.Line.Number + 1));
                e.Line.DeleteAllMarkers();
                e.Line.AddMarker(0);
            }
            else
            {
                // Remove breakpoint.
                Program.Manager.DebugManager.Breakpoints.RemoveAll(result => result.SourceFile == relname && result.SourceLine == e.Line.Number + 1);
                e.Line.DeleteAllMarkers();
            }
        }

        #endregion

        #region Operation Implementions

        /// <summary>
        /// Boolean value indicating whether this designer can currently have data cut.
        /// </summary>
        bool ICuttable.CanCut
        {
            get { return this.c_CodeEditor.Selection.Length > 0; }
        }

        /// <summary>
        /// Boolean value indicating whether this designer can currently have data copied.
        /// </summary>
        bool ICopyable.CanCopy
        {
            get { return this.c_CodeEditor.Selection.Length > 0; }
        }

        /// <summary>
        /// Boolean value indicating whether this designer can be pasted into.
        /// </summary>
        bool IPastable.CanPaste
        {
            get { return true; }
        }

        /// <summary>
        /// Called when the user wants to cut data.
        /// </summary>
        void ICuttable.Cut()
        {
            this.c_CodeEditor.NativeInterface.Cut();
        }

        /// <summary>
        /// Called when the user wants to copy data.
        /// </summary>
        void ICopyable.Copy()
        {
            this.c_CodeEditor.NativeInterface.Copy();
        }

        /// <summary>
        /// Called when the user wants to remove data.
        /// </summary>
        void IDeletable.Delete()
        {
            this.c_CodeEditor.NativeInterface.DeleteBack();
        }

        /// <summary>
        /// Called when the user wants to paste data.
        /// </summary>
        void IPastable.Paste()
        {
            this.c_CodeEditor.NativeInterface.Paste();
        }

        /// <summary>
        /// Boolean value indicating whether the current data can be saved.
        /// </summary>
        bool ISavable.CanSave
        {
            get { return this.p_CanSave; }
        }

        /// <summary>
        /// The string value representing the name of the current data (e.g. file name).
        /// </summary>
        string ISavable.SaveName
        {
            get { return this.File.FileInfo.Name; }
        }

        /// <summary>
        /// Called when the user wants to save the file with it's current name.
        /// </summary>
        void ISavable.SaveFile()
        {
            using (System.IO.StreamWriter writer = new System.IO.StreamWriter(this.File.FileInfo.FullName, false))
            {
                writer.WriteLine(this.c_CodeEditor.Text);
            }
            this.TabText = this.File.FileInfo.Name;
            this.m_SavedText = this.c_CodeEditor.Text;
        }

        /// <summary>
        /// Called when the user wants to save the file as a different name.
        /// </summary>
        void ISavable.SaveFileAs()
        {
            SaveFileDialog sfd = new SaveFileDialog();
            sfd.AddExtension = true;
            sfd.DefaultExt = "lua";
            sfd.Filter = "Script Files|*.lua";
            sfd.InitialDirectory = this.File.FileInfo.DirectoryName;
            if (sfd.ShowDialog() == DialogResult.OK)
            {
                this.File = new File(null, null, sfd.FileName);
                (this as ISavable).SaveFile();
            }
        }

        #endregion

        #region Debugging Interface

        /// <summary>
        /// Called when a breakpoint has been hit and we should highlight it.
        /// </summary>
        /// <param name="file">The file that the breakpoint occurred in (should be the same as the file owned by the debugger).</param>
        /// <param name="line">The line number the breakpoint occurred on.</param>
        public void Debug(File file, uint line)
        {
            this.c_CodeEditor.Lines[(int)line - 1].AddMarker(1);
            this.c_CodeEditor.Lines[(int)line - 1].AddMarker(2);
            this.c_CodeEditor.GetRange(this.c_CodeEditor.Lines[(int)line - 1].StartPosition, this.c_CodeEditor.Lines[(int)line - 1].StartPosition).Select();
            this.m_ActiveLine = (int)line - 1;
        }

        /// <summary>
        /// Called when the breakpoint has continued and we should no longer highlight it.
        /// </summary>
        public void EndDebug()
        {
            this.c_CodeEditor.Lines[(int)this.m_ActiveLine].DeleteMarker(1);
            this.c_CodeEditor.Lines[(int)this.m_ActiveLine].DeleteMarker(2);
        }

        #endregion
    }
}
