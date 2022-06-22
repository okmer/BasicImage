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

        public static T[] GetPixel<T>(this BaseImage<T> image, int x, int y) => image?.Data?.Skip(y * image.Stride + x * image.Channels).Take(image.Channels).ToArray() ?? Array.Empty<T>();
    }
}
