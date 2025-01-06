using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using Windows.UI;

namespace login_full.Views
{
    public class EvenRowBackgroundConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi số trang thành màu nền
		/// </summary>
		/// <returns>Màu nền tương ứng với trạng thái của số trang</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int index)
            {
                // Nếu index là số chẵn, trả về màu nền xám nhạt, ngược lại trả về màu trắng
                return index % 2 == 0 
                    ? new SolidColorBrush(Color.FromArgb(255, 245, 245, 245)) // Màu xám nhạt cho hàng chẵn
                    : new SolidColorBrush(Colors.White); // Màu trắng cho hàng lẻ
            }
            return new SolidColorBrush(Colors.White); // Mặc định trả về màu trắng
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 