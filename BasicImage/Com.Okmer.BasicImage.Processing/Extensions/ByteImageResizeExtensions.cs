using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Processing
{
    public static class ByteImageResizeExtensions
    {
        /// <summary>
        /// WeldFinderImage extention to return a Nearest Neighbor scaled image
        /// </summary>
        /// <returns>WeldFinderImage with a a scaled image.</returns>
        /// <see cref="http://tech-algorithm.com/articles/nearest-neighbor-image-scaling/"/>
        public static BaseImage<byte> ResizeHalf(this BaseImage<byte> image)
        {
            if (image.Channels == 1) return image.ResizeHalfGray();

            var imageWidth = image.Width;
            var imageHeight = image.Height;
            var imageStride = image.Stride;
            var channels = image.Channels;

            var width = imageWidth / 2;
            var height = imageHeight / 2;
            var stride = imageStride / 2;

            var result = new BaseImage<byte>(width, height, channels);

            var input = image.Data;
            var output = result.Data;

            for (int y = 0; y < height; y++)
            {
                var imageLineOffset1 = 2 * y * imageStride;
                var imageLineOffset2 = imageLineOffset1 + imageStride;
                var lineOffset = y * stride;

                for (int x = 0; x < width; x++)
                {
                    var pixel = x * channels;
                    var imagePixelOffset1 = 2 * x * channels;
                    var imagePixelOffset2 = imagePixelOffset1 + channels;

                    for (int i = 0; i < channels; i++)
                    {
                        output[lineOffset + pixel + i] = (byte)((
                                                        input[imageLineOffset1 + imagePixelOffset1 + i] +
                                                        input[imageLineOffset1 + imagePixelOffset2 + i] +
                                                        input[imageLineOffset2 + imagePixelOffset1 + i] +
                                                        input[imageLineOffset2 + imagePixelOffset2 + i]
                                                        ) * 0.25);
                    }
                }
            }
            return result;
        }

        private static BaseImage<byte> ResizeHalfGray(this BaseImage<byte> image)
        {
            if (image.Channels > 1)
                throw new ArgumentException("ResizedHalfGray supports one byte per pixel images only.");

            var imageWidth = image.Width;
            var imageHeight = image.Height;
            var imageStride = image.Stride;
            var channels = image.Channels;

            var width = imageWidth / 2;
            var height = imageHeight / 2;

            var result = new BaseImage<byte>(width, height, channels);

            var input = image.Data;
            var output = result.Data;

            for (int y = 0; y < height; y++)
            {
                var imageLineOffset1 = 2 * y * imageStride;
                var imageLineOffset2 = imageLineOffset1 + imageStride;
                var lineOffset = y * width;

                for (int x = 0; x < width; x++)
                {
                    var imagePixelOffset1 = 2 * x;
                    var imagePixelOffset2 = imagePixelOffset1 + 1;

                    output[lineOffset + x] = (byte)((
                                input[imageLineOffset1 + imagePixelOffset1] +
                                input[imageLineOffset1 + imagePixelOffset2] +
                                input[imageLineOffset2 + imagePixelOffset1] +
                                input[imageLineOffset2 + imagePixelOffset2]
                                ) * 0.25);
                }
            }
            return result;
        }

        public static BaseImage<byte> Resize(this BaseImage<byte> image, int scale)
        {
            if (image.Channels == 1) return image.ResizeGray(scale);

            var imageWidth = image.Width;
            var imageHeight = image.Height;
            var imageStride = image.Stride;
            var channels = image.Channels;

            var width = imageWidth / scale;
            var height = imageHeight / scale;
            var stride = imageStride / scale;

            var result = new BaseImage<byte>(width, height, channels);

            var input = image.Data;
            var output = result.Data;

            var imageLineOffset = new int[scale];
            var imagePixelOffset = new int[scale];
            int value;

            for (int y = 0; y < height; y++)
            {
                imageLineOffset[0] = scale * y * imageStride;
                for (int l = 1; l < scale; l++)
                {
                    imageLineOffset[l] = imageLineOffset[l - 1] + imageStride;
                }

                var lineOffset = y * stride;

                for (int x = 0; x < width; x++)
                {
                    imagePixelOffset[0] = scale * x * channels;
                    for (int p = 1; p < scale; p++)
                    {
                        imagePixelOffset[p] = imagePixelOffset[p - 1] + channels;
                    }

                    var pixelOffset = x * channels;

                    for (int i = 0; i < channels; i++)
                    {
                        value = 0;
                        for (int r = 0; r < scale; r++)
                        {
                            for (int c = 0; c < scale; c++)
                            {
                                value += input[imageLineOffset[r] + imagePixelOffset[c] + i];
                            }
                        }
                        output[lineOffset + pixelOffset + i] = (byte)(value / (float)(scale * scale));
                    }
                }
            }
            return result;
        }

        private static BaseImage<byte> ResizeGray(this BaseImage<byte> image, int scale)
        {
            if (image.Channels > 1)
                throw new ArgumentException("ResizedHalfGray supports one byte per pixel images only.");

            var imageWidth = image.Width;
            var imageHeight = image.Height;
            var imageStride = image.Stride;
            var channels = image.Channels;

            var width = imageWidth / scale;
            var height = imageHeight / scale;

            var result = new BaseImage<byte>(width, height, channels);

            var input = image.Data;
            var output = result.Data;

            var imageLineOffset = new int[scale];
            var imagePixelOffset = new int[scale];
            int value;

            for (int y = 0; y < height; y++)
            {
                imageLineOffset[0] = scale * y * imageStride;
                for (int l = 1; l < scale; l++)
                {
                    imageLineOffset[l] = imageLineOffset[l - 1] + imageStride;
                }

                var lineOffset = y * width;

                for (int x = 0; x < width; x++)
                {
                    imagePixelOffset[0] = scale * x;
                    for (int p = 1; p < scale; p++)
                    {
                        imagePixelOffset[p] = imagePixelOffset[p - 1] + 1;
                    }

                    value = 0;
                    for (int r = 0; r < scale; r++)
                    {
                        for (int c = 0; c < scale; c++)
                        {
                            value += input[imageLineOffset[r] + imagePixelOffset[c]];
                        }
                    }
                    output[lineOffset + x] = (byte)(value / (float)(scale * scale));
                }
            }
            return result;
        }
    }
}
