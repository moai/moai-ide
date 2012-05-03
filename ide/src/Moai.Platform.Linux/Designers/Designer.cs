using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Moai.Platform;
using Moai.Platform.Management;
using Moai.Platform.Designers;
using Qyoto;

namespace Moai.Designers
{
    public class Designer : QWidget, IDesigner
    {
        private QTabWidget m_Owner = null;
        private string m_TabText = "<unset>";
        private QIcon m_TabIcon = null;

        public Designer()
        {
        }

        protected override void ShowEvent(QShowEvent arg1)
        {
            base.ShowEvent(arg1);
            if (this.Opened != null)
                this.Opened(this, new EventArgs());
        }

        protected override void HideEvent(QHideEvent arg1)
        {
            base.HideEvent(arg1);
            if (this.Closed != null)
                this.Closed(this, new EventArgs());
        }

        /// <summary>
        /// Creates a new Designer base object.
        /// </summary>
        /// <param name="file">The associated file.</param>
        public Designer(File file)
        {
            this.File = file;
            if (this.Parent() is QTabWidget)
            {
                this.m_Owner = this.Parent() as QTabWidget;
                this.m_Owner.SetTabText(this.m_Owner.IndexOf(this), this.m_TabText);
            }
            if (this.File != null)
                this.TabText = this.File.FileInfo.Name;
        }

        public void SwitchParent(QObject arg1)
        {
            base.SetParent(arg1);
            if (this.Parent() is QTabWidget)
            {
                this.m_Owner = this.Parent() as QTabWidget;
                this.m_Owner.SetTabText(this.m_Owner.IndexOf(this), this.m_TabText);
            }
            else
                this.m_Owner = null;
        }

        public string TabText
        {
            get
            {
                return this.m_TabText;
            }
            set
            {
                this.m_TabText = value;
                if (this.m_Owner != null)
                    this.m_Owner.SetTabText(this.m_Owner.IndexOf(this), this.m_TabText);
            }
        }

        public QIcon TabIcon
        {
            get
            {
                return this.m_TabIcon;
            }
            set
            {
                this.m_TabIcon = value;
                if (this.m_Owner != null && this.m_TabIcon != null)
                    this.m_Owner.SetTabIcon(this.m_Owner.IndexOf(this), this.m_TabIcon);
            }
        }

        /// <summary>
        /// The path to the file that this editor is currently editing.
        /// </summary>
        public string Path
        {
            get;
            protected set;
        }

        /// <summary>
        /// The File object that represents the file that this editor is currently editing.
        /// Only valid if the Path points to a file that is currently in the project.
        /// </summary>
        public File File
        {
            get;
            protected set;
        }

        #region IDesigner Members

        public event EventHandler Closed;
        public event EventHandler Opened;

        #endregion

        #region IHasInvoke Members

        public object Invoke(Delegate method)
        {
            return method.DynamicInvoke(null);
        }

        public bool InvokeRequired
        {
            get
            {
                return false;
            }
        }

        #endregion

    }
}
