using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

using Com.Okmer.BaseImage;
using Com.Okmer.BasicImage;
using Com.Okmer.BasicImage.Convolution;
using Com.Okmer.BasicImage.Processing;
using Com.Okmer.BasicImage.WPF;

namespace Com.Okmer.BaseImage.SampleApp
{
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private BitmapImage inputImage = new BitmapImage(new Uri("testimage.jpg", UriKind.Relative));

        public MainWindow()
        {
            InitializeComponent();

            InputImage.Source = inputImage;
        }

        private void Copy_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.Copy();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void FlipX_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.FlipX();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void FlipY_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.FlipY();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void SwapXY_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.SwapXY();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void Rotate90_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.Rotate90();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void Rotate180_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.Rotate180();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void Rotate270_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.Rotate270();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void Clip127_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.Clip(127);
            OutputImage.Source = output.ToBitmapSource();
        }

        private void ClipInPlaceSIMD127_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            image.ClipInPlaceSIMD(127);
            OutputImage.Source = image.ToBitmapSource();
        }

        private void ResizeHalf_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.ResizeHalf();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void ResizeOneFifth_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.Resize(5);
            OutputImage.Source = output.ToBitmapSource();
        }

        private void Grey_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.AveragedChannel();
            OutputImage.Source = output.ToBitmapSource();
        }

        private void Red_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.SingleChannel(2);
            OutputImage.Source = output.ToBitmapSource();
        }

        private void Green_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.SingleChannel(1);
            OutputImage.Source = output.ToBitmapSource();
        }

        private void Blue_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var output = image.SingleChannel(0);
            OutputImage.Source = output.ToBitmapSource();
        }

        private void FloatAndBack_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var floatImage = image.ToFloatImage();
            using var output = floatImage.ToByteImage();

            OutputImage.Source = output.ToBitmapSource();
        }

        private void GaussianBlur_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var grayImage = image.AveragedChannel();
            using var floatImage = grayImage.ToFloatImage();
            using var floatOutput = floatImage.GaussianBlurFastSIMD(11, 2.50f);
            using var output = floatOutput.ToByteImage();

            OutputImage.Source = output.ToBitmapSource();
        }

        private void SobelHorizontal_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var grayImage = image.AveragedChannel();
            using var floatImage = grayImage.ToFloatImage();
            using var floatOutput = floatImage.SobelHorizontalSIMD();
            using var output = floatOutput.ToByteImage();

            OutputImage.Source = output.ToBitmapSource();
        }

        private void SobelVertical_Click(object sender, RoutedEventArgs e)
        {
            using var image = inputImage.ToByteImage();
            using var grayImage = image.AveragedChannel();
            using var floatImage = grayImage.ToFloatImage();
            using var floatOutput = floatImage.SobelVerticalSIMD();
            using var output = floatOutput.ToByteImage();

            OutputImage.Source = output.ToBitmapSource();
        }
    }
}
