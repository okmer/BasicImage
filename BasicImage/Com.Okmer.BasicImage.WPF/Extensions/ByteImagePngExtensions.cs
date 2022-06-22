using System;
using System.IO;
using System.Windows.Media.Imaging;

namespace Com.Okmer.BasicImage.WPF
{
    public static class ByteImagePngExtensions
    {
        public static byte[] ToPng(this BitmapSource image)
        {
            PngBitmapEncoder encoder = new PngBitmapEncoder();
            BitmapFrame outputFrame = BitmapFrame.Create(image);
            encoder.Frames.Add(outputFrame);
            using (var output = new MemoryStream())
            {
                encoder.Save(output);
                return output.ToArray();
            }
        }

        public static byte[] ToPng(this ByteImage image) => ToPng(image.ToWriteableBitmap());

        public static void ToPngFile(this BitmapSource image, string fileName)
        {
            using (Stream file = File.Create(fileName))
            {
                file.Write(image.ToPng());
            }
        }

        public static void ToPngFile(this ByteImage image, string fileName) => ToPngFile(image.ToWriteableBitmap(), fileName);

        public static ByteImage ByteImageFromPng(this byte[] data)
        {
            using (var input = new MemoryStream(data))
            {
                var decoder = new PngBitmapDecoder(input, BitmapCreateOptions.None, BitmapCacheOption.None);
                BitmapFrame frame = decoder.Frames[0];
                var bytesPerPixel = (frame.Format.BitsPerPixel + 7) / 8;
                var image = new ByteImage(frame.PixelWidth, frame.PixelHeight, bytesPerPixel);
                frame.CopyPixels(image.Data, image.Stride, 0);
                return image;
            }
        }
    }
}
