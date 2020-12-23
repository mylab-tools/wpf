using System;
using System.Collections;
using System.Globalization;
using System.Windows;
using System.Windows.Data;

namespace MyLab.Wpf.Converters
{
    public class EmptyCollectionToVisibilityConverter : IValueConverter
    {
        public Visibility WhenEmpty { get; set; } = Visibility.Collapsed;

        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as ICollection;

            bool invert = parameter != null && parameter.ToString() == "invert";

            return collection == null || collection.Count == 0
                ? (invert ? Visibility.Visible : WhenEmpty)
                : (invert ? WhenEmpty : Visibility.Visible);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
