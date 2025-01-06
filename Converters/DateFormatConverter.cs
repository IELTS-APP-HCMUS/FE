using System;
using Microsoft.UI.Xaml.Data;

namespace login_full.Converters
{
	/// <summary>
	/// Converter định dạng ngày tháng
	/// </summary>
	/// <remarks>
	/// Chuyển đổi DateTime sang chuỗi định dạng "dd/MM/yy"
	/// </remarks>
	public class DateFormatConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi DateTime sang chuỗi định dạng ngày
		/// </summary>
		/// <returns>Chuỗi ngày đã định dạng</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime date)
            {
                return date.ToString("dd/MM/yy");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 