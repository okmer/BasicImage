# BasicImage
A simple basic image class for .NET6.0. I will be backporting various image manipulations and converters (to other image formats) when I have time. I’m really missing a basic (frontend independent) image class in the .NET ecosystem. This is my personal fix within multiple projects.

![image](https://user-images.githubusercontent.com/3484773/205339662-e9053a0f-e348-409a-a26d-2b21bdadaf91.png)

Progress report:
- BaseImage<T>
- ByteImage (BaseImage&lt;byte&gt;)
- FloatImage (BaseImage&lt;float&gt;)
- BaseImage Extensions
	- Copy (BaseImage<T>)
	- Clear
	- SingleChannel (BaseImage<T>)
	- PixelArray (T[])
	- PixelSpan (Span<T>)
	- LineArray (T[])
	- LineSpan (Span<T>)
- BaseImage Processing Extensions
	- FlipX
	- FlipY
	- SwapXY
	- Rotate90
	- Rotate180
	- Rotate270
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
		- ByteImageFromPng (PNG data byte[] to ByteImage)ClipInPlxadwddd
	 - ByteImageJpegExtensions
		- ToJpeg (BitmapSource to JPEG data byte[])
		- ToJpeg (ByteImage to JPEG data byte[])
		- ToJpegFile (BitmapSource to JPEG file)dwd
		- ToJpegFile (ByteImage to JPEG file)
		- ByteImageFromJpeg (JPEG data byte[] to ByteImage)
- WinForms
	- ByteImageBitmapExtensions
		- ToBitmap (ByteImage to Bitmap)
- Convolution [Experimental]
	- FloatImageConvolutionExtensions
		- GaussianBlurFast
		- GaussianBlurFastSIMD
		- SobelHorizontal
		- SobelHorizontalSIMD
		- SobelVertical
		- SobelVerticalSIMD
	- PaddedImageExtensions
		- ClearPadding
- Processing
	- ByteImageProcessingExtensions
		- ToFloatImage
		- AveragedChannel
	- FloatImageProcessingExtensions
		- ToByteImage
		- AveragedChannel
	- ByteImageClippingExtensions
		- Clip
		- ClipInPlace
		- ClipInPlaceSIMD
	- ByteImageResizeExtensions
		- ResizeHalf
		- Resize
