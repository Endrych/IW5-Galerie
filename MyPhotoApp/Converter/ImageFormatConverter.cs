using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using MyPhotoDb.Enums;
using MyPhotoDb.Migrations;

namespace MyPhotoApp.Converter
{
    public class ImageFormatConverter:IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var format = (EntityImageFormat?) value;
            return format?.ToString();
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var formatString = value as string;
            Enum.TryParse(formatString, out EntityImageFormat format);
            return format;
        }
    }
}
