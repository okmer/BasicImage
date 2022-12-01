using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage
{
    public class FloatImage : BaseImage<float>
    {
        public int Padding { get; set; } = 0;

        public FloatImage(int width, int height, int channels, float[]? data = null, int padding = 0) : base(width, height, channels, data)
        {
            Padding = padding;
        }
    }
}
