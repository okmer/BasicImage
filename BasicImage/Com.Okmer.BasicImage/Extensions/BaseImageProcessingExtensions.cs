﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage;

public static class BaseImageProcessingExtensions
{
    public static BaseImage<T> FlipY<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(FlipY)}: {nameof(image)} is NOT valid.");

        var result = new BaseImage<T>(image.Width, image.Height, image.Channels);

        int height = image.Height;
        int stride = image.Stride;
        int lenght = height * stride;

        var inputSpan = new Span<T>(image.Data, 0, lenght);
        var outputSpan = new Span<T>(result.Data, 0, lenght);

        for (int y = 0; y < height; y++)
        {
            int y_inverted = (height - 1) - y;
            inputSpan.Slice(y * stride, stride).CopyTo(outputSpan.Slice(y_inverted * stride, stride));
        }

        return result;
    }

    public static BaseImage<T> FlipX<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(FlipX)}: {nameof(image)} is NOT valid.");

        int width = image.Width;
        int height = image.Height;
        int channels = image.Channels;

        //Fast single channel
        if (channels == 1)
        {
            var copy = image.Copy();

            for (int y = 0; y < height; y++)
            {
                copy.LineSpan(y).Reverse();
            }

            return copy;
        }

        var result = new BaseImage<T>(image.Width, image.Height, image.Channels);

        //Slow multi channel
        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                int x_inverted = (width - 1) - x;
                image.PixelSpan(x, y).CopyTo(result.PixelSpan(x_inverted, y));
            }
        }

        return result;
    }

    public static BaseImage<T> SwapXY<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(SwapXY)}: {nameof(image)} is NOT valid.");

        var result = new BaseImage<T>(image.Height, image.Width, image.Channels);

        int width = image.Width;
        int height = image.Height;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                image.PixelSpan(x, y).CopyTo(result.PixelSpan(y, x));
            }
        }

        return result;
    }

    public static BaseImage<T> Rotate90<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(Rotate90)}: {nameof(image)} is NOT valid.");

        var result = new BaseImage<T>(image.Height, image.Width, image.Channels);

        int width = image.Width;
        int height = image.Height;

        for (int y = 0; y < height; y++)
        {
            int y_inverted = (height - 1) - y;
            for (int x = 0; x < width; x++)
            {
                image.PixelSpan(x, y).CopyTo(result.PixelSpan(y_inverted, x));
            }
        }

        return result;
    }

    public static BaseImage<T> Rotate180<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(Rotate180)}: {nameof(image)} is NOT valid.");

        var result = new BaseImage<T>(image.Width, image.Height, image.Channels);

        int width = image.Width;
        int height = image.Height;

        for (int y = 0; y < height; y++)
        {
            int y_inverted = (height - 1) - y;
            for (int x = 0; x < width; x++)
            {
                int x_inverted = (width - 1) - x;
                image.PixelSpan(x, y).CopyTo(result.PixelSpan(x_inverted, y_inverted));
            }
        }

        return result;
    }

    public static BaseImage<T> Rotate270<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(Rotate270)}: {nameof(image)} is NOT valid.");

        var result = new BaseImage<T>(image.Height, image.Width, image.Channels);

        int width = image.Width;
        int height = image.Height;

        for (int x = 0; x < width; x++)
        {
            int x_inverted = (width - 1) - x;
            for (int y = 0; y < height; y++)
            {
                image.PixelSpan(x, y).CopyTo(result.PixelSpan(y, x_inverted));
            }
        }

        return result;
    }
}
