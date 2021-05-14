using System;
using System.Collections;
using System.Globalization;
using System.Windows.Data;

namespace MyLab.Wpf.Converters
{
    public class EmptyCollectionToBoolConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            var collection = value as ICollection;

            bool invert = parameter != null && parameter.ToString() == "invert";
            bool isEmpty = collection == null || collection.Count == 0;

            return invert ? !isEmpty : isEmpty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            throw new NotSupportedException();
        }
    }
}
