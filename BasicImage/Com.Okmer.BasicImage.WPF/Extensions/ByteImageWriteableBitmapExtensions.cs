using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Com.Okmer.BasicImage.WPF
{
    public static class ByteImageWriteableBitmapExtensions
    {
        public static Int32Rect GetFullFrameRectangle(this BaseImage<byte> image, int xOffset = 0, int yOffset = 0) => new Int32Rect(xOffset, yOffset, image.Width, image.Height);

        public static WriteableBitmap ToWriteableBitmap(this BaseImage<byte> image)
        {
            PixelFormat pixelFormat;

            switch (image.Channels)
            {
                case 1:
                    pixelFormat = PixelFormats.Gray8;
                    break;
                case 2:
                    pixelFormat = PixelFormats.Gray16;
                    break;
                case 3:
                    pixelFormat = PixelFormats.Bgr24;
                    break;
                default:
                    pixelFormat = PixelFormats.Bgr32;
                    break;

            }

            WriteableBitmap result = new WriteableBitmap(image.Width, image.Height, 96, 96, pixelFormat, null);
            result.WritePixels(image.GetFullFrameRectangle(), image.Data, image.Stride, 0);
            return result;
        }

        public static BitmapSource ToBitmapSource(this BaseImage<byte> image) => image.ToWriteableBitmap();

        public static BaseImage<byte> ToByteImage(this BitmapSource bitmap)
        {
            var bytesPerPixel = (bitmap.Format.BitsPerPixel + 7) / 8;
            var result = new BaseImage<byte>(bitmap.PixelWidth, bitmap.PixelHeight, bytesPerPixel);
            bitmap.CopyPixels(result.Data, result.Stride, 0);
            return result;
        }
    }
}
