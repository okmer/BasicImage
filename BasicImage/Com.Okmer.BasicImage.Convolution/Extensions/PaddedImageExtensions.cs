using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Convolution;

public static class PaddedImageExtensions
{
    public static void ClearPadding<T>(this PaddedImage<T> image)
    {
        if (!image.IsValid) throw new ArgumentException($"{nameof(ClearPadding)}: {nameof(image)} is NOT valid.");

        if (image.Padding == 0) return;

        int topBottomRange = image.Padding * image.Stride;

        int topStart = 0;
        int bottomStart = image.Height * image.Stride - topBottomRange;

        //Clear top line(s)
        var topSpan = new Span<T>(image.Data, topStart, topBottomRange);
        topSpan.Clear();

        //Clear bottom line(s)
        var bottomSpan = new Span<T>(image.Data, bottomStart, topBottomRange);
        bottomSpan.Clear();

        int stride = image.Stride;
        int leftRightRange = image.Padding * image.Channels;

        //Clear left and right column(s)
        for (int y = image.Padding; y < image.Height; y++)
        {
            int lineIndex = y * stride;
                
            var leftSpan = new Span<T>(image.Data, lineIndex, leftRightRange);
            leftSpan.Clear();

            var rightSpan = new Span<T>(image.Data, lineIndex - leftRightRange, leftRightRange);
            rightSpan.Clear();
        }
    }
}

