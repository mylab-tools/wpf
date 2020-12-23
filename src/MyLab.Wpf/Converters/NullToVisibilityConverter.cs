using System;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyLab.Wpf.Converters
{
    public class NullToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var invert = (parameter as string) == "invert";

            var visIfNull = invert ? Visibility.Visible : Visibility.Collapsed;
            var visIfNotNull = invert ? Visibility.Collapsed : Visibility.Visible;
            
            return value == null ? visIfNull : visIfNotNull;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotImplementedException();
        }
    }
}
