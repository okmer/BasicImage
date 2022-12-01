using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Runtime.InteropServices;

namespace Com.Okmer.BasicImage.WinForms.Extensions
{
    public static class ByteImageBitmapExtensions
    {
        public static Bitmap ToBitmap(this ByteImage image)
        {
            if (!image.IsValid) throw new ArgumentException("ToBitmap: Image is NOT valid.");

            if (image.Data is null) throw new ArgumentNullException("ToBitmap: Image.Data is Null.");

            PixelFormat pixelFormat;

            switch (image.Channels)
            {
                case 1:
                    pixelFormat = PixelFormat.Format8bppIndexed;
                    break;
                case 2:
                    pixelFormat = PixelFormat.Format16bppGrayScale;
                    break;
                case 3:
                    pixelFormat = PixelFormat.Format24bppRgb;
                    break;
                default:
                    pixelFormat = PixelFormat.Format32bppArgb;
                    break;

            }

            //Create bitmap image
            var bitmap = new Bitmap(image.Width, image.Height, pixelFormat);

            // Lock the bitmap data
            Rectangle rect = new Rectangle(0, 0, bitmap.Width, bitmap.Height);

            BitmapData bitmapData =
                bitmap.LockBits(rect, ImageLockMode.ReadWrite,
                    bitmap.PixelFormat);

            // Get the address of the first byte
            var ptr = bitmapData.Scan0;

            // Copy the image array into the bitmap data.
            Marshal.Copy(image.Data, 0, ptr, image.Height * image.Stride);

            // Unlock bitmap data
            bitmap.UnlockBits(bitmapData);

            return bitmap;
        }
    }
}
