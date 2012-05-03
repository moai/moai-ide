using System;
using Moai.Platform.Designers;

namespace Moai.Platform.Linux.Designers.Code
{
    public partial class Designer : Moai.Designers.Designer, ICodeDesigner
    {
        public Designer()
        {
        }

        #region IDebuggable Members

        public void Debug(Moai.Platform.Management.File file, uint line)
        {
            throw new NotImplementedException();
        }

        public void EndDebug()
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISavable Members

        public void SaveFile()
        {
            throw new NotImplementedException();
        }

        public void SaveFileAs()
        {
            throw new NotImplementedException();
        }

        public bool CanSave
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        public string SaveName
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IDeletable Members

        public void Delete()
        {
            throw new NotImplementedException();
        }

        public bool CanDelete
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region IPastable Members

        public void Paste()
        {
            throw new NotImplementedException();
        }

        public bool CanPaste
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICopyable Members

        public void Copy()
        {
            throw new NotImplementedException();
        }

        public bool CanCopy
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion

        #region ICuttable Members

        public void Cut()
        {
            throw new NotImplementedException();
        }

        public bool CanCut
        {
            get
            {
                throw new NotImplementedException();
            }
        }

        #endregion
    }
}

