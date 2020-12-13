using System;
using System.Windows;
using Microsoft.Extensions.DependencyInjection;

namespace MyLab.Wpf
{
    /// <summary>
    /// Integration extensions for <see cref="Application"/>
    /// </summary>
    public static class GuiAppIntegration
    {
        /// <summary>
        /// Creates <see cref="GuiApp"/> object based on windows application
        /// </summary>
        public static GuiApp CreateGuiApp(this Application app)
        {
            return CreateGuiApp<DefaultGuiManager>(app);
        }

        /// <summary>
        /// Creates <see cref="GuiApp"/> object based on windows application
        /// </summary>
        public static GuiApp CreateGuiApp<TGuiManager>(this Application app)
            where TGuiManager : class, IGuiManager
        {
            if (app == null) throw new ArgumentNullException(nameof(app));

            var srvDesc = new ServiceDescriptor(typeof(IGuiManager), typeof(TGuiManager), ServiceLifetime.Singleton);
            return new GuiApp(srvDesc, app);
        }

        /// <summary>
        /// Creates <see cref="GuiApp"/> object based on windows application
        /// </summary>
        public static GuiApp CreateGuiApp<TGuiManager>(this Application app, TGuiManager guiMgrInstance)
            where TGuiManager : class, IGuiManager
        {
            if (app == null) throw new ArgumentNullException(nameof(app));
            if (guiMgrInstance == null) throw new ArgumentNullException(nameof(guiMgrInstance));

            var srvDesc = new ServiceDescriptor(typeof(IGuiManager), guiMgrInstance);
            return new GuiApp(srvDesc, app);
        }
    }
}