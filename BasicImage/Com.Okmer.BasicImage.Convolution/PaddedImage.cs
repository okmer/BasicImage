using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage.Convolution;

public class PaddedImage<T> : BaseImage<T>
{
    public int Padding { get; set; } = 0;

    public PaddedImage(int width, int height, int channels, T[]? data = null, int padding = 0) : base(width, height, channels, data)
    {
        Padding = padding;
    }

    public PaddedImage(BaseImage<T> image, int padding = 0) : base(image.Width, image.Height, image.Channels, image.Data)
    {
        Padding = padding;
    }
}
