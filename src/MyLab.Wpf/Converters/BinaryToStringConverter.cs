using System.Text;

namespace MyLab.Wpf.Converters
{
    public class BinaryToStringConverter : ValueConverter<byte[], string>
    {
        protected override string Convert(byte[] source, object parameter)
        {
            return source == null ? string.Empty : Encoding.UTF8.GetString(source);
        }
    }
}
