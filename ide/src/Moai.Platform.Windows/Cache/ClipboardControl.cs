using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;

namespace Moai.Platform.Windows.Cache
{
    /// <summary>
    /// Clipboard control; adapted from http://stackoverflow.com/questions/621577/clipboard-event-c.
    /// </summary>
    public class ClipboardControl : Control
    {
        private IntPtr m_NextClipboardViewer;

        public event EventHandler ClipboardChanged;

        // Native imports; these will need to be adjusted for platforms
        // other than Windows.
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern IntPtr SetClipboardViewer(IntPtr hWndNewViewer);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern int SendMessage(IntPtr hWnd, int wMsg, IntPtr wParam, IntPtr lParam);
        [DllImport("User32.dll", CharSet = CharSet.Auto)]
        private static extern bool ChangeClipboardChain(IntPtr hWndRemove, IntPtr hWndNewNext);

        /// <summary>
        /// Creates a new clipboard control; this must be added to a form
        /// in order to receive clipboard events.
        /// </summary>
        public ClipboardControl()
        {
            this.m_NextClipboardViewer = ClipboardControl.SetClipboardViewer(this.Handle);
        }

        protected override void Dispose(bool disposing)
        {
            Action a = () => { ClipboardControl.ChangeClipboardChain(this.Handle, this.m_NextClipboardViewer); };
            if (this.InvokeRequired)
                this.Invoke(a);
            else
                a();
        }

        protected override void WndProc(ref Message m)
        {
            // Defined in winuser.h
            const int WM_DRAWCLIPBOARD = 0x308;
            const int WM_CHANGECBCHAIN = 0x030D;

            switch (m.Msg)
            {
                case WM_DRAWCLIPBOARD:
                    // The clipboard contents have been changed.
                    if (this.ClipboardChanged != null)
                        this.ClipboardChanged(this, new EventArgs());
                    ClipboardControl.SendMessage(this.m_NextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                case WM_CHANGECBCHAIN:
                    // Another clipboard viewer has been registered in
                    // the chain.
                    if (m.WParam == this.m_NextClipboardViewer)
                        this.m_NextClipboardViewer = m.LParam;
                    else
                        ClipboardControl.SendMessage(this.m_NextClipboardViewer, m.Msg, m.WParam, m.LParam);
                    break;
                default:
                    base.WndProc(ref m);
                    break;
            }
        }
    }
}
