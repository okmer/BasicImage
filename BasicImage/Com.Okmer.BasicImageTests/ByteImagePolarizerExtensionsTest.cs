using Com.Okmer.BasicImage.Processing;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImageTests;

[TestClass]
public class ByteImagePolarizerExtensionsTest
{
    [TestMethod]
    public void Split_Polarizer3x3()
    {
        int width = 3;
        int height = 3;
        int channels = 3;

        int horizontal = 3;
        int vertical = 3;

        var data = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        using var image = new BaseImage<byte>(width, height, channels, data);

        Assert.IsTrue(image.IsValid);
        Assert.IsNotNull(image.Data);

        var polarizers = image.SplitPolarizers(horizontal, vertical);

        Assert.AreEqual(polarizers.Count, horizontal * vertical);

        int dataOffset = 0;
        foreach (var polarizer in polarizers)
        {
            Assert.IsTrue(polarizer.IsValid);
            Assert.AreEqual(polarizer.Width, width / horizontal);
            Assert.AreEqual(polarizer.Height, height / vertical);
            Assert.AreEqual(polarizer.Channels, channels);

            CollectionAssert.AreEqual(polarizer.Data.Take(polarizer.Height * polarizer.Stride).ToArray(), data.Skip(dataOffset).Take(polarizer.Height * polarizer.Stride).ToArray());

            dataOffset += channels;
        }
    }

    [TestMethod]
    public void Split_Polarizer1x3()
    {
        int width = 3;
        int height = 3;
        int channels = 3;

        int horizontal = 1;
        int vertical = 3;

        var data = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        using var image = new BaseImage<byte>(width, height, channels, data);

        Assert.IsTrue(image.IsValid);
        Assert.IsNotNull(image.Data);

        var polarizers = image.SplitPolarizers(horizontal, vertical);

        Assert.AreEqual(polarizers.Count, horizontal * vertical);

        foreach (var polarizer in polarizers)
        {
            Assert.IsTrue(polarizer.IsValid);
            Assert.AreEqual(polarizer.Width, width / horizontal);
            Assert.AreEqual(polarizer.Height, height / vertical);
            Assert.AreEqual(polarizer.Channels, channels);
        }

        for (int i = 0; i < polarizers.Count; i++)
        {
            CollectionAssert.AreEqual(polarizers[i].Data.Take(polarizers[i].Height * polarizers[i].Stride).ToArray(), new byte[] {
                1, 2, 3,  4, 5, 6,  7, 8, 9,
                11, 12, 13,  14, 15, 16,  17, 18, 19,
                21, 22, 23,  24, 25, 26,  27, 28, 29, }.Skip(i * width * channels).Take(width * channels).ToArray());
        }
    }

    [TestMethod]
    public void Split_Polarizer3x1()
    {
        int width = 3;
        int height = 3;
        int channels = 3;

        int horizontal = 3;
        int vertical = 1;

        var data = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        using var image = new BaseImage<byte>(width, height, channels, data);

        Assert.IsTrue(image.IsValid);
        Assert.IsNotNull(image.Data);

        var polarizers = image.SplitPolarizers(horizontal, vertical);

        Assert.AreEqual(polarizers.Count, horizontal * vertical);

        foreach (var polarizer in polarizers)
        {
            Assert.IsTrue(polarizer.IsValid);
            Assert.AreEqual(polarizer.Width, width / horizontal);
            Assert.AreEqual(polarizer.Height, height / vertical);
            Assert.AreEqual(polarizer.Channels, channels);
        }

        CollectionAssert.AreEqual(polarizers[0].Data.Take(polarizers[0].Height * polarizers[0].Stride).ToArray(), new byte[] {
                1, 2, 3,
                11, 12, 13,
                21, 22, 23, });

        CollectionAssert.AreEqual(polarizers[1].Data.Take(polarizers[1].Height * polarizers[1].Stride).ToArray(), new byte[] {
                4, 5, 6, 
                14, 15, 16,
                24, 25, 26, });

        CollectionAssert.AreEqual(polarizers[2].Data.Take(polarizers[2].Height * polarizers[2].Stride).ToArray(), new byte[] {
                7, 8, 9,
                17, 18, 19,
                27, 28, 29, });
    }
}