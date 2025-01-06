using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using System;
using System.Threading.Tasks;
using Windows.UI;
using Microsoft.UI;

namespace login_full.Helpers
{
    /// <summary>
    // Quản lý hiển thị thông báo dạng toast.
    // </summary>
    public class ToastManager(Grid rootGrid)
    {
        /// <summary>
        /// Grid gốc để hiển thị toast.
        /// </summary>
        private readonly Grid RootGrid = rootGrid;
        /// <summary>
        /// Khởi tạo một instance mới của <see cref="ToastManager"/>.
        /// </summary>
        /// <param name="rootGrid">Grid gốc để hiển thị toast</param>
        public async void ShowToast(string message)
        {
            // Tạo khung hiển thị toast
            Border toastContainer = new Border
            {
                Background = new SolidColorBrush(Color.FromArgb(200, 0, 0, 0)), // Màu nền (đen mờ)
                CornerRadius = new CornerRadius(8),
                Padding = new Thickness(10),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center,
                Margin = new Thickness(10),
                Child = new TextBlock
                {
                    Text = message,
                    Foreground = new SolidColorBrush(Colors.White),
                    FontSize = 16,
                    TextWrapping = TextWrapping.Wrap,
                    TextAlignment = TextAlignment.Center
                }
            };

            // Thêm toast vào giao diện
            RootGrid.Children.Add(toastContainer);

            // Chờ 5 giây
            await Task.Delay(1000);

            // Gỡ toast ra khỏi giao diện
            RootGrid.Children.Remove(toastContainer);
        }
    }
}
