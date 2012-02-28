using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using Moai.Platform.Windows.Cache;
using System.Windows.Forms;

namespace Moai.Platform.Windows
{
    public class WindowsClipboard : IClipboard
    {
        private static ClipboardControl m_Control = null;

        public WindowsClipboard()
        {
            WindowsClipboard.m_Control = new ClipboardControl();
            if (Central.Manager.IDE == null || (Central.Manager.IDE as WindowsIDE).Visible == false)
                Central.Manager.IDEOpened += new EventHandler((_a, _b) => { this.Register(); });
            else
                this.Register();
        }

        private void Register()
        {
            WindowsClipboard.m_Control = new ClipboardControl();
            WindowsClipboard.m_Control.ClipboardChanged += new EventHandler((sender, e) =>
            {
                if (this.ContentsChanged != null)
                    this.ContentsChanged(null, new EventArgs());
            });
            (Central.Manager.IDE as WindowsIDE).Controls.Add(WindowsClipboard.m_Control);
        }

        #region IClipboard Members

        public event EventHandler ContentsChanged;

        public ClipboardContentType Type
        {
            get
            {
                if (System.Windows.Forms.Clipboard.ContainsFileDropList())
                    return ClipboardContentType.FileDrop;
                else if (System.Windows.Forms.Clipboard.ContainsText())
                    return ClipboardContentType.Text;
                else
                    throw new NotSupportedException();
            }
        }

        public object Contents
        {
            get
            {
                switch (this.Type)
                {
                    case ClipboardContentType.FileDrop:
                        return System.Windows.Forms.Clipboard.GetFileDropList();
                    case ClipboardContentType.Text:
                        return System.Windows.Forms.Clipboard.GetText();
                    default:
                        throw new NotSupportedException();
                }
            }
            private set
            {
                System.Windows.Forms.Clipboard.SetDataObject(value, true);
            }
        }

        public void Cut(ClipboardContentType type, object contents)
        {
            System.Windows.Forms.IDataObject data = new System.Windows.Forms.DataObject(this.GetDataFormatByContentType(type), contents);
            if (type == ClipboardContentType.FileDrop)
            {
                MemoryStream stream = new MemoryStream(4);
                byte[] bytes = new byte[] { 2, 0, 0, 0 };
                stream.Write(bytes, 0, bytes.Length);
                data.SetData("Preferred DropEffect", stream);
            }
            this.Contents = data;
        }

        public void Copy(ClipboardContentType type, object contents)
        {
            System.Windows.Forms.IDataObject data = new System.Windows.Forms.DataObject(this.GetDataFormatByContentType(type), contents);
            if (type == ClipboardContentType.FileDrop)
            {
                MemoryStream stream = new MemoryStream(4);
                byte[] bytes = new byte[] { 5, 0, 0, 0 };
                stream.Write(bytes, 0, bytes.Length);
                data.SetData("Preferred DropEffect", stream);
            }
            this.Contents = data;
        }

        public void Clear()
        {
            System.Windows.Forms.Clipboard.Clear();
        }

        #endregion

        #region Enumeration Conversion

        private string GetDataFormatByContentType(ClipboardContentType type)
        {
            switch (type)
            {
                case ClipboardContentType.FileDrop:
                    return System.Windows.Forms.DataFormats.FileDrop;
                case ClipboardContentType.Text:
                    return System.Windows.Forms.DataFormats.Text;
                default:
                    throw new NotSupportedException();
            }
        }

        #endregion
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
}
