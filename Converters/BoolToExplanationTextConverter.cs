using Microsoft.UI.Xaml.Data;
using System;

namespace login_full.Converters
{
	/// <summary>
	/// Converter chuyển đổi giá trị boolean thành văn bản giải thích
	/// </summary>
	/// <remarks>
	/// Chuyển đổi:
	/// - true -> "Ẩn giải thích"
	/// - false -> "Xem giải thích"
	/// </remarks>
	public class BoolToExplanationTextConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi từ boolean sang văn bản giải thích
		/// </summary>
		/// <returns>Văn bản tương ứng với trạng thái boolean</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? "Ẩn giải thích" : "Xem giải thích";
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 