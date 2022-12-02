﻿using System;
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

        public static BaseImage<T> SingleChannel<T>(this BaseImage<T> image, int channelIndex)
        {
            if (!image.IsValid) throw new ArgumentException("ChannelImage: Image is NOT valid.");

            if (channelIndex > image.Channels) throw new ArgumentOutOfRangeException($"ChannelImage: {nameof(channelIndex)} is out of range.");

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

        public static Span<T> PixelSpan<T>(this BaseImage<T> image, int x, int y) => image.Data.AsSpan(y * image.Stride + x * image.Channels, image.Channels);

        public static T[] PixelArray<T>(this BaseImage<T> image, int x, int y) => image.PixelSpan(x, y).ToArray();

        public static Span<T> LineSpan<T>(this BaseImage<T> image, int y) => image.Data.AsSpan(y * image.Stride, image.Stride);

        public static T[] LineArray<T>(this BaseImage<T> image, int x, int y) => image.LineSpan(y).ToArray();
    }
}
