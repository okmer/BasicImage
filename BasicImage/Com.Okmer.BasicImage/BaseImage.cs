using System.Buffers;

namespace Com.Okmer.BasicImage
{
    public class BaseImage<T> : IDisposable
    {
        private readonly ArrayPool<T>? sharedPool = null;

        public int Width { get; private set; } = 0;
        public int Height { get; private set; } = 0;
        public int Channels { get; private set; } = 0;
        public T[] Data { get; private set; } = Array.Empty<T>();

        public int Stride => Width * Channels;

        public virtual bool IsValid => Data.Length >= Width * Height * Channels;

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
                if (data.Length < Width * Height * Channels) throw new ArgumentOutOfRangeException($"BaseImage: Data buffer {nameof(data)} is to small for the image dimensions.");

                Data = data;
            }
        }

        #region IDisposable Support
        private bool disposedValue = false;

        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing && Data.Length > 0)
                {
                    sharedPool?.Return(Data); //Only return Data if sharedPool is not Null.
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