using System.Globalization;
using System.Windows.Controls;

namespace MyLab.Wpf
{
    class EmailValidationRule : ValidationRule
    {
        public override ValidationResult Validate(object value, CultureInfo cultureInfo)
        {
            var email = value.ToString();

            if(string.IsNullOrWhiteSpace(email))
                return ValidationResult.ValidResult;

            int dotPos = email.LastIndexOf('.');
            int dogPos = email.IndexOf('@');

            if(dogPos == -1 || dotPos == -1 || dotPos<dogPos || 0 == dogPos || email.Length-1 == dotPos)
                return new ValidationResult(false, "Wrong format");
            return ValidationResult.ValidResult;
        }
    }
}
