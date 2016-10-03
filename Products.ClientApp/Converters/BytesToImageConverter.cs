using System;
using System.IO;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.UI.Xaml.Data;
using Windows.UI.Xaml.Media.Imaging;
using Products.Model;

namespace Products.ClientApp.Converters
{
    public class BytesToImageConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            var product = value as Product;

            if (product == null)
                return null;

            var width = product.PhotoWidth;
            var height = product.PhotoHeight;
            WriteableBitmap bmp = new WriteableBitmap(width, height);
            using (Stream pixStream = bmp.PixelBuffer.AsStream())
            {
                pixStream.Write(product.PhotoData, 0, width * height * 4);
                return bmp;
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter,
            string language)
        {
            throw new NotImplementedException();
        }
    }
}
