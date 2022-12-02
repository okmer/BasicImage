using System.Buffers;

namespace Com.Okmer.BasicImage
{
    public class BaseImage<T> : IDisposable
    {
        private readonly ArrayPool<T>? sharedPool = null;

        public int Width { get; private set; }
        public int Height { get; private set; }
        public int Channels { get; private set; }
        public T[] Data { get; private set; }

        public bool IsValid => Data is not null && Data.Length > 0 && Width > 0 && Height > 0 && Channels > 0;

        public int Stride => Width * Channels;

        public BaseImage(int width, int height, int channels, T[]? data = null)
        {
            Width = width;
            Height = height;
            Channels = channels;

            if (data is null)
            {
                sharedPool = ArrayPool<T>.Shared;
                Data = sharedPool.Rent(Height * Stride);
            }
            else
            {
                Data = data;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && Data is not null && Data.Length > 0)
                {
                    sharedPool?.Return(Data);
                }

                Width = 0;
                Height = 0;
                Channels = 0;
                Data = Array.Empty<T>();

                disposedValue = true;
            }
        }

        public void Dispose()
        {
            Dispose(true);
        }
        #endregion
    }
}