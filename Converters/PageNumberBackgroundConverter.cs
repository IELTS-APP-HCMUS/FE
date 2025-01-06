using Microsoft.UI.Xaml.Data;
using Windows.UI;
using System;

namespace login_full.Converters
{
	/// <summary>
	/// Converter chuyển đổi số trang thành màu nền
	/// </summary>
	/// <remarks>
	/// Chuyển đổi:
	/// - Nếu số trang hiện tại trùng với số trang được chọn, trả về màu "#275051"
	/// - Ngược lại, trả về màu "Gray"
	/// </remarks>
	public class PageNumberBackgroundConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi số trang thành màu nền
		/// </summary>
		/// <returns>Màu nền tương ứng với trạng thái của số trang</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int currentPage && parameter is int pageNumber)
            {
                return currentPage == pageNumber 
                    ? Color.FromArgb(255, 39, 80, 81)  // #275051
                    : Color.FromArgb(255, 73, 187, 189); // #49BBBD
            }
            return Color.FromArgb(255, 73, 187, 189); // #49BBBD
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
