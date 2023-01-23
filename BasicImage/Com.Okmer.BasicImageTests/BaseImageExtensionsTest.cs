using Com.Okmer.BasicImage;
using Com.Okmer.BasicImage.Processing;
using System.Threading.Channels;

namespace Com.Okmer.BasicImageTests;

[TestClass]
public class BaseImageExtensionsTest
{
    public void Copy_Channels(int channels)
    {
        int width = 640;
        int height = 480;

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
    public void Copy_Channels1() => Copy_Channels(1);

    [TestMethod]
    public void Copy_Channels3() => Copy_Channels(3);

    [TestMethod]
    public void Clear()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9 };

        var cleared = new byte[] {
            0, 0, 0,
            0, 0, 0,
            0, 0, 0 };

        using var image = new BaseImage<byte>(3, 3, 1, input);

        image.Clear();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), cleared);
    }

    [TestMethod]
    public void PixelSpan_Channels1()
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
    public void PixelSpan_Channels3()
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
    public void LineSpan_Channels1()
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
    public void LineSpan_Channels3()
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

    [TestMethod]
    public void Single_Channel3()
    {
        int width = 3;
        int height = 3;
        int channels = 3;

        var data = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        using var image = new BaseImage<byte>(width, height, channels, data);

        var split1 = image.SingleChannel(0);
        var split2 = image.SingleChannel(1);
        var split3 = image.SingleChannel(2);

        Assert.IsTrue(image.IsValid);
        Assert.IsNotNull(image.Data);

        Assert.IsTrue(split1.IsValid);
        Assert.AreEqual(split1.Width, width);
        Assert.AreEqual(split1.Height, height);
        Assert.AreEqual(split1.Channels, 1);

        Assert.IsTrue(split2.IsValid);
        Assert.AreEqual(split2.Width, width);
        Assert.AreEqual(split2.Height, height);
        Assert.AreEqual(split2.Channels, 1);

        Assert.IsTrue(split3.IsValid);
        Assert.AreEqual(split3.Width, width);
        Assert.AreEqual(split3.Height, height);
        Assert.AreEqual(split3.Channels, 1);

        CollectionAssert.AreEqual(split1.Data.Take(width * height).ToArray(), new byte[] {
            1,  4,  7,
            11,  14,  17,
            21,  24,  27, });
        CollectionAssert.AreEqual(split2.Data.Take(width * height).ToArray(), new byte[] {
            2,  5,  8,
            12,  15,  18,
            22,  25,  28, });
        CollectionAssert.AreEqual(split3.Data.Take(width * height).ToArray(), new byte[] {
            3,  6,  9,
            13,  16,  19,
            23,  26,  29, });
    }

    [TestMethod]
    public void Split_Channel3()
    {
        int width = 3;
        int height = 3;
        int channels = 3;

        var data = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        using var image = new BaseImage<byte>(width, height, channels, data);

        var split = image.SplitChannels();

        var split1 = split[0];
        var split2 = split[1];
        var split3 = split[2];

        Assert.IsTrue(image.IsValid);
        Assert.IsNotNull(image.Data);

        Assert.IsTrue(split1.IsValid);
        Assert.AreEqual(split1.Width, width);
        Assert.AreEqual(split1.Height, height);
        Assert.AreEqual(split1.Channels, 1);

        Assert.IsTrue(split2.IsValid);
        Assert.AreEqual(split2.Width, width);
        Assert.AreEqual(split2.Height, height);
        Assert.AreEqual(split2.Channels, 1);

        Assert.IsTrue(split3.IsValid);
        Assert.AreEqual(split3.Width, width);
        Assert.AreEqual(split3.Height, height);
        Assert.AreEqual(split3.Channels, 1);

        CollectionAssert.AreEqual(split1.Data.Take(width * height).ToArray(), new byte[] {
            1,  4,  7,
            11,  14,  17,
            21,  24,  27, });
        CollectionAssert.AreEqual(split2.Data.Take(width * height).ToArray(), new byte[] {
            2,  5,  8,
            12,  15,  18,
            22,  25,  28, });
        CollectionAssert.AreEqual(split3.Data.Take(width * height).ToArray(), new byte[] {
            3,  6,  9,
            13,  16,  19,
            23,  26,  29, });
    }

    [TestMethod]
    public void Merge_Channel3()
    {
        int width = 3;
        int height = 3;
        int channels = 3;

        var data1 = new byte[] {
            1,  4,  7,
            11,  14,  17,
            21,  24,  27, };

        var data2 = new byte[] {
            2,  5,  8,
            12,  15,  18,
            22,  25,  28, };

        var data3 = new byte[] {
            3,  6,  9,
            13,  16,  19,
            23,  26,  29, };

        using var image1 = new BaseImage<byte>(width, height, 1, data1);
        using var image2 = new BaseImage<byte>(width, height, 1, data2);
        using var image3 = new BaseImage<byte>(width, height, 1, data3);

        var images = new List<BaseImage<byte>>() { image1, image2, image3 };

        var image = images.MergeChannels();

        Assert.IsTrue(image.IsValid);
        Assert.IsNotNull(image.Data);

        Assert.AreEqual(image.Width, width);
        Assert.AreEqual(image.Height, height);
        Assert.AreEqual(image.Channels, channels);

        CollectionAssert.AreEqual(image.Data.Take(width * height * channels).ToArray(), new byte[]  {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, });
    }

    [TestMethod]
    public void Split_Merge_Channel3()
    {
        int width = 3;
        int height = 3;
        int channels = 3;

        var data = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        using var image = new BaseImage<byte>(width, height, channels, data);

        var split = image.SplitChannels();

        var merge = split.MergeChannels();

        Assert.IsTrue(merge.IsValid);
        Assert.IsNotNull(merge.Data);
        Assert.AreEqual(merge.Width, width);
        Assert.AreEqual(merge.Height, height);
        Assert.AreEqual(merge.Channels, channels);

        CollectionAssert.AreEqual(image.Data.Take(width * height * channels).ToArray(), merge.Data.Take(width * height * channels).ToArray());
    }
}