using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace login_full.Converters
{
	/// <summary>
	/// Converter chuyển đổi boolean thành màu xanh hoặc đỏ
	/// </summary>
	/// <remarks>
	/// Chuyển đổi:
	/// - true -> Green
	/// - false -> Red (nếu parameter là true)
	/// - false -> Black (nếu parameter là false hoặc không có)
	/// </remarks>
	public class BoolToGreenConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi từ boolean sang màu sắc
		/// </summary>
		/// <returns>Màu sắc tương ứng với giá trị boolean và parameter</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            bool isCorrect = (bool)value;
            bool isWrong = parameter != null && (bool)parameter;

            if (isCorrect)
                return new SolidColorBrush(Colors.Green);
            else if (isWrong)
                return new SolidColorBrush(Colors.Red);
            else
                return new SolidColorBrush(Colors.Black);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 