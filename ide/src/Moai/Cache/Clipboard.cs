using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Runtime.InteropServices;
using System.Windows.Forms;
using System.IO;
using System.Collections.Specialized;
using System.Security;

namespace Moai.Cache
{
    public static class Clipboard
    {
        private static Moai.Manager p_Manager = null;
        private static ClipboardControl m_Control = null;

        public static event EventHandler<ClipboardEventArgs> ClipboardChanged;

        /// <summary>
        /// Registers the clipboard cache object with the IDE's
        /// window handle.
        /// </summary>
        public static void Register(Moai.Manager manager)
        {
            Clipboard.p_Manager = manager;
            Clipboard.p_Manager.IDEOpened += new EventHandler((_a, _b) =>
            {
                Clipboard.m_Control = new ClipboardControl();
                Clipboard.m_Control.ClipboardChanged += new EventHandler((sender, e) =>
                {
                    if (Clipboard.ClipboardChanged != null)
                        Clipboard.ClipboardChanged(null, new ClipboardEventArgs(Clipboard.Contents));
                });
                Clipboard.p_Manager.IDEWindow.Controls.Add(Clipboard.m_Control);
            });
        }

        /// <summary>
        /// Gets or sets the contents of the clipboard.
        /// </summary>
        public static System.Windows.Forms.IDataObject Contents
        {
            get
            {
                return System.Windows.Forms.Clipboard.GetDataObject();
            }
            set
            {
                System.Windows.Forms.Clipboard.SetDataObject(value, true);
            }
        }

        /// <summary>
        /// Gets or sets the Wave audio stream stored on the clipboard.
        /// </summary>
        public static Stream ContentsAudio
        {
            get
            {
                if (System.Windows.Forms.Clipboard.ContainsAudio())
                    return System.Windows.Forms.Clipboard.GetAudioStream();
                else
                    return null;
            }
            set
            {
                System.Windows.Forms.Clipboard.SetAudio(value);
            }
        }

        /// <summary>
        /// Gets or sets the file list stored on the clipboard.
        /// </summary>
        public static StringCollection ContentsFileDropList
        {
            get
            {
                if (System.Windows.Forms.Clipboard.ContainsFileDropList())
                    return System.Windows.Forms.Clipboard.GetFileDropList();
                else
                    return null;
            }
            set
            {
                System.Windows.Forms.Clipboard.SetFileDropList(value);
            }
        }

        /// <summary>
        /// Gets or sets the text stored on the clipboard.
        /// </summary>
        public static string ContentsText
        {
            get
            {
                if (System.Windows.Forms.Clipboard.ContainsText())
                    return System.Windows.Forms.Clipboard.GetText();
                else
                    return null;
            }
            set
            {
                System.Windows.Forms.Clipboard.SetText(value);
            }
        }

        /// <summary>
        /// Clears the contents of the clipboard.
        /// </summary>
        public static void Clear()
        {
            System.Windows.Forms.Clipboard.Clear();
        }
    }

    /// <summary>
    /// Event arguments class for clipboard events.
    /// </summary>
    public class ClipboardEventArgs : EventArgs
    {
        public System.Windows.Forms.IDataObject DataObject { get; protected set; }

        public ClipboardEventArgs(System.Windows.Forms.IDataObject data)
        {
            this.DataObject = data;
        }
    }

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
            ClipboardControl.ChangeClipboardChain(this.Handle, this.m_NextClipboardViewer);
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
