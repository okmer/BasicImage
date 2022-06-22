using System.Buffers;

namespace Com.Okmer.BasicImage
{
    public class BaseImage<T> : IDisposable
    {
        private readonly ArrayPool<T>? sharedPool = null;

        public int Width { get; }
        public int Height { get; }
        public int Channels { get; }
        public T[]? Data { get; }

        public bool IsValid
        {
            get { return Data != null && Width > 0 && Height > 0 && Stride > 0; }
        }

        public int Stride => Width * Channels;

        public BaseImage(int width, int height, int channels, T[]? data = null)
        {
            Width = width;
            Height = height;
            Channels = channels;
            Data = data;

            if (Data == null)
            {
                sharedPool = ArrayPool<T>.Shared;
                Data = sharedPool.Rent(Height * Stride);
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && Data is not null)
                {
                    sharedPool?.Return(Data);
                }

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