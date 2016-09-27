using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows.Media;
using Vapp.Platform.Windows.Wpf.ViewModels;

namespace Vapp.Platform.Windows.Wpf.Converters
{
    class CommandValidationColorConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            IEnumerable<string> input = CommandConsoleViewModel.GetArguments(((string) value == null ? "" : (string) value).ToLower().Trim());
            string key = input.Count() > 0 ? input.ElementAt(0) : "";
            return key.Length == 0 ? new SolidColorBrush(new Color() { R = 192, G = 192, B = 192, A = 255 }) : App.CommandRegisterService.Contains(key) ? Brushes.Green : Brushes.Red;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
