using System;
using System.Windows;

namespace MyLab.Wpf.Converters
{
    public class BoolToVisibilityConverter : ValueConverter<bool, Visibility>
    {
        public Visibility NegativeValue { get; set; } = Visibility.Collapsed;

        protected override Visibility Convert(bool source, object parameter)
        {
            bool invert = (string)parameter == "invert";
            var trueVal = invert ? NegativeValue : Visibility.Visible;
            var falseVal = invert ? Visibility.Visible : NegativeValue;


            return source ? trueVal : falseVal;
        }
    }
}
