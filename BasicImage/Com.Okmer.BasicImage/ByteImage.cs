namespace Com.Okmer.BasicImage
{
    public class ByteImage : BaseImage<byte>
    {
        public ByteImage(int width, int height, int channels, byte[]? data = null) : base(width, height, channels, data) {}
    }
}