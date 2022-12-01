﻿#define PARALLEL_LOOPS

using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Com.Okmer.BasicImage;

namespace Com.Okmer.BasicImage.Convolution
{
    public static class FloatImageConvolutionExtensions
    {
        public static FloatImage GaussianBlurFast(this FloatImage image, int kernelSize, float kernelSigma)
        {
            return ImageGaussianBlurFilterFast(image, kernelSize, kernelSigma);
        }

        public static FloatImage GaussianBlurFastSIMD(this FloatImage image, int kernelSize, float kernelSigma)
        {
            return ImageGaussianBlurFilterFastSIMD(image, kernelSize, kernelSigma);
        }

        /// <summary>
        /// "Fast" gaussian blur filter for image data as a 1D byte array ("Fast" -> Fixed low number of kernel columns)
        /// </summary>
        /// <returns>FloatImage, containing the gaussian blur filtered image</returns>
        private static FloatImage ImageGaussianBlurFilterFast(FloatImage image,
        int kernelSize, float kernelSigma)
        {
            if (KernelGaussianBlurNxN.size != kernelSize || KernelGaussianBlurNxN.sigma != kernelSigma)
            {
                KernelGaussianBlurNxN = (KernelGaussianBlur(kernelSize, kernelSigma, 3), kernelSize, kernelSigma, 3);
            }

            return ConvolutionFilter(image,
                KernelGaussianBlurNxN.kernel, 0, (kernelSize - 1) / 2); //Fixed width of 3
        }

        private static FloatImage ImageGaussianBlurFilterFastSIMD(FloatImage image,
        int kernelSize, float kernelSigma)
        {
            if (KernelGaussianBlurNxN.size != kernelSize || KernelGaussianBlurNxN.sigma != kernelSigma)
            {
                KernelGaussianBlurNxN = (KernelGaussianBlur(kernelSize, kernelSigma, 3), kernelSize, kernelSigma, 3);
            }

            return ConvolutionFilterSIMD(image,
                KernelGaussianBlurNxN.kernel, 0, (kernelSize - 1) / 2); //Fixed width of 3
        }

        /// <summary>
        /// Horizontal Sobel filter
        /// </summary>
        /// <returns>FloatImage, containing the horizontal sobel filtered image</returns>
        public static FloatImage SobelHorizontal(this FloatImage image, byte bias = 127)
        {
            return ImageSobelHorizontalFilter(image, bias);
        }

        public static FloatImage SobelHorizontalSIMD(this FloatImage image, byte bias = 127)
        {
            return ImageSobelHorizontalFilterSIMD(image, bias);
        }

        /// <summary>
        /// Horizontal sobel filter for image data as a 1D byte array
        /// </summary>
        /// <returns>FloatImage, containing the horizontal sobel filtered image</returns>
        private static FloatImage ImageSobelHorizontalFilter(FloatImage image, byte bias = 127)
        {
            return ConvolutionFilter(image,KernelSobel3x3Horizontal, bias, 1);
        }

        private static FloatImage ImageSobelHorizontalFilterSIMD(FloatImage image, byte bias = 127)
        {
            return ConvolutionFilterSIMD(image, KernelSobel3x3Horizontal, bias, 1);
        }

        /// <summary>
        /// Vertical Sobel filter
        /// </summary>
        /// <returns>FloatImage, containing the vertical sobel filtered image</returns>
        public static FloatImage SobelVertical(this FloatImage image, byte bias = 127)
        {
            return ImageSobelVerticalFilter(image, bias);
        }

        public static FloatImage SobelVerticalSIMD(this FloatImage image, byte bias = 127)
        {
            return ImageSobelVerticalFilterSIMD(image, bias);
        }

        /// <summary>
        /// Vertical sobel filter for image data as a 1D byte array
        /// </summary>
        /// <returns>FloatImage, containing the vertical sobel filtered image</returns>
        private static FloatImage ImageSobelVerticalFilter(FloatImage image, byte bias = 127)
        {
            return ConvolutionFilter(image, KernelSobel3x3Vertical, bias, 1);
        }

        private static FloatImage ImageSobelVerticalFilterSIMD(FloatImage image, byte bias = 127)
        {
            return ConvolutionFilterSIMD(image, KernelSobel3x3Vertical, bias, 1);
        }

