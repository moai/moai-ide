using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.Collections.Specialized;

namespace Moai.Platform.UI
{
    public interface IImageList : IProxable
    {
        void Add(Image image);
        void Add(string name, Image image);
        T ConvertTo<T>() where T : class;

        int Count { get; }
        Image this[int i] { get; }
        Image this[string n] { get; }
        StringCollection Keys { get; }
    }
}
