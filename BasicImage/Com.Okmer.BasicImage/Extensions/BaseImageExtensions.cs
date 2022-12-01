using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage
{
    public static class BaseImageExtensions
    {
        public static BaseImage<T> Copy<T>(this BaseImage<T> image)
        {
            var result = new BaseImage<T>(image.Width, image.Height, image.Channels);

            if (image.Data is not null && result.Data is not null) Array.Copy(image.Data, result.Data, image.Stride * image.Height);

            return result;
        }

        public static Span<T> GetPixelSpan<T>(this BaseImage<T> image, int x, int y) => image.Data.AsSpan(y * image.Stride + x * image.Channels, image.Channels);

        public static T[] GetPixelArray<T>(this BaseImage<T> image, int x, int y) => image.GetPixelSpan(x, y).ToArray();

        public static Span<T> GetLineSpan<T>(this BaseImage<T> image, int y) => image.Data.AsSpan(y * image.Stride, image.Stride);

        public static T[] GetLineArray<T>(this BaseImage<T> image, int x, int y) => image.GetLineSpan(y).ToArray();
    }
}
