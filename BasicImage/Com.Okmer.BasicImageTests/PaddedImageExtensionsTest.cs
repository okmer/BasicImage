using Com.Okmer.BasicImage.Convolution;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImageTests;

[TestClass]
public class PaddedImageExtensionsTest
{
    [TestMethod]
    public void ClearPadding_Channels1a()
    {
        var input = new byte[] {
        1, 2, 3,
        4, 5, 6,
        7, 8, 9 };

        var output = new byte[] {
        0, 0, 0,
        0, 5, 0,
        0, 0, 0 };

        using var image = new BaseImage<byte>(3, 3, 1, input);

        using var padded = new PaddedImage<byte>(image, 1);

        padded.ClearPadding();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(padded.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void ClearPadding_Channels1b()
    {
        var input = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        var output = new byte[] {
            0, 0, 0,  0, 0, 0,   0, 0, 0,
            0, 12, 13,  14, 15, 16,  17, 18, 0,
            0, 0, 0,  0, 0, 0,   0, 0, 0, };

        using var image = new BaseImage<byte>(9, 3, 1, input);

        using var padded = new PaddedImage<byte>(image, 1);

        padded.ClearPadding();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(padded.Data?.Take(output.Length).ToArray(), output);
    }

    [TestMethod]
    public void ClearPadding_Channels3()
    {
        var input = new byte[] {
            1, 2, 3,  4, 5, 6,  7, 8, 9,
            11, 12, 13,  14, 15, 16,  17, 18, 19,
            21, 22, 23,  24, 25, 26,  27, 28, 29, };

        var output = new byte[] {
            0, 0, 0,  0, 0, 0,   0, 0, 0,
            0, 0, 0,  14, 15, 16,  0, 0, 0,
            0, 0, 0,  0, 0, 0,   0, 0, 0, };

        using var image = new BaseImage<byte>(3, 3, 3, input);

        using var padded = new PaddedImage<byte>(image, 1);

        padded.ClearPadding();

        CollectionAssert.AreEqual(image.Data?.Take(input.Length).ToArray(), input);
        CollectionAssert.AreEqual(padded.Data?.Take(output.Length).ToArray(), output);
    }
}
