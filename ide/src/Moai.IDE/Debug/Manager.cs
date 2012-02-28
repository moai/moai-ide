using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Diagnostics;
using System.IO;
using System.Threading;
using Moai.Debug.Messages;
using Moai.Platform.Debug;
using Moai.Platform.UI;
using Moai.Platform.Tools;
using Moai.Platform.Designers;

namespace Moai.Debug
{
    public class Manager : IDebugManager
    {
        private static string m_LaunchPath = PathHelpers.Sanitize(Path.Combine(Central.Manager.Settings["RootPath"], "Engines/Win32/Debug/moai.exe"));

        private IOutputTool m_OutputTool = null;
        private bool p_Paused = false;

        private Process m_Process = null;
        private Communicator m_Communicator = null;

        public event EventHandler DebugStart;
        public event EventHandler DebugPause;
        public event EventHandler DebugContinue;
        public event EventHandler DebugStop;

        private IDebuggable m_ActiveDesigner = null;
        private Breakpoint m_ActiveBreakpoint = null;

        /// <summary>
        /// Creates a new Manager class for managing debugging.
        /// </summary>
        /// <param name="parent">The main Moai manager which owns this debugging manager.</param>
        public Manager()
        {
            this.Breakpoints = new List<IBreakpoint>();
        }

