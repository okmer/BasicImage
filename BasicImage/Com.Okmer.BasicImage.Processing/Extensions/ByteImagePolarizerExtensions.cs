using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Processing
{
    public static class ByteImagePolarizerExtensions
    {
        public static List<BaseImage<T>> SplitPolarizers<T>(this BaseImage<T> image, int polarizersHorizontal, int polarizersVertical)
        {
            ArgumentNullException.ThrowIfNull(image);
            if (!image.IsValid) 
                throw new ArgumentException($"Image is invalid", nameof(image));
            
            ArgumentOutOfRangeException.ThrowIfLessThan(polarizersHorizontal, 1, nameof(polarizersHorizontal));
            ArgumentOutOfRangeException.ThrowIfLessThan(polarizersVertical, 1, nameof(polarizersVertical));

            int width = image.Width;
            int height = image.Height;
            int channels = image.Channels;
            
            // Check if image dimensions are divisible by polarizer count
            if (width % polarizersHorizontal != 0)
                throw new ArgumentException($"Image width {width} not divisible by horizontal polarizers {polarizersHorizontal}");
            if (height % polarizersVertical != 0)
                throw new ArgumentException($"Image height {height} not divisible by vertical polarizers {polarizersVertical}");

            int subWidth = width / polarizersHorizontal;
            int subHeight = height / polarizersVertical;
            
            var images = new List<BaseImage<T>>(polarizersHorizontal * polarizersVertical);

            // Create sub-images
            for (int h = 0; h < polarizersHorizontal; h++)
            {
                for (int v = 0; v < polarizersVertical; v++)
                {
                    images.Add(new BaseImage<T>(subWidth, subHeight, channels));
                }
            }

            // Distribute pixels based on polarizer pattern
                for (int y = 0; y < height; y++)
            {
                int vIndex = (y / subHeight) % polarizersVertical;
                int subY = y % subHeight;
                
                for (int x = 0; x < width; x++)
                {
                    int hIndex = (x / subWidth) % polarizersHorizontal;
                    int subX = x % subWidth;
                    int imageIndex = vIndex * polarizersHorizontal + hIndex;
                    
                    image.PixelSpan(x, y).CopyTo(images[imageIndex].PixelSpan(subX, subY));
                }
            }

            return images;
        }
    }
}
