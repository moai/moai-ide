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
            return LinuxImageList.ConvertToQIcon(icon.ToBitmap());
        }

        internal static QIcon ConvertToQIcon(Image image)
        {
            QImage ms = ConvertToQImage(image);
            QPixmap px = QPixmap.FromImage(ms);
            return new QIcon(px);
        }

        internal static QImage ConvertToQImage(Icon icon)
        {
            return LinuxImageList.ConvertToQImage(icon.ToBitmap());
        }

        internal static QImage ConvertToQImage(Image image)
        {
            QImage result;
            using (MemoryStream ms = new MemoryStream())
            {
                image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                result = QImage.FromData(new QByteArray(ms.GetBuffer()), "PNG");
            }
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
