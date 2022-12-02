using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Processing
{
    public static class ByteImageProcessingExtensions
    {
        public static BaseImage<byte> AveragedChannel(this BaseImage<byte> image)
        {
            if (!image.IsValid) throw new ArgumentException($"AveragedChannel: Image {nameof(image)} is NOT valid.");

            if (image.Channels == 1) return image.Copy();

            var result = new BaseImage<byte>(image.Width, image.Height, 1);

            int width = image.Width;
            int height = image.Height;
            int channels = image.Channels;
            byte[]? data = result.Data;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    int total = 0;
                    foreach(var v in image.PixelSpan(x, y))
                    {
                        total += v;
                    }

                    data[y * width + x] = (byte)(total / channels);
                }
            }

            return result;
        }

        public static BaseImage<float> ToFloatImage(this BaseImage<byte> image, float multiplier = 1.0f)
        {
            if (!image.IsValid) throw new ArgumentException($"ToFloatImage: Image {nameof(image)} is NOT valid.");

            var result = new BaseImage<float>(image.Width, image.Height, image.Channels);

            int length = image.Height * image.Stride;

            byte[]? input = image.Data;
            float[]? output = result.Data;

            for (int i=0; i<length; i++)
            {
                output[i] = input[i] * multiplier;
            }

            return result;
        }

    }
}
