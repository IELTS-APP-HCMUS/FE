using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Globalization;
using Windows.UI.Xaml.Data;


namespace login_full.Converters
{
	/// <summary>
	/// Converter chuyển đổi boolean thành màu sắc
	/// </summary>
	/// <remarks>
	/// Chuyển đổi:
	/// - true -> Green
	/// - false -> Red
	/// </remarks>
	public class BoolToColorConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi từ boolean sang brush màu
		/// </summary>
		/// <returns>SolidColorBrush với màu tương ứng</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value 
                ? new SolidColorBrush(Colors.Green) 
                : new SolidColorBrush(Colors.Red);
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 