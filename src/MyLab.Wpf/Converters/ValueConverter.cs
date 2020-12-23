using System;
using System.Globalization;
using System.Windows.Data;

namespace MyLab.Wpf.Converters
{
    /// <summary>
    /// The base class for value converters
    /// </summary>
    public abstract class ValueConverter<TSource, TDest> : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return Convert((TSource) value, parameter);
        }

        public object ConvertBack(object value, Type targetType, object parameter, CultureInfo culture)
        {
            return ConvertBack((TDest) value, parameter);
        }

        /// <summary>
        /// Override to define convert logic
        /// </summary>
        protected abstract TDest Convert(TSource source, object parameter);

        /// <summary>
        /// Override to define convert-back logic
        /// </summary>
        protected virtual TSource ConvertBack(TDest dest, object parameter)
        {
            throw new NotSupportedException("Convert back is not supported");
        }
    }
}
