using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Processing.Extensions
{
    public static class ByteImagePolarizerExtensions
    {
        public static List<BaseImage<T>> SplitPolarizers<T>(this BaseImage<T> image, int polarizersHorizontal, int polarizersVectical)
        {
            if (!image.IsValid) throw new ArgumentException($"{nameof(SplitPolarizers)}: {nameof(image)} is NOT valid.");

            if (polarizersHorizontal < 1) throw new ArgumentOutOfRangeException($"{nameof(SplitPolarizers)}: {nameof(polarizersHorizontal)} is out of range.");

            if (polarizersVectical < 1) throw new ArgumentOutOfRangeException($"{nameof(SplitPolarizers)}: {nameof(polarizersVectical)} is out of range.");

            var result = new BaseImage<T>(image.Width, image.Height, 1);

            int width = image.Width;
            int height = image.Height;
            int channels = image.Channels;

            var images = new List<BaseImage<T>>();

            for(int h=0; h<polarizersHorizontal; h++)
            {
                for (int v = 0; v < polarizersHorizontal; v++)
                {
                    images.Add(new BaseImage<T>(width / polarizersHorizontal, height / polarizersVectical, channels));
                }
            }

            for (int y = 0; y < height; y++)
            {
                int v = y % polarizersVectical;
                int vOffset = v * polarizersHorizontal;

                for (int x = 0; x < width; x++)
                {
                    int h = x % polarizersHorizontal;

                    image.PixelSpan(x, y).CopyTo(images[vOffset + h].PixelSpan(x / polarizersHorizontal, y / polarizersVectical));
                }
            }

            return images;
        }
    }
}
