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
    }
}