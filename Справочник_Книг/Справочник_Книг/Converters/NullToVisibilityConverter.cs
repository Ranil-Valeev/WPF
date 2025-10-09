using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Data;
using System.Windows;

namespace Справочник_Книг.Converters
{
    class NullToVisibilityConverter : IValueConverter
    {
        // parameter == "HasValue" -> Visible когда value != null
        // иначе -> Visible когда value == null
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            bool hasValue = value != null && !(value is string s && string.IsNullOrWhiteSpace(s));

            if ((parameter as string) == "HasValue")
                return hasValue ? Visibility.Visible : Visibility.Collapsed;

            return hasValue ? Visibility.Collapsed : Visibility.Visible;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }

    }
}
