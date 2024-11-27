using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI.WebUI;

namespace login_full.Services
{
    public class NavigationService : INavigationService
    {
        private Frame _frame;

        //public NavigationService()
        //{
            
        //}

        // Hoặc cho phép set Frame sau khi khởi tạo
        public void Initialize(Frame frame)
        {
            _frame = frame ?? throw new ArgumentNullException(nameof(frame));
        }

        public Task NavigateToAsync(Type pageType)
        {
            if (_frame == null)
            {
                throw new InvalidOperationException("Navigation frame is not initialized.");
            }

            _frame.Navigate(pageType);
            return Task.CompletedTask;
        }

        public Task NavigateToAsync(Type pageType, object parameter)
        {
            try
            {
                if (_frame == null)
                {
					var mainWindow = App.MainWindow;
					if (mainWindow == null)
                        throw new InvalidOperationException("MainWindow is not available");

                    _frame = mainWindow.Content as Frame;
                    if (_frame == null)
                        throw new InvalidOperationException("Navigation frame is not initialized");
                }

				// Sử dụng DispatcherQueue từ App.MainWindow
				var dispatcherQueue = App.MainWindow.DispatcherQueue;
				dispatcherQueue.TryEnqueue(() =>
                {
                    _frame.Navigate(pageType, parameter);
                });

                return Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Navigation error: {ex.Message}");
                System.Diagnostics.Debug.WriteLine($"StackTrace: {ex.StackTrace}");
                throw;
            }
        }
    }
}