        /// <summary>
        /// Runs the specified project with debugging.
        /// </summary>
        /// <param name="project">The project to run under the debugger.</param>
        public bool Start(Moai.Platform.Management.Project project)
        {
            // Check to see whether we are paused or not.
            if (this.p_Paused)
            {
                // Unpause, optionally sending an EndDebug call to
                // the appropriate place.
                if (this.m_ActiveDesigner != null)
                {
                    // Inform them we have stopped debugging.
                    this.m_ActiveDesigner.EndDebug();
                    this.m_ActiveDesigner = null;
                }

                // Now send the continue message.
                this.m_Communicator.Send(new ContinueMessage());
                this.p_Paused = false;
                if (this.DebugContinue != null)
                    this.DebugContinue(this, new EventArgs());
            }

            // Otherwise make sure we have no process running.
            if (this.m_Process != null)
            {
                // Can't run.
                return false;
            }

            // Check to see if the launch path exists.
            if (!File.Exists(Manager.m_LaunchPath))
            {
                Central.Platform.UI.ShowMessage(@"Moai IDE was unable to start debugging because it could not
locate the engine executable.  Ensure that you have installed
the engine executable in the required path and try again.", "Debugging Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            
            // Fire the event to say that debugging has started.
            if (this.DebugStart != null)
                this.DebugStart(this, new EventArgs());

            // Clear the existing output log.
            this.m_OutputTool = Central.Manager.ToolsManager.Get(typeof(IOutputTool)) as IOutputTool;
            if (this.m_OutputTool != null)
                this.m_OutputTool.ClearLog();

            // Start the debug listening service.
            try
            {
                this.m_Communicator = new Communicator(7018);
            }
            catch (ConnectionFailureException)
            {
                // It seems we can't start the debugging communicator.  Stop debugging
                // (forcibly terminate the process) and alert the user.
                if (this.DebugStop != null)
                    this.DebugStop(this, new EventArgs());
                Central.Platform.UI.ShowMessage(@"Moai IDE was unable to start debugging because it could not
listen or connect to the debugging socket.  Ensure there
are no other instances of the Moai engine running in debug
mode and try again.", "Debugging Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }
            this.m_Communicator.MessageArrived += new EventHandler<MessageEventArgs>(m_Communicator_MessageArrived);

            this.m_Process = new Process();
            this.m_Process.StartInfo.FileName = Manager.m_LaunchPath;
            this.m_Process.StartInfo.WorkingDirectory = project.ProjectInfo.Directory.FullName;
            this.m_Process.StartInfo.UseShellExecute = false;
            this.m_Process.StartInfo.Arguments = "Main.lua";
            this.m_Process.EnableRaisingEvents = true;
            this.m_Process.Exited += new EventHandler(m_Process_Exited);

            // FIXME: Find some way to make this work.  We need the Moai output to be completely unbuffered,
            //        and changing things around in .NET and on the engine side seems to make absolutely no
            //        difference what-so-ever.  My suggestion is to make the engine-side of the debugger replace
            //        the Lua print() function and send it over the network directly back to the IDE (this
            //        means that print would work even during remote debugging!)
            //
            // this.m_Process.StartInfo.RedirectStandardOutput = true;
            // this.m_Process.OutputDataReceived += new DataReceivedEventHandler(m_Process_OutputDataReceived);

            this.m_Process.Start();
            //this.m_Process.BeginOutputReadLine();

            this.p_Paused = false;
            return true;
        }

        /// <summary>
        /// Runs the specified project without debugging.
        /// </summary>
        /// <param name="project">The project to run without the debugger.</param>
        public bool StartWithout(Moai.Platform.Management.Project project)
        {
            if (this.m_Process != null)
            {
                // Can't run.
                return false;
            }

            // Check to see if the launch path exists.
            if (!File.Exists(Manager.m_LaunchPath))
            {
                Central.Platform.UI.ShowMessage(@"Moai was unable to start debugging because it could not
locate the engine executable.  Ensure that you have installed
the engine executable in the required path and try again.", "Debugging Failed", MessageBoxButtons.OK, MessageBoxIcon.Error);
                return false;
            }

            // Fire the event to say that debugging has started (even though
            // technically no debug events will be fired).
            if (this.DebugStart != null)
                this.DebugStart(this, new EventArgs());

            // Clear the existing output log.
            this.m_OutputTool = Central.Manager.ToolsManager.Get(typeof(IOutputTool)) as IOutputTool;
            if (this.m_OutputTool != null)
                this.m_OutputTool.ClearLog();

            // Start the process.
            this.m_Process = new Process();
            this.m_Process.StartInfo.FileName = Manager.m_LaunchPath;
            this.m_Process.StartInfo.WorkingDirectory = project.ProjectInfo.Directory.FullName;
            this.m_Process.StartInfo.UseShellExecute = false;
            this.m_Process.StartInfo.Arguments = "Main.lua";
            this.m_Process.EnableRaisingEvents = true;
            this.m_Process.Exited += new EventHandler(m_Process_Exited);

            this.m_Process.Start();
            this.p_Paused = false;
            return true;
        }

        /// <summary>
        /// Stops the debugging process that is currently underway.
        /// </summary>
        public void Pause()
        {
            if (this.m_Communicator != null)
                this.m_Communicator.Send(new PauseMessage());
        }

        /// <summary>
        /// Stops the debugging process that is currently underway.
        /// </summary>
        public void Stop()
        {
            if (this.m_ActiveDesigner != null && !Central.Manager.IDE.IsDisposed)
            {
                // Inform them we have stopped debugging.
                (this.m_ActiveDesigner as IDesigner).Invoke(new Action(() =>
                    {
                        this.m_ActiveDesigner.EndDebug();
                        this.m_ActiveDesigner = null;
                    }));
            }

            if (this.m_Process == null)
                return;
            if (!this.m_Process.HasExited)
                this.m_Process.Kill();
            this.m_Process = null;
            if (this.m_Communicator != null)
                this.m_Communicator.Close();
            this.m_Communicator = null;
            this.p_Paused = false;

            // Fire the event to say that debugging has stopped.
            if (this.DebugStop != null)
                this.DebugStop(this, new EventArgs());
        }

        /// <summary>
        /// Evaluates the specified Lua string by sending a message to the engine while
        /// debugging is paused.  If the engine is not paused, this method raises InvalidOperationException.
        /// </summary>
        /// <param name="expression">The expression to evaluate.</param>
        /// <param name="callback">The callback to issue when the expression has been evaluated.</param>
        public void Evaluate(string expression, Action<object> callback)
        {
            // Check to make sure we're paused.
            if (!this.Paused || !this.Running)
                throw new InvalidOperationException();

            // Attach a callback for MessageArrived.
            EventHandler<MessageEventArgs> ev = null;
            ev = new EventHandler<MessageEventArgs>((sender, e) =>
            {
                // Check to see whether it's a ResultMessage.
                if (e.Message is ResultMessage)
                {
                    // Unregister the event.
                    this.m_Communicator.MessageArrived -= ev;

                    // TODO: It would be good if the ResultMessage included the
                    //       original expression so we can be sure that the value
                    //       belongs to us.
                    callback((e.Message as ResultMessage).Value);
                }
            });
            this.m_Communicator.MessageArrived += ev;

            // Ask the engine to evaluate something (we will get a
            // response back via MessageArrived).
            this.m_Communicator.Send(new EvaluateMessage { Evaluation = expression });
        }

        /// <summary>
        /// This event is raised when the game sends a debugging message to the IDE.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        private void m_Communicator_MessageArrived(object sender, MessageEventArgs e)
        {
            // Invoke the message handling on the IDE's thread.
            Central.Manager.IDE.Invoke(new Action(() =>
            {
                if (e.Message is WaitMessage)
                {
                    // This is the game signalling that it is ready to receive
                    // message requests such as setting breakpoints before the
                    // game starts executing.
                    foreach (IBreakpoint b in this.Breakpoints)
                    {
                        BreakpointSetAlwaysMessage bm = new BreakpointSetAlwaysMessage();
                        bm.FileName = b.SourceFile;
                        bm.LineNumber = b.SourceLine;
                        this.m_Communicator.Send(bm);
                        Thread.Sleep(10); // Give the game a little bit of time to receive the message.
                    }

                    // After we have set breakpoints, we must tell the game to
                    // continue executing.
                    this.m_Communicator.Send(new ContinueMessage());
                }
                else if (e.Message is BreakMessage)
                {
                    // This is the game signalling that it has hit a breakpoint
                    // and is now paused.

                    // Open the designer window for the specified file.
                    Moai.Platform.Management.File f = Central.Manager.ActiveProject.GetByPath((e.Message as BreakMessage).FileName);
                    IDesigner d = Central.Manager.DesignersManager.OpenDesigner(f);
                    if (d is IDebuggable)
                    {
                        // We can only go to a specific line in the file if the
                        // designer supports it.
                        (d as IDebuggable).Debug(f, (e.Message as BreakMessage).LineNumber);

                        // Set current active line information so when we resume we can
                        // send the EndDebug call.
                        this.m_ActiveDesigner = d as IDebuggable;
                    }

                    // Inform the IDE that the game is now paused.
                    this.p_Paused = true;
                    if (this.DebugPause != null)
                        this.DebugPause(this, new EventArgs());
                }
                else if (e.Message is ExcpInternalMessage)
                {
                    /* FIXME: Implement this.
                    ExcpInternalMessage m = e.Message as ExcpInternalMessage;
                    ExceptionDialog d = new ExceptionDialog();
                    d.IDEWindow = this.p_Parent.IDEWindow;
                    d.MessageInternal = m;
                    d.Show(); */
                    // TODO: Indicate to the UI that the game is now paused.
                }
                else if (e.Message is ExcpUserMessage)
                {
                    /* FIXME: Implement this.
                    ExcpUserMessage m = e.Message as ExcpUserMessage;
                    ExceptionDialog d = new ExceptionDialog();
                    d.IDEWindow = this.p_Parent.IDEWindow;
                    d.MessageUser = m;
                    d.Show(); */
                    // TODO: Indicate to the UI that the game is now paused.
                }
                else if (e.Message is ResultMessage)
                {
                    ResultMessage m = e.Message as ResultMessage;
                    // TODO: Use a queue to track messages sent to the engine and match them up with the result messages.
                }
                else
                {
                    // Unknown message!
                    // TODO: Handle this properly?
                    Central.Platform.UI.ShowMessage(e.Message.ID);
                }
            }));
        }

        /// <summary>
        /// This event is raised when the game has exited during debugging.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        void m_Process_Exited(object sender, EventArgs e)
        {
            this.Stop();
        }

        /// <summary>
        /// The event is raised when the game or engine outputs to standard output
        /// and it's been redirected.
        /// </summary>
        /// <param name="sender">The sender of the event.</param>
        /// <param name="e">The event information.</param>
        void m_Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (this.m_OutputTool != null && e.Data != null)
            {
                System.Action lambda = () =>
                    {
                        this.m_OutputTool.AddLogEntry(e.Data);
                    };
                this.m_OutputTool.Invoke(lambda);
            }
        }

        /// <summary>
        /// Whether the program is currently running (either executing or
        /// in a paused state).
        /// </summary>
        public bool Running
        {
            get
            {
                return (this.m_Process != null);
            }
        }
        
        /// <summary>
        /// Whether the program is currently paused or not running.
        /// </summary>
        public bool Paused
        {
            get
            {
                return (this.m_Process == null) || this.p_Paused;
            }
        }

        /// <summary>
        /// A list of breakpoints that should be used during the
        /// execution of a program.
        /// </summary>
        public List<IBreakpoint> Breakpoints
        {
            get;
            private set;
        }
    }
}
