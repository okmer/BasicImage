using System;
using System.Collections.Generic;
using System.Linq;
using System.Numerics;
using System.Text;
using System.Threading.Tasks;
using Com.Okmer.BasicImage;

namespace Com.Okmer.BasicImage.Processing
{
    public static class ByteImageClippingExtensions
    {
        /// <summary>
        /// Clip pixel values to maxValue
        /// </summary>
        /// <returns>ByteImage, clipped to maxValue</returns>
        public static ByteImage Clip(this ByteImage image, byte maxValue)
        {
            var result = new ByteImage(image.Width, image.Height, image.Stride, null);

            byte[] input = image.Data ?? throw new ArgumentNullException(nameof(image));
            byte[] output = result.Data ?? throw new ArgumentNullException(nameof(result));
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
        public static void ClipInPlace(this ByteImage image, byte maxValue)
        {
            byte[] input = image.Data ?? throw new ArgumentNullException(nameof(image));
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

        public static void ClipInPlaceSIMD(this ByteImage image, byte maxValue)
        {
            if (maxValue == 255) return;

            int vectorCount = Vector<byte>.Count;

            if (maxValueVector.value != maxValue)
            {
                var maxVales = Enumerable.Repeat(maxValue, vectorCount).ToArray();
                maxValueVector = (new Vector<byte>(maxVales), maxValue);
            }

            var maxVector = maxValueVector.vector;

            byte[] input = image.Data ?? throw new ArgumentNullException(nameof(image));
            int length = (image.Height * image.Stride) - vectorCount;

            for (int i = 0; i < length; i += vectorCount)
            {
                var inputVector = new Vector<byte>(input, i);

                Vector.Min(inputVector, maxVector).CopyTo(input, i);
            }
        }
    }
}
