using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Moai.Platform.UI;

namespace Moai.Platform.Windows.UI
{
    public class WindowsImageList : IImageList
    {
        private ImageList m_ImageList = null;

        public WindowsImageList()
        {
            this.m_ImageList = new ImageList();
        }

        #region IImageList Members

        public void Add(System.Drawing.Image image)
        {
            this.m_ImageList.Images.Add(image);
        }

        public void Add(string name, System.Drawing.Image image)
        {
            this.m_ImageList.Images.Add(name, image);
        }

        public int Count
        {
            get { return this.m_ImageList.Images.Count; }
        }

        public System.Drawing.Image this[int i]
        {
            get { return this.m_ImageList.Images[i]; }
        }

        public System.Drawing.Image this[string n]
        {
            get { return this.m_ImageList.Images[n]; }
        }

        public System.Collections.Specialized.StringCollection Keys
        {
            get { return this.m_ImageList.Images.Keys; }
        }

        #endregion

        #region IImageList Members

        public T ConvertTo<T>() where T : class
        {
            if (typeof(T) == typeof(ImageList))
                return this.m_ImageList as T;
            else
                return null;
        }

        #endregion
    }
}
