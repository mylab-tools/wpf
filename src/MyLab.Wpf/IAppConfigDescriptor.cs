using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

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

    class DelegateAppConfigDescriptor : IAppConfigDescriptor
    {
        private readonly Action<IConfigurationBuilder> _decriptor;

        public DelegateAppConfigDescriptor(Action<IConfigurationBuilder> decriptor)
        {
            _decriptor = decriptor ?? throw new ArgumentNullException(nameof(decriptor));
        }

        public void Describe(IConfigurationBuilder configurationBuilder)
        {
            _decriptor(configurationBuilder);
        }
    }
}
