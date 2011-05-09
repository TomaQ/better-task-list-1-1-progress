using System.Drawing;
using System.Drawing.Drawing2D;

namespace BetterTaskList.Extensions
{
    public static class ImageExtensions
    {
        /// <summary>
        /// Proportionately resizes an image to the specified dimensions
        /// </summary>
        /// <param name="src">The image to be resized</param>
        /// <param name="w">The new width</param>
        /// <param name="h">The new height</param>
        /// <returns>The resized image</returns>
        public static Image ResizeTo(this Image src, int w, int h)
        {
            var bmp = new Bitmap(w, h);

            var rect = new Rectangle(0, 0, w, h);
            if (src.Height > src.Width)
            {
                rect.Width = (int)((double)rect.Width * ((double)src.Width / (double)src.Height));
                rect.X += (w - rect.Width) / 2;
            }
            else
            {
                rect.Height = (int)((double)rect.Height * ((double)src.Height / (double)src.Width));
                rect.Y += (h - rect.Height) / 2;
            }

            using (var g = Graphics.FromImage(bmp))
            {
                g.SmoothingMode = SmoothingMode.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.PixelOffsetMode = PixelOffsetMode.HighQuality;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.DrawImage(src, rect, 0, 0, src.Width, src.Height, GraphicsUnit.Pixel);
            }

            return bmp;
        }
    }
}