        /// <summary>
        /// Convolution filter (with factor and bias "pixel = factor * pixel + bias") on a 1D byte array.
        /// </summary>
        /// <param name="image">FloatImage</param>
        /// <param name="filterMatrix">Kernel data as a 2D byte array</param>
        /// <param name="factor">Mutiplication factor to correct for non-normalized kernels (is multiplied with the pixel value)</param>
        /// <param name="bias">Pixel value offset (is added to the pixel value)</param>
        /// <param name="addedPadding">Additional padding of the image sides</param>
        /// <returns>FloatImage, containing the processed image</returns>
        private static FloatImage ConvolutionFilter(FloatImage image,
            float[][] filterMatrix, float biasValue, int addedPadding, bool doInitFillWithBias = false)
        {
            //Calculate filter size and offset
            int filterHeight = filterMatrix.Length;
            int filterWidth = filterMatrix[0].Length;

            int filterOffsetX = (filterWidth - 1) / 2;
            int filterOffsetY = (filterHeight - 1) / 2;

            //Create input buffer
            float[] input = image.Data;

            //Create output buffer (with optional initial bias value)
            var result = new FloatImage(image.Width, image.Height, image.Stride, null, image.Padding + addedPadding);

            float[] output = result.Data;
            if (doInitFillWithBias)
            {
                int lenght = image.Height * image.Stride;
                for (int i = 0; i < lenght; i++)
                {
                    output[i] = biasValue;
                }
            }

            //Calculate start and stop values using the filter offset 
            //(Skipping the image edges simplifies the kernel processing)
            int yStart = (filterOffsetY + image.Padding);
            int yStop = image.Height - (filterOffsetY + image.Padding);

            int xStart = (filterOffsetX + image.Padding);
            int xStop = image.Width - (filterOffsetX + image.Padding);

            int width = image.Width;
            int imageStride = image.Stride;

#if (PARALLEL_LOOPS)
            //Process image lines in parallel 
            Parallel.For(yStart, yStop, (imageY) =>
#else
            // Process image lines in sequencial
            for (int imageY = yStart; imageY < yStop; imageY++)
#endif
            {
                var fullRowsInPixels = imageY * width;
                for (int imageX = xStart; imageX < xStop; imageX++)
                {
                    float pixel = 0.0f;

                    //Calcutate output pixel using the kernel
                    for (int filterY = 0; filterY < filterMatrix.Length; filterY++)
                    {
                        var offset = (imageY + filterY - filterOffsetY) * imageStride + imageX;
                        var filterRow = filterMatrix[filterY];
                        for (int filterX = 0; filterX < filterRow.Length; filterX++)
                        {
                            pixel += input[offset + filterX - filterOffsetX] * filterRow[filterX];
                        }
                    }

                    //Add output pixel value to output buffer (within a valid range).
                    output[fullRowsInPixels + imageX] = (float)Math.Max(0.0, Math.Min(255.0, pixel + biasValue));
                }
            }
#if (PARALLEL_LOOPS)
            );
#endif

            return result;
        }

        private static readonly Vector<float> zeroVector = new Vector<float>(Enumerable.Repeat(0.0f, Vector<float>.Count).ToArray());
        private static readonly Vector<float> minVector = zeroVector;
        private static readonly Vector<float> maxVector = new Vector<float>(Enumerable.Repeat(255.0f, Vector<float>.Count).ToArray());
        private static (Vector<float> vector, float value) biasValueVector = (new Vector<float>(Enumerable.Repeat(0.0f, Vector<float>.Count).ToArray()), 0.0f);

