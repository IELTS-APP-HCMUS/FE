using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using login_full.ViewModels;
using System;

namespace login_full.Services
{
    public static class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;

        static ServiceLocator()
        {
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IReadingTestService, ReadingTestService>();
            services.AddSingleton<LocalStorageService>();

            // Register ViewModels
            services.AddTransient<HistoryViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }

        public static T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }
    }
}
