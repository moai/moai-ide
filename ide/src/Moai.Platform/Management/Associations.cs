using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Drawing;
using System.IO;
using Moai.Platform.UI;
using Moai.Platform.Designers;

namespace Moai.Platform.Management
{
    public static class Associations
    {
        private static IImageList p_ImageList;
        private static HandlerDictionary p_Handlers = new HandlerDictionary
            {
                { "lua", "Script", typeof(ICodeDesigner) },
                { "xml", "XML", typeof(ICodeDesigner) },
                { "ogg", "Audio", null },
                { "wav", "Audio", null },
                { "png", "Image", null },
                { "bmp", "Image", null },
                { "jpg", "Image", null },
                { "tga", "Image", null },
                { "pcx", "Image", null },
                { "psd", "Image", null },
                { "3ds", "Model", null },
                { "b3d", "Model", null },
                { "csm", "Model", null },
                { "dae", "Model", null },
                { "dmf", "Model", null },
                { "x", "Model", null },
                { "obj", "Model", null },
                { "my3d", "Model", null },
                { "oct", "Model", null },
                { "mesh", "Model", null },
                { "lmts", "Model", null },
                { "bsp", "Model", null },
                { "md2", "Model", null },
                { "md3", "Model", null },
                { "rkm", "Model", null },
                { "c", "CScript", null },
                { "cc", "CScript", null },
                { "cpp", "CScript", null },
                { "h", "CHeader", null },
                { "hpp", "CHeader", null },
                { "folder", "Folder", null }
            };

        private static Bitmap m_ReprImage = IDE.Resources.Images.image;

        /// <summary>
        /// Initalizes the static image list.
        /// </summary>
        static Associations()
        {
            // Create image list.
            Associations.p_ImageList = Central.Platform.UI.CreateImageList();

            // Add images.
            Associations.p_ImageList.Add("Solution", IDE.Resources.Images.solution);
            Associations.p_ImageList.Add("Project", IDE.Resources.Images.project);
            Associations.p_ImageList.Add("Model", IDE.Resources.Images.model);
            Associations.p_ImageList.Add("Folder", IDE.Resources.Images.folder);
            Associations.p_ImageList.Add("Audio", IDE.Resources.Images.audio);
            Associations.p_ImageList.Add("Area", IDE.Resources.Images.area);
            Associations.p_ImageList.Add("Image", IDE.Resources.Images.image);
            Associations.p_ImageList.Add("ProjectReference", IDE.Resources.Images.projectreference);
            Associations.p_ImageList.Add("Reference", IDE.Resources.Images.reference);
            Associations.p_ImageList.Add("Script", IDE.Resources.Images.script);
            Associations.p_ImageList.Add("CHeader", IDE.Resources.Images.cheader);
            Associations.p_ImageList.Add("CScript", IDE.Resources.Images.cscript);
            Associations.p_ImageList.Add("World", IDE.Resources.Images.world);
            Associations.p_ImageList.Add("NotFound", IDE.Resources.Images.not_found);
            Associations.p_ImageList.Add("NotFoundFolder", IDE.Resources.Images.not_found_folder);

            // Generate faded versions of the icons for cut operations.
            int count = Associations.p_ImageList.Count;
            for (int i = 0; i < count; i += 1)
            {
                Bitmap b = new Bitmap(Associations.p_ImageList[i].Width, Associations.p_ImageList[i].Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
                Graphics g = Graphics.FromImage(b);
                g.DrawImage(Associations.p_ImageList[i], new Point(0, 0));
                for (int x = 0; x < b.Width; x += 1)
                    for (int y = 0; y < b.Height; y += 1)
                    {
                        Color col = b.GetPixel(x,y);
                        if (col.A > 0)
                            b.SetPixel(x, y, Color.FromArgb(128, col));
                    }
                Associations.p_ImageList.Add(Associations.p_ImageList.Keys[i] + ":Faded", b);
            }
        }

        /// <summary>
        /// Handles requests to open image files by opening the appropriate
        /// designer.
        /// </summary>
        /// <param name="manager"></param>
        /// <param name="path"></param>
        public static void ImageHandler(string path)
        {

        }

        /// <summary>
        /// A public function which provides access to the image representations
        /// for extensions.  Returns null if there is no appropriate representation.
        /// </summary>
        /// <param name="ext">The extension to fetch the representation for.</param>
        public static Image GetImageData(string ext)
        {
            foreach (KeyValuePair<string, HandlerDetails> h in Associations.p_Handlers)
                if (ext.ToLowerInvariant() == h.Key.ToLowerInvariant())
                    return Associations.ImageList[h.Value.ImageKey];

            return null;
        }

        /// <summary>
        /// A public function which provides access to the image keys for situations
        /// which involve the use of the ImageList property to access images.  Returns
        /// null if there is no appropriate representation.
        /// </summary>
        /// <param name="ext">The extension to fetch the representation for.</param>
        public static string GetImageKey(string ext)
        {
            foreach (KeyValuePair<string, HandlerDetails> h in Associations.p_Handlers)
                if (ext.ToLowerInvariant() == h.Key.ToLowerInvariant())
                    return h.Value.ImageKey;

            return null;
        }

        /// <summary>
        /// A public function which returns the type of designer for this filetype.
        /// Returns null if there is no appropriate representation.
        /// </summary>
        public static Type GetDesignerType(string ext)
        {
            foreach (KeyValuePair<string, HandlerDetails> h in Associations.p_Handlers)
                if (ext.ToLowerInvariant() == h.Key.ToLowerInvariant())
                    return h.Value.DesignerType;

            return null;
        }

        /// <summary>
        /// The static image list.
        /// </summary>
        public static IImageList ImageList
        {
            get
            {
                return Associations.p_ImageList;
            }
        }
    }

    /// <summary>
    /// A derived class that allows the handler dictionary to specify both the key, handler
    /// and the image representation of the type (for use in solution explorer)
    /// </summary>
    public class HandlerDictionary : Dictionary<string, HandlerDetails>
    {
        public void Add(string key, string image, Type designer)
        {
            base.Add(key, new HandlerDetails(image, designer));
        }
    }

    public struct HandlerDetails
    {
        public Type DesignerType;
        public string ImageKey;

        public HandlerDetails(string image, Type designer)
        {
            this.DesignerType = designer;
            this.ImageKey = image;
        }
    }
}
