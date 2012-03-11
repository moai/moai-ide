using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Specialized;
using Moai.Platform.UI;
using System.IO;

namespace Moai.Platform.Linux.UI
{
    public class LinuxImageList : IImageList
    {
        private OrderedDictionary m_Images = new OrderedDictionary();

        #region Conversion Methods

        internal static Gtk.Image ConvertToGtk(Image image)
        {
            if (image == null) return null;
            return new Gtk.Image(LinuxImageList.ConvertToPixbuf(image));
        }

        internal static Gtk.Image ConvertToGtk(Icon icon)
        {
            if (icon == null) return null;
            return new Gtk.Image(LinuxImageList.ConvertToPixbuf(icon));
        }

        internal static Gdk.Pixbuf ConvertToPixbuf(Image image)
        {
            if (image == null) return null;
            using (MemoryStream stream = new MemoryStream())
            {
                image.Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(stream);
                return pixbuf;
            }
        }

        internal static Gdk.Pixbuf ConvertToPixbuf(Icon icon)
        {
            if (icon == null) return null;
            using (MemoryStream stream = new MemoryStream())
            {
                icon.ToBitmap().Save(stream, System.Drawing.Imaging.ImageFormat.Png);
                stream.Position = 0;
                Gdk.Pixbuf pixbuf = new Gdk.Pixbuf(stream);
                return pixbuf;
            }
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
