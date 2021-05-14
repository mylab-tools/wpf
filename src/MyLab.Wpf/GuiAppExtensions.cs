using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Wpf
{
    /// <summary>
    /// Extensions for <see cref="GuiApp"/>
    /// </summary>
    public static class GuiAppExtensions
    {
        /// <summary>
        /// Specifies configuration descriptor which defines config sources
        /// </summary>
        public static GuiApp UseConfiguration(this GuiApp guiApp, Action<IConfigurationBuilder> description)
        {
            if (description == null) throw new ArgumentNullException(nameof(description));
            if (guiApp == null) throw new ArgumentNullException(nameof(guiApp));

            return guiApp.UseConfiguration(new DelegateAppConfigDescriptor(description));
        }

        /// <summary>
        /// Specifies service configurator which build service collection
        /// </summary>
        public static GuiApp UseServiceConfiguration(this GuiApp guiApp, Action<IConfiguration, IServiceCollection> configuration)
        {
            if (guiApp == null) throw new ArgumentNullException(nameof(guiApp));
            if (configuration == null) throw new ArgumentNullException(nameof(configuration));

            return guiApp.UseServiceConfiguration(new DelegateAppServiceConfigurator(configuration));
        }
    }
}