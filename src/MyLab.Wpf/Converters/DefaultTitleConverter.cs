using System.Text;

namespace MyLab.Wpf.Converters
{
    /// <summary>
    /// Gets default title
    /// </summary>
    public class DefaultTitleConverter : ValueConverter<ViewModel, string>
    {
        public string Prefix { get; set; }

        protected override string Convert(ViewModel source, object parameter)
        {
            if (source == null) return "[none]";
            var name = source.GetType().Assembly.GetName();

            var sb = new StringBuilder();

            sb.Append(name.Name);
            sb.Append(" v");
            sb.Append(name.Version?.ToString(3));

            if (!string.IsNullOrWhiteSpace(Prefix))
            {
                sb.Append(" - ");
                sb.Append(Prefix);
            }

            return sb.ToString();
        }
    }
}
