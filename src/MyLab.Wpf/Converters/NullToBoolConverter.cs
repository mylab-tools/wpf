namespace MyLab.Wpf.Converters
{
    public class NullToBoolConverter : ValueConverter<object, bool>
    {
        protected override bool Convert(object source, object parameter)
        {
            var invert = (parameter as string) == "invert";

            bool isNull = source == null;

            return invert ? !isNull : isNull;
        }
    }
}
