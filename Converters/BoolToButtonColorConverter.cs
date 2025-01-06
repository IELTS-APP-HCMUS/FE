using Microsoft.UI.Xaml.Data;
using Microsoft.UI;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Converters
{
	/// <summary>
	/// Converter chuyển đổi boolean thành màu nút
	/// </summary>
	/// <remarks>
	/// Chuyển đổi:
	/// - true -> LightBlue
	/// - false -> Gray
	/// </remarks>
	public class BoolToButtonColorConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi từ boolean sang màu
		/// </summary>
		/// <param name="value">Giá trị boolean</param>
		/// <returns>Màu tương ứng cho nút</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? Colors.LightBlue : Colors.White;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
