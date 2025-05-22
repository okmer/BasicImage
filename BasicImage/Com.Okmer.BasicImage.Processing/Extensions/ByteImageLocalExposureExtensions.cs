using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Processing;

public static class ByteImageLocalExposureExtensions
{
    /// <summary>
    /// Locally normalizes the exposure of an 8-bit, n-channel image in place.
    /// </summary>
    /// <param name="img">Any BaseImage whose BitsPerPixel is a multiple of 8.</param>
    /// <param name="windowSize">
    /// Side length of the square neighborhood (must be odd, e.g. 31).
    /// </param>
    /// <param name="maxScale">
    /// Maximum allowed gain.  Values <1 will darken, >1 will brighten.  
    /// E.g. pass 3.0f to allow up to ±3× local scaling.
    /// </param>
    public static BaseImage<byte> LocalExposureNormalized(this BaseImage<byte> image,
                                                int windowSize = 31,
                                                float maxScale = 3f)
    {
        int channels = image.Channels;

        int w = image.Width, h = image.Height;
        int stride = w * channels;
        var src = image.Data;

        var result = new BaseImage<byte>(image.Width, image.Height, image.Channels);

        var dst = result.Data;

        // Compute per-pixel “intensity” and global mean
        var intensity = new double[w * h];
        double globalSum = 0;

        for (int y = 0; y < h; y++)
        {
            int row = y * stride;
            for (int x = 0; x < w; x++)
            {
                int idx = row + x * channels;
                double I = 0;

                if (channels == 3)
                {
                    // Assume B,G,R ordering
                    I = 0.114 * src[idx + 0]
                        + 0.587 * src[idx + 1]
                        + 0.299 * src[idx + 2];
                }
                else
                {
                    // Fallback: simple average over all channels
                    for (int c = 0; c < channels; c++)
                        I += src[idx + c];
                    I /= channels;
                }

                intensity[y * w + x] = I;
                globalSum += I;
            }
        }

        double globalMean = globalSum / (w * h);

        // Build integral image of intensity (size (h+1)x(w+1))
        var Iint = new double[h + 1, w + 1];
        for (int y = 1; y <= h; y++)
        {
            double rowSum = 0;
            for (int x = 1; x <= w; x++)
            {
                rowSum += intensity[(y - 1) * w + (x - 1)];
                Iint[y, x] = Iint[y - 1, x] + rowSum;
            }
        }

        int half = windowSize >> 1;

        // For each pixel, compute local mean, scale, and apply to all channels
        for (int y = 0; y < h; y++)
        {
            int y0 = Math.Max(0, y - half), y1 = Math.Min(h - 1, y + half);
            int iY0 = y0, iY1 = y1 + 1;
            int row = y * stride;

            for (int x = 0; x < w; x++)
            {
                int x0 = Math.Max(0, x - half), x1 = Math.Min(w - 1, x + half);
                int iX0 = x0, iX1 = x1 + 1;

                // Sum over window via integral image
                double sum =
                        Iint[iY1, iX1]
                    - Iint[iY0, iX1]
                    - Iint[iY1, iX0]
                    + Iint[iY0, iX0];
                int count = (y1 - y0 + 1) * (x1 - x0 + 1);
                double localMean = sum / count;

                // Compute and clamp scale factor
                double scale = globalMean / (localMean + 1e-6);
                scale = Math.Min(maxScale, Math.Max(1.0 / maxScale, scale));

                // Apply to every channel
                int baseIdx = row + x * channels;
                for (int c = 0; c < channels; c++)
                {
                    int v = (int)Math.Round(src[baseIdx + c] * scale);
                    dst[baseIdx + c] = (byte)(v < 0 ? 0 : v > 255 ? 255 : v);
                }
            }
        }

        return result;
    }
}

