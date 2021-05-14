using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Wpf
{
    /// <summary>
    /// Configure application services
    /// </summary>
    public interface IAppServiceConfigurator
    {
        /// <summary>
        /// Configure application services
        /// </summary>
        void Configure(IConfiguration config, IServiceCollection services);
    }

    class DelegateAppServiceConfigurator : IAppServiceConfigurator
    {
        private readonly Action<IConfiguration, IServiceCollection> _configurator;

        public DelegateAppServiceConfigurator(Action<IConfiguration, IServiceCollection> configurator)
        {
            _configurator = configurator ?? throw new ArgumentNullException(nameof(configurator));
        }

        public void Configure(IConfiguration config, IServiceCollection services)
        {
            _configurator(config, services);
        }
    }
}