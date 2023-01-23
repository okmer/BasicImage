using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using static System.Net.Mime.MediaTypeNames;

namespace Com.Okmer.BasicImage;

public static class BaseImageExtensions
{
    public static BaseImage<T> Copy<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(Copy)}: {nameof(image)} is NOT valid.");

        var result = new BaseImage<T>(image.Width, image.Height, image.Channels);

        Array.Copy(image.Data, result.Data, image.Stride * image.Height);

        return result;
    }

    public static void Clear<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(Clear)}: {nameof(image)} is NOT valid.");

        Array.Clear(image.Data);
    }

    public static BaseImage<T> SingleChannel<T>(this BaseImage<T> image, int channelIndex)
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

    public static List<BaseImage<T>> SplitChannels<T>(this BaseImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(SplitChannels)}: {nameof(image)} is NOT valid.");

        var images = new List<BaseImage<T>>();

        int channels = image.Channels;

        for(int i = 0; i < channels; i++)
        {
            images.Add(image.SingleChannel(i));
        }

        return images;
    }

    public static BaseImage<T> MergeChannels<T>(this IEnumerable<BaseImage<T>> singleChannelImages)
    {
        if (singleChannelImages is null) throw new ArgumentNullException($"{nameof(MergeChannels)}: {nameof(singleChannelImages)} is NULL.");

        int channels = singleChannelImages.Count();

        if (channels == 0) throw new ArgumentOutOfRangeException($"{nameof(MergeChannels)}: {nameof(channels)} is out of range.");

        var firstChannel = singleChannelImages.First();

        foreach (var singleChannelImage in singleChannelImages)
        {
            if (singleChannelImage is null) throw new ArgumentNullException($"{nameof(MergeChannels)}: {nameof(singleChannelImage)} is NULL.");
            if (!singleChannelImage.IsValid) throw new ArgumentException($"{nameof(MergeChannels)}: {nameof(singleChannelImage)} is NOT valid.");

            if(singleChannelImage.Height != firstChannel.Height) throw new ArgumentException($"{nameof(MergeChannels)}: {nameof(singleChannelImage)} height NOT equal.");
            if(singleChannelImage.Stride != firstChannel.Stride) throw new ArgumentException($"{nameof(MergeChannels)}: {nameof(singleChannelImage)} stride NOT equal.");
        }

        int width = firstChannel.Width;
        int height = firstChannel.Height;
        int channelStride = firstChannel.Stride;

        var image = new BaseImage<T>(width, height, channels);
        int imageStride = image.Stride;
        var imageSpan = new Span<T>(image.Data);

        for (int c = 0; c < channels; c++)
        { 
            var channelSpan = new Span<T>(singleChannelImages.ElementAt(c).Data);

            for (int y = 0; y < height; y++)
            {
                int channelOffsetY = channelStride * y;
                int imageOffsetY = imageStride * y;

                for (int x = 0; x < width; x++)
                {
                    imageSpan[imageOffsetY + x * channels + c] = channelSpan[channelOffsetY + x];
                }
            }
        }

        return image;
    }

    public static Span<T> PixelSpan<T>(this BaseImage<T> image, int x, int y) => image.Data.AsSpan(y * image.Stride + x * image.Channels, image.Channels);

    public static T[] PixelArray<T>(this BaseImage<T> image, int x, int y) => image.PixelSpan(x, y).ToArray();

    public static Span<T> LineSpan<T>(this BaseImage<T> image, int y) => image.Data.AsSpan(y * image.Stride, image.Stride);

    public static T[] LineArray<T>(this BaseImage<T> image, int x, int y) => image.LineSpan(y).ToArray();
}
