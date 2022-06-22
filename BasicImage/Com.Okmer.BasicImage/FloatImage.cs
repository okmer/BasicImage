using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Com.Okmer.BasicImage
{
    public class FloatImage : BaseImage<float>
    {
        public FloatImage(int width, int height, int channels, float[]? data = null) : base(width, height, channels, data) {}
    }
}
