using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;

namespace Vapp.Platform.Windows.Wpf.Converters
{
    class FileSizeToStringConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            if (!(value is int) && !(value is long))
                return value;

            int level = 0;
            double bytes = (long) value;

            while (bytes >= 1024.0)
            {
                bytes /= 1024.0;
                level++;
            }

            bytes = Math.Round(bytes, 2);

            switch (level)
            {
                default:
                    return string.Format("{0} Bytes", (long) value);
                case 1:
                    return string.Format("{0} KB", bytes);
                case 2:
                    return string.Format("{0} MB", bytes);
                case 3:
                    return string.Format("{0} GB", bytes);
                case 4:
                    return string.Format("{0} TB", bytes);
                case 5:
                    return string.Format("{0} PB", bytes);
                case 6:
                    return string.Format("{0} EB", bytes);
                case 7:
                    return string.Format("{0} ZB", bytes);
                case 8:
                    return string.Format("{0} YB", bytes);
            }
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
