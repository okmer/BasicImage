using System.Buffers;

namespace Com.Okmer.BasicImage;

public class BaseImage<T> : IDisposable
{
    private ArrayPool<T>? sharedPool = null;

    public int Width { get; private set; } = 0;
    public int Height { get; private set; } = 0;
    public int Channels { get; private set; } = 0;
    public T[] Data { get; private set; } = Array.Empty<T>();

    public int Stride => Width * Channels;

    public virtual bool IsValid => Data.Length >= Width * Height * Channels && Width > 0 && Height > 0 && Channels > 0;

    public BaseImage(int width, int height, int channels, T[]? data = null)
    {
        // Input validation
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(width, nameof(width));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(height, nameof(height));
        ArgumentOutOfRangeException.ThrowIfNegativeOrZero(channels, nameof(channels));
        
        // Check for overflow
        if ((long)width * height * channels > int.MaxValue)
            throw new ArgumentOutOfRangeException("Image dimensions would exceed maximum array size");
        
        Width = width;
        Height = height;
        Channels = channels;
        
        var requiredLength = Width * Height * Channels;
        
        if (data is null)
        {
            sharedPool = ArrayPool<T>.Shared;
            Data = sharedPool.Rent(requiredLength);
        }
        else
        {
            if (data.Length < requiredLength)
                throw new ArgumentOutOfRangeException(nameof(data), 
                    $"Data buffer too small: {data.Length} < {requiredLength}");
            Data = data;
        }
    }

    ~BaseImage()
    {
        Dispose(false);
    }

    #region IDisposable Support
    private readonly object disposeLock = new object();
    private volatile bool disposedValue = false;

    protected virtual void Dispose(bool disposing)
    {
        if (disposedValue) return;
        
        lock (disposeLock)
        {
            if (disposedValue) return;
            
            var poolToReturn = Interlocked.Exchange(ref sharedPool, null);
            if (poolToReturn != null && Data.Length > 0)
            {
                try
                {
                    poolToReturn.Return(Data, clearArray: true);
                }
                catch
                {
                    // Log but don't throw in disposal
                }
            }
            
            Data = Array.Empty<T>();
            Width = Height = Channels = 0;
            disposedValue = true;
        }
        
        if (disposing)
        {
            GC.SuppressFinalize(this);
        }
    }

    public void Dispose()
    {
        Dispose(true);
    }
    #endregion
}