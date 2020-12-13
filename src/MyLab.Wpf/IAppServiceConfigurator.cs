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
}