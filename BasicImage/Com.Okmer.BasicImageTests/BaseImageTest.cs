namespace Com.Okmer.BasicImageTests
{
    [TestClass]
    public class BaseImageTest
    {
        [TestMethod]
        public void CreateBaseImage()
        {
            int width = 640;
            int height = 480;
            int channels = 3;

            using var image = new BaseImage<byte>(width, height, channels);

            Assert.IsTrue(image.IsValid);

            Assert.AreEqual(width, image.Width);
            Assert.AreEqual(height, image.Height);
            Assert.AreEqual(channels, image.Channels);
            Assert.AreEqual(width * channels, image.Stride);

            Assert.IsNotNull(image.Data);
            Assert.IsTrue(image.Data.Length >= width * height * channels);
        }
    }
}