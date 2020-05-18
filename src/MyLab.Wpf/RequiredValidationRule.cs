using System.Globalization;
using System.Windows.Controls;

namespace MyLab.Wpf
{
    public class RequiredValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            return value == null || string.IsNullOrWhiteSpace(value.ToString())
                ? new ValidationResult(false, "Value is null")
                : ValidationResult.ValidResult;
        }
    }
}