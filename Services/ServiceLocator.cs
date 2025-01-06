using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.DependencyInjection.Extensions;
using login_full.ViewModels;
using System;

namespace login_full.Services
{
    /// <summary>
    // ServiceLocator quản lý việc đăng ký và cung cấp các dịch vụ trong ứng dụng.
    // </summary>
    public static class ServiceLocator
    {
        private static IServiceProvider _serviceProvider;
        /// <summary>
        /// Khởi tạo các dịch vụ và ViewModel.
        /// </summary>
        static ServiceLocator()
        {
            var services = new ServiceCollection();

            // Register services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IReadingTestService, ReadingTestService>();
            services.AddSingleton<LocalStorageService>();
            services.AddSingleton<IPdfExportService, PdfExportService>();
            services.AddSingleton<DictionaryService>();
            services.AddSingleton<TextHighlightService>();

            // Register ViewModels
            services.AddTransient<HistoryViewModel>();

            _serviceProvider = services.BuildServiceProvider();
        }

        /// <summary>
        /// Lấy một dịch vụ đã đăng ký.
        /// </summary>
        /// <typeparam name="T">Loại dịch vụ cần lấy</typeparam>
        /// <returns>Đối tượng dịch vụ</returns>
        /// <exception cref="InvalidOperationException">Ném ra khi ServiceProvider chưa được khởi tạo</exception>
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
