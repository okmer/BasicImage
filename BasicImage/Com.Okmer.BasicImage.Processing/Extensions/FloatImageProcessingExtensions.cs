﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Processing;

public static class FloatImageProcessingExtensions
{
    public static BaseImage<float> AveragedChannel(this BaseImage<float> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(AveragedChannel)}: {nameof(image)} is NOT valid.");

        if (image.Channels == 1) return image.Copy();

        var result = new BaseImage<float>(image.Width, image.Height, 1);

        int width = image.Width;
        int height = image.Height;
        int channels = image.Channels;
        float[]? data = result.Data;

        for (int y = 0; y < height; y++)
        {
            for (int x = 0; x < width; x++)
            {
                float total = 0;
                foreach (var v in image.PixelSpan(x, y))
                {
                    total += v;
                }

                data[y * width + x] = total / channels;
            }
        }

        return result;
    }

    public static BaseImage<byte> ToByteImage(this BaseImage<float> image, float multiplier = 1.0f)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(ToByteImage)}: {nameof(image)} is NOT valid.");

        var result = new BaseImage<byte>(image.Width, image.Height, image.Channels);

        int length = image.Height * image.Stride;

        float[]? input = image.Data;
        byte[]? output = result.Data;

        for (int i = 0; i < length; i++)
        {
            output[i] = (byte)Math.Round(input[i] * multiplier);
        }

        return result;
    }
}
