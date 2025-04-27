using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace scoi_4
{
    using System;
    using System.Drawing;
    using System.Drawing.Imaging;
    using System.Runtime.InteropServices;

    public class ImageProcessor
    {
        public byte[] Pixels { get; set; }
        public int Width { get; }
        public int Height { get; }
        public int Stride { get; }
        public int BytesPerPixel { get; }

        public ImageProcessor(Bitmap bmp)
        {
            Width = bmp.Width;
            Height = bmp.Height;

            var rect = new Rectangle(0, 0, Width, Height);
            var bmpData = bmp.LockBits(rect, ImageLockMode.ReadOnly, PixelFormat.Format24bppRgb);

            Stride = bmpData.Stride;
            BytesPerPixel = Image.GetPixelFormatSize(PixelFormat.Format24bppRgb) / 8;
            Pixels = new byte[Stride * Height];

            Marshal.Copy(bmpData.Scan0, Pixels, 0, Pixels.Length);
            bmp.UnlockBits(bmpData);
        }

        public Bitmap ToBitmap()
        {
            Bitmap result = new Bitmap(Width, Height, PixelFormat.Format24bppRgb);
            var rect = new Rectangle(0, 0, Width, Height);
            var bmpData = result.LockBits(rect, ImageLockMode.WriteOnly, PixelFormat.Format24bppRgb);
            Marshal.Copy(Pixels, 0, bmpData.Scan0, Pixels.Length);
            result.UnlockBits(bmpData);
            return result;
        }

        // Зеркальное отражение по краям
        public byte GetPixelSafe(int x, int y, int channel)
        {
            x = Reflect(x, Width);
            y = Reflect(y, Height);
            int index = y * Stride + x * BytesPerPixel;
            return Pixels[index + channel];
        }

        private int Reflect(int value, int max)
        {
            if (value < 0) return -value;
            if (value >= max) return 2 * max - value - 2;
            return value;
        }
    }

}