        private static FloatImage ConvolutionFilterSIMD(FloatImage image,
            float[][] filterMatrix, float biasValue, int addedPadding, bool doInitFillWithBias = false)
        {
            //Calculate filter size and offset
            int filterHeight = filterMatrix.Length;
            int filterWidth = filterMatrix[0].Length;

            int filterOffsetX = (filterWidth - 1) / 2;
            int filterOffsetY = (filterHeight - 1) / 2;

            int vectorCount = Vector<float>.Count;

            if (biasValueVector.value != biasValue)
            {
                biasValueVector = (new Vector<float>(Enumerable.Repeat(biasValue, vectorCount).ToArray()), biasValue);
            }

            //Create input buffer
            float[] input = image.Data;

            //Create output buffer (with optional initial bias value)
            var result = new FloatImage(image.Width, image.Height, image.Stride, null, image.Padding + addedPadding);

            float[] output = result.Data;
            if (doInitFillWithBias)
            {
                int lenght = image.Height * image.Stride;
                for (int i = 0; i < lenght; i++)
                {
                    output[i] = biasValue;
                }
            }

            //Calculate start and stop values using the filter offset 
            //(Skipping the image edges simplifies the kernel processing)
            int yStart = (filterOffsetY + image.Padding);
            int yStop = image.Height - (filterOffsetY + image.Padding);

            int xStart = (filterOffsetX + image.Padding);
            int xStop = image.Width - (filterOffsetX + image.Padding);

            //Respect the vectorCount step size
            xStop -= (xStop - xStart) % vectorCount;

            int width = image.Width;
            int imageStride = image.Stride;

#if (PARALLEL_LOOPS)
            //Process image lines in parallel 
            Parallel.For(yStart, yStop, (imageY) =>
#else
            // Process image lines in sequencial
            for (int imageY = yStart; imageY < yStop; imageY++)
#endif
            {
                var fullRowsInPixels = imageY * width;
                for (int imageX = xStart; imageX < xStop; imageX += vectorCount)
                {
                    Vector<float> pixelsVector = zeroVector;

                    //Calcutate output pixel using the kernel
                    for (int filterY = 0; filterY < filterMatrix.Length; filterY++)
                    {
                        var offset = (imageY + filterY - filterOffsetY) * imageStride + imageX;
                        var filterRow = filterMatrix[filterY];

                        for (int filterX = 0; filterX < filterRow.Length; filterX++)
                        {
                            var inputVector = new Vector<float>(input, offset + filterX - filterOffsetX);
                            var filterVector = Vector.Multiply(inputVector, filterRow[filterX]);
                            pixelsVector = Vector.Add(pixelsVector, filterVector);
                        }
                    }

                    //Apply (optional) bias to output pixels
                    if (biasValue != 0.0f)
                    {
                        pixelsVector = Vector.Add(pixelsVector, biasValueVector.vector);
                    }

                    pixelsVector = Vector.Min(maxVector, Vector.Max(minVector, pixelsVector));

                    pixelsVector.CopyTo(output, imageY * imageStride + imageX);
                }
            }
#if (PARALLEL_LOOPS)
            );
#endif
            return result;
        }

        /// <summary>
        /// Calculate a gaussion blur kernel size x size (or size x size2)
        /// </summary>
        /// <param name="size">Number of rows (and columns if size2 == 0)</param>
        /// <param name="sigma">Sigma value of the kernel (higher value -> more blur)</param>
        /// <param name="size2">Number of columns (optional)</param>
        /// <returns>Gaussion blur kernel size x size (or size x size2)</returns>
        /// <remarks>Code source: http://patrick-fuller.com/gaussian-blur-image-processing-for-scientists-and-engineers-part-4/ </remarks>

        private static float[][] KernelGaussianBlur(int size, float sigma, int size2 = 0)
        {
            // Generate a size*size (or size*size2) kernel by sampling the Gaussian function
            int uSize = size;
            int vSize = size2 == 0 ? size : size2;
            float[][] kernel = new float[uSize][];

            int uc, vc;
            float g, sum;
            sum = 0;

            for (int u = 0; u < uSize; u++)
            {
                kernel[u] = new float[vSize];
                for (int v = 0; v < vSize; v++)
                {
                    // Center the Gaussian sample so max is at u,v
                    uc = u - (uSize - 1) / 2;
                    vc = v - (vSize - 1) / 2;
                    // Calculate and save
                    g = (float)Math.Exp(-(float)(uc * uc + vc * vc) / (2.0 * sigma * sigma));
                    sum += g;
                    kernel[u][v] = g;
                }
            }

            // Normalize it
            for (int u = 0; u < uSize; u++)
            {
                for (int v = 0; v < vSize; v++)
                {
                    kernel[u][v] /= sum;
                }
            }

            return kernel;
        }

        /// <summary>
        /// Gets Sobel horizontal kernel 3 x 3
        /// </summary>
        private static float[][] KernelSobel3x3Horizontal = new float[][]
        {
            new float[] { 1,  0,  -1, },
            new float[] { 2,  0,  -2, },
            new float[] { 1,  0,  -1, }
        };

        /// <summary>
        /// Gets Sobel vertical kernel 3 x 3
        /// </summary>
        private static float[][] KernelSobel3x3Vertical = new float[][]
        {
            new float[] { -1, -2, -1, },
            new float[] { 0, 0, 0, },
            new float[] { 1, 2, 1, }
        };

        private static (float[][] kernel, int size, float sigma, int size2) KernelGaussianBlurNxN = (null, 0, 0, 0);
    }
}