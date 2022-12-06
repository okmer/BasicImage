namespace Com.Okmer.BasicImageTests;

[TestClass]
public class BaseImageProcessingExtensionsTest
{
    [TestMethod]
    public void FlipY_Channels1()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9 };

        var output = new byte[] {
            7, 8, 9,
            4, 5, 6,
            1, 2, 3 };

        using var image = new BaseImage<byte>(3, 3, 1, input);

        using var flipped = image.FlipY();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(flipped.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void FlipX_Channels1()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9 };

        var output = new byte[] {
            3, 2, 1,
            6, 5, 4,
            9, 8, 7 };

        using var image = new BaseImage<byte>(3, 3, 1, input);

        using var flipped = image.FlipX();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(flipped.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void FlipX_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        var output = new byte[] {
            7, 8, 9,  4, 5, 6,  1, 2, 3,
            17, 18, 19,  14, 15, 16,  11, 12, 13,
            27, 28, 29,  24, 25, 26,  21, 22, 23, };

        using var image = new BaseImage<byte>(3, 3, 3, input);

        using var flipped = image.FlipX();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(flipped.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void SwapXY_Channels1()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            1, 4, 7,
            2, 5, 8,
            3, 6, 9, };

        using var image = new BaseImage<byte>(3, 3, 1, input);

        using var swapped = image.SwapXY();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(swapped.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate90_3x3_Channels1()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            7, 4, 1,
            8, 5, 2,
            9, 6, 3, };

        using var image = new BaseImage<byte>(3, 3, 1, input);

        using var rotated = image.Rotate90();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate90_1x3_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            7, 8, 9,
            4, 5, 6,
            1, 2, 3, };

        using var image = new BaseImage<byte>(1, 3, 3, input);

        using var rotated = image.Rotate90();

        Assert.AreEqual(image.Width, rotated.Height);
        Assert.AreEqual(image.Height, rotated.Width);
        Assert.AreEqual(image.Channels, rotated.Channels);

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate90_3x1_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        using var image = new BaseImage<byte>(3, 1, 3, input);

        using var rotated = image.Rotate90();

        Assert.AreEqual(image.Width, rotated.Height);
        Assert.AreEqual(image.Height, rotated.Width);
        Assert.AreEqual(image.Channels, rotated.Channels);

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    public void Rotate90_3x3_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        var output = new byte[] {
            7, 8, 9,  17, 18, 19,  27, 28, 29,
            4, 5, 6,  14, 15, 16,  24, 25, 26,
            1, 2, 3,  11, 12, 13,  21, 22, 23, };

        using var image = new BaseImage<byte>(3, 1, 3, input);

        using var rotated = image.Rotate90();

        Assert.AreEqual(image.Width, rotated.Height);
        Assert.AreEqual(image.Height, rotated.Width);
        Assert.AreEqual(image.Channels, rotated.Channels);

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate270_3x3_Channels1()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            3, 6, 9,
            2, 5, 8,
            1, 4, 7, };

        using var image = new BaseImage<byte>(3, 3, 1, input);

        using var rotated = image.Rotate270();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate270_1x3_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        using var image = new BaseImage<byte>(1, 3, 3, input);

        using var rotated = image.Rotate270();

        Assert.AreEqual(image.Width, rotated.Height);
        Assert.AreEqual(image.Height, rotated.Width);
        Assert.AreEqual(image.Channels, rotated.Channels);

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate270_3x1_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            7, 8, 9,
            4, 5, 6,
            1, 2, 3, };

        using var image = new BaseImage<byte>(3, 1, 3, input);

        using var rotated = image.Rotate270();

        Assert.AreEqual(image.Width, rotated.Height);
        Assert.AreEqual(image.Height, rotated.Width);
        Assert.AreEqual(image.Channels, rotated.Channels);

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    public void Rotate270_3x3_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        var output = new byte[] {
            27, 28, 29,  24, 25, 26,  21, 22, 23,
            17, 18, 19,  14, 15, 16,  11, 12, 13,
            7, 8, 9,  4, 5, 6,  1, 2, 3, };

        using var image = new BaseImage<byte>(3, 3, 3, input);

        using var rotated = image.Rotate270();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate180_Channels1()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            9, 8, 7,
            6, 5, 4,
            3, 2, 1, };

        using var image = new BaseImage<byte>(3, 3, 1, input);

        using var rotated = image.Rotate180();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate180_1x3_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            7, 8, 9,
            4, 5, 6,
            1, 2, 3, };

        using var image = new BaseImage<byte>(1, 3, 3, input);

        using var rotated = image.Rotate180();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate180_3x1_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,
            4, 5, 6,
            7, 8, 9, };

        var output = new byte[] {
            7, 8, 9,
            4, 5, 6,
            1, 2, 3, };

        using var image = new BaseImage<byte>(3, 1, 3, input);

        using var rotated = image.Rotate180();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void Rotate180_3x3_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        var output = new byte[] {
            27, 28, 29,  24, 25, 26,  21, 22, 23,
            17, 18, 19,  14, 15, 16,  11, 12, 13,
            7, 8, 9,  4, 5, 6,  1, 2, 3, };

        using var image = new BaseImage<byte>(3, 3, 3, input);

        using var rotated = image.Rotate180();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(rotated.Data?.Take(output.Length).ToArray(), output);
    }
}