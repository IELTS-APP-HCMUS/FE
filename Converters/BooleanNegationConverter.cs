using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Converters
{
	/// <summary>
	/// Converter đảo ngược giá trị boolean
	/// </summary>
	/// <remarks>
	/// Chuyển đổi:
	/// - true -> false
	/// - false -> true
	/// </remarks>
	public class BooleanNegationConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi từ boolean sang giá trị đảo ngược
		/// </summary>
		/// <param name="value">Giá trị boolean cần đảo ngược</param>
		/// <param name="targetType">Type đích</param>
		/// <param name="parameter">Tham số phụ</param>
		/// <param name="language">Ngôn ngữ</param>
		/// <returns>Giá trị boolean đảo ngược</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            return !(bool)value;
        }
    }
}
