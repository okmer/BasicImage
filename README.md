# BasicImage
A simple basic image class for .NET6.0. I will be backporting various image manipulations and converters (to other image formats) when I have time. I’m really missing a basic (frontend independent) image class in the .NET ecosystem. This is my personal fix within multiple projects.

Progress report:
- BaseImage<T>
- ByteImage (BaseImage<byte>)
- FloatImage (BaseImage<float>)
- BaseImage Extensions
	- Copy
	- GetPixel (T[])
- WPF
	- ByteImageWriteableBitmapExtensions
		- ToWriteableBitmap (ByteImage to WriteableBitmap)
		- ToBitmapSource (ByteImage to BitmapSource)
		- ToByteImage (BitmapSource to ByteImage)
	 - ByteImagePngExtensions
		- ToPng (BitmapSource to PNG data byte[])
		- ToPng (ByteImage to PNG data byte[])
		- ToPngFile (BitmapSource to PNG file)
		- ToPngFile (ByteImage to PNG file)
		- ByteImageFromPng (PNG data byte[] to ByteImage)
	 - ByteImageJpegExtensions
		- ToJpeg (BitmapSource to JPEG data byte[])
		- ToJpeg (ByteImage to JPEG data byte[])
		- ToJpegFile (BitmapSource to JPEG file)
		- ToJpegFile (ByteImage to JPEG file)
		- ByteImageFromJpeg (JPEG data byte[] to ByteImage)
