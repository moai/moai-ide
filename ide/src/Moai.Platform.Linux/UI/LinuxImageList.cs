using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Specialized;
using Moai.Platform.UI;
using System.IO;
using Qyoto;

namespace Moai.Platform.Linux.UI
{
    public class LinuxImageList : IImageList
    {
        private OrderedDictionary m_Images = new OrderedDictionary();

        #region Conversion Methods
    	
        internal static QIcon ConvertToQIcon(Icon icon)
        {
            // FIXME: On Linux systems we should use some sort of
            // icon caching mechanism so we're not writing out the
            // every single time we want to use it.
            string fname = Path.GetTempFileName();
            icon.ToBitmap().Save(fname, System.Drawing.Imaging.ImageFormat.Png);
            QIcon result = new QIcon(fname);
            File.Delete(fname);
            return result;
        }
        
        internal static QImage ConvertToQImage(Image image)
        {
            // FIXME: On Linux systems we should use some sort of
            // icon caching mechanism so we're not writing out the
            // every single time we want to use it.
            string fname = Path.GetTempFileName();
            image.Save(fname, System.Drawing.Imaging.ImageFormat.Png);
            QImage result = new QImage(fname);
            File.Delete(fname);
            return result;
        }

        #endregion

        #region IImageList Members

        public void Add(Image image)
        {
            this.m_Images.Add(this.m_Images.Count.ToString(), image);
        }

        public void Add(string name, Image image)
        {
            this.m_Images.Add(name, image);
        }

        public T ConvertTo<T>() where T : class
        {
            throw new NotImplementedException();
        }

        public int Count
        {
            get { return this.m_Images.Count; }
        }

        public System.Drawing.Image this[int i]
        {
            get { return (Image)this.m_Images[i]; }
        }

        public System.Drawing.Image this[string n]
        {
            get { return (Image)this.m_Images[n]; }
        }

        public System.Collections.Specialized.StringCollection Keys
        {
            get
            {
                StringCollection sc = new StringCollection();
                foreach (string s in this.m_Images.Keys)
                    sc.Add(s);
                return sc;
            }
        }

        #endregion
    }
}
