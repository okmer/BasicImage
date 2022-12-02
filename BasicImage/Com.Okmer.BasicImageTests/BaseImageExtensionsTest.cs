namespace Com.Okmer.BasicImageTests
{
    [TestClass]
    public class BaseImageExtensionsTest
    {
        [TestMethod]
        public void CopyImage()
        {
            int width = 640;
            int height = 480;
            int channels = 3;

            using var image = new BaseImage<byte>(width, height, channels);

            using var copy = image.Copy();

            Assert.IsTrue(copy.IsValid);

            Assert.AreEqual(width, copy.Width);
            Assert.AreEqual(height, copy.Height);
            Assert.AreEqual(channels, copy.Channels);
            Assert.AreEqual(width * channels, copy.Stride);

            Assert.IsNotNull(copy.Data);
            Assert.IsTrue(copy.Data.Length >= width * height * channels);
        }

        [TestMethod]
        public void GetPixelSpan_Channels1()
        {
            int width = 3;
            int height = 3;
            int channels = 1;

            var data = new byte[] {
                1, 2, 3,
                4, 5, 6,
                7, 8, 9, };

            using var image = new BaseImage<byte>(width, height, channels, data);

            var pixel1 = image.PixelSpan(0, 0);
            var pixel5 = image.PixelSpan(1, 1);
            var pixel9 = image.PixelSpan(2, 2);

            Assert.IsTrue(image.IsValid);
            Assert.IsNotNull(image.Data);

            Assert.AreEqual(pixel1.Length, channels);
            Assert.AreEqual(pixel5.Length, channels);
            Assert.AreEqual(pixel9.Length, channels);

            Assert.AreEqual(pixel1[0], data[0]);
            Assert.AreEqual(pixel5[0], data[4]);
            Assert.AreEqual(pixel9[0], data[8]);
        }

        [TestMethod]
        public void GetPixelSpan_Channels3()
        {
            int width = 3;
            int height = 1;
            int channels = 3;

            var data = new byte[] {
                1, 2, 3,
                4, 5, 6,
                7, 8, 9, };

            using var image = new BaseImage<byte>(width, height, channels, data);

            var pixel1 = image.PixelSpan(0, 0);
            var pixel2 = image.PixelSpan(1, 0);
            var pixel3 = image.PixelSpan(2, 0);

            Assert.IsTrue(image.IsValid);
            Assert.IsNotNull(image.Data);

            Assert.AreEqual(pixel1.Length, channels);
            Assert.AreEqual(pixel2.Length, channels);
            Assert.AreEqual(pixel3.Length, channels);

            Assert.AreEqual(pixel1[0], data[0]);
            Assert.AreEqual(pixel1[1], data[1]);
            Assert.AreEqual(pixel1[2], data[2]);

            Assert.AreEqual(pixel2[0], data[3]);
            Assert.AreEqual(pixel2[1], data[4]);
            Assert.AreEqual(pixel2[2], data[5]);

            Assert.AreEqual(pixel3[0], data[6]);
            Assert.AreEqual(pixel3[1], data[7]);
            Assert.AreEqual(pixel3[2], data[8]);
        }

        [TestMethod]
        public void GetLineSpan_Channels1()
        {
            int width = 3;
            int height = 3;
            int channels = 1;

            var data = new byte[] {
                1, 2, 3,
                4, 5, 6,
                7, 8, 9, };

            using var image = new BaseImage<byte>(width, height, channels, data);

            var line1 = image.LineSpan(0);
            var line2 = image.LineSpan(1);
            var line3 = image.LineSpan(2);

            Assert.IsTrue(image.IsValid);
            Assert.IsNotNull(image.Data);

            Assert.AreEqual(line1.Length, width * channels);
            Assert.AreEqual(line2.Length, width * channels);
            Assert.AreEqual(line3.Length, width * channels);

            CollectionAssert.AreEqual(line1.ToArray(), data.Take(width * channels).ToArray());
            CollectionAssert.AreEqual(line2.ToArray(), data.Skip(width * channels).Take(width * channels).ToArray());
            CollectionAssert.AreEqual(line3.ToArray(), data.Skip(2 * width * channels).Take(width * channels).ToArray());
        }

        [TestMethod]
        public void GetLineSpan_Channels3()
        {
            int width = 3;
            int height = 3;
            int channels = 3;

            var data = new byte[] {
                1, 2, 3,  4, 5, 6,  7, 8, 9,
                11, 12, 13,  14, 15, 16,  17, 18, 19,
                21, 22, 23,  24, 25, 26,  27, 28, 29, };

            using var image = new BaseImage<byte>(width, height, channels, data);

            var line1 = image.LineSpan(0);
            var line2 = image.LineSpan(1);
            var line3 = image.LineSpan(2);

            Assert.IsTrue(image.IsValid);
            Assert.IsNotNull(image.Data);

            Assert.AreEqual(line1.Length, width * channels);
            Assert.AreEqual(line2.Length, width * channels);
            Assert.AreEqual(line3.Length, width * channels);

            CollectionAssert.AreEqual(line1.ToArray(), data.Take(width * channels).ToArray());
            CollectionAssert.AreEqual(line2.ToArray(), data.Skip(width * channels).Take(width * channels).ToArray());
            CollectionAssert.AreEqual(line3.ToArray(), data.Skip(2 * width * channels).Take(width * channels).ToArray());
        }
    }
}