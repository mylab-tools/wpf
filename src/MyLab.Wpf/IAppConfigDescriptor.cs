using Microsoft.Extensions.Configuration;

namespace MyLab.Wpf
{
    /// <summary>
    /// Describes application configuration
    /// </summary>
    public interface IAppConfigDescriptor
    {
        /// <summary>
        /// Modifies <see cref="IConfigurationBuilder"/>
        /// </summary>
        void Describe(IConfigurationBuilder configurationBuilder);
    }
}
