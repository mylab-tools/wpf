namespace MyLab.Wpf.Converters
{
    public class NullConverter : ValueConverter<object, string>
    {
        public string NullString { get; set; } = "[null]";
        protected override string Convert(object source, object parameter)
        {
            return source != null ? source.ToString() : NullString;
        }
    }
}
