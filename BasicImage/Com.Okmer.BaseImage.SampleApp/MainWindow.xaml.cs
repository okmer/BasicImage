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
    }
}
