using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Processing.Extensions
{
    public static class ByteImagePolarizerExtensions
    {
        public static BaseImage<T> PolarizerChannel<T>(this BaseImage<T> image, int channelIndex)
        {
            if (!image.IsValid) throw new ArgumentException($"{nameof(SingleChannel)}: {nameof(image)} is NOT valid.");

            if (channelIndex > image.Channels) throw new ArgumentOutOfRangeException($"{nameof(SingleChannel)}: {nameof(channelIndex)} is out of range.");

            var result = new BaseImage<T>(image.Width, image.Height, 1);

            int width = image.Width;
            int height = image.Height;

            for (int y = 0; y < height; y++)
            {
                for (int x = 0; x < width; x++)
                {
                    image.PixelSpan(x, y).Slice(channelIndex, 1).CopyTo(result.PixelSpan(x, y));
                }
            }

            return result;
        }
    }
}
