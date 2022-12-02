using System;
using System.IO;
using System.Windows;
using System.Windows.Media;
using System.Windows.Media.Imaging;

namespace Com.Okmer.BasicImage.WPF
{
    public static class ByteImageJpegExtensions
    {
        public static byte[] ToJpeg(this BitmapSource image, int quality = 100)
        {
            JpegBitmapEncoder encoder = new JpegBitmapEncoder();
            BitmapFrame outputFrame = BitmapFrame.Create(image);
            encoder.Frames.Add(outputFrame);
            encoder.QualityLevel = quality;
            using (var output = new MemoryStream())
            {
                encoder.Save(output);
                return output.ToArray();
            }
        }

        public static byte[] ToJpeg(this BaseImage<byte> image, int quality = 100) => ToJpeg(image.ToWriteableBitmap(), quality);

        public static void ToJpegFile(this BitmapSource image, string fileName, int quality = 100)
        {
            using (Stream file = File.Create(fileName))
            {
                file.Write(image.ToJpeg(quality));
            }
        }

        public static void ToJpegFile(this BaseImage<byte> image, string fileName, int quality = 100) => ToJpegFile(image.ToWriteableBitmap(), fileName, quality);

        public static BaseImage<byte> ByteImageFromJpeg(this byte[] data)
        {
            using (var input = new MemoryStream(data))
            {
                var decoder = new JpegBitmapDecoder(input, BitmapCreateOptions.None, BitmapCacheOption.None);
                BitmapFrame frame = decoder.Frames[0];
                var bytesPerPixel = (frame.Format.BitsPerPixel + 7) / 8;
                var image = new BaseImage<byte>(frame.PixelWidth, frame.PixelHeight, bytesPerPixel);
                frame.CopyPixels(image.Data, image.Stride, 0);
                return image;
            }
        }
    }
}
