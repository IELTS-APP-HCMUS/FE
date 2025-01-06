using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Converters
{
	/// <summary>
	/// Chuyển đổi từ boolean sang Visibility
	/// </summary>
	/// <param name="parameter">Nếu là "Inverse" sẽ đảo ngược kết quả</param>
	/// <returns>Trạng thái Visibility</returns>
	public class BoolToSolidColorBrushConverter : IValueConverter
    {
        public SolidColorBrush TrueBrush { get; set; }
        public SolidColorBrush FalseBrush { get; set; }
		/// <summary>
		/// Chuyển đổi từ boolean sang Visibility
		/// </summary>
		/// <param name="parameter">Nếu là "Inverse" sẽ đảo ngược kết quả</param>
		/// <returns>Trạng thái Visibility</returns>
		public object Convert(object value, Type targetType, object parameter, string language)
        {
            return (bool)value ? TrueBrush : FalseBrush;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
