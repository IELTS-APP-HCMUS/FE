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
            services.AddSingleton<IPdfExportService, PdfExportService>();
            services.AddSingleton<MockDictionaryService>();
            services.AddSingleton<TextHighlightService>();

            // Register ViewModels
            services.AddTransient<HistoryViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }
      

        public static T GetService<T>() where T : class
        {
            if (_serviceProvider == null)
            {
                throw new InvalidOperationException("ServiceProvider is not initialized");
            }
            return _serviceProvider.GetService<T>();
        }
    }
}
