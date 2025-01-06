using Microsoft.UI.Xaml.Data;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Converters
{
	/// <summary>
	/// Converter chia chiều rộng cho bốn
	/// </summary>
	/// <remarks>
	/// Sử dụng để tính toán chiều rộng của các phần tử khi chia layout thành bốn cột
	/// </remarks>
	public class DivideByFourConverter : IValueConverter
    {
		/// <summary>
		/// Chia chiều rộng cho bốn
		/// </summary>
		/// <returns>Một phần tư chiều rộng trừ đi padding/margin</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is double width)
            {
                return (width - 60) / 4; // Trừ đi padding/margin
            }
            return 0;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
