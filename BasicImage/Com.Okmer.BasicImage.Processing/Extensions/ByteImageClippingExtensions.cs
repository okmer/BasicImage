using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Com.Okmer.BasicImage;

namespace Com.Okmer.BasicImage.Processing;

public static class ByteImageClippingExtensions
{
    /// <summary>
    /// Clip pixel values to maxValue
    /// </summary>
    /// <returns>ByteImage, clipped to maxValue</returns>
    public static BaseImage<byte> Clip(this BaseImage<byte> image, byte maxValue)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(Clip)}: {nameof(image)} is NOT valid.");

        var result = new BaseImage<byte>(image.Width, image.Height, image.Channels, null);

        byte[] input = image.Data;
        byte[] output = result.Data;
        int length = image.Height * image.Stride;

        for (int i = 0; i < length; i++)
        {
            byte value = input[i];
            output[i] = value > maxValue ? maxValue : value;
        }

        return result;
    }

    /// <summary>
    /// In place clip pixel values to maxValue
    /// </summary>
    public static void ClipInPlace(this BaseImage<byte> image, byte maxValue)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(ClipInPlace)}: {nameof(image)} is NOT valid.");

        byte[] input = image.Data;
        int length = image.Height * image.Stride;
        for (int i = 0; i < length; i++)
        {
            if (input[i] > maxValue)
            {
                input[i] = maxValue;
            }
        }
    }

    private static (Vector<byte> vector, byte value) maxValueVector = (new Vector<byte>(), 255);

    public static void ClipInPlaceSIMD(this BaseImage<byte> image, byte maxValue)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(ClipInPlaceSIMD)}: {nameof(image)} is NOT valid.");

        if (maxValue == 255) return;

        int vectorCount = Vector<byte>.Count;

        if (maxValueVector.value != maxValue)
        {
            var maxVales = Enumerable.Repeat(maxValue, vectorCount).ToArray();
            maxValueVector = (new Vector<byte>(maxVales), maxValue);
        }

        var maxVector = maxValueVector.vector;

        byte[] input = image.Data;
        int length = (image.Height * image.Stride) - vectorCount;

        for (int i = 0; i < length; i += vectorCount)
        {
            var inputVector = new Vector<byte>(input, i);

            Vector.Min(inputVector, maxVector).CopyTo(input, i);
        }
    }
}
