using System;
using Microsoft.UI.Xaml.Data;

namespace login_full.Converters
{
	/// <summary>
	/// Converter ??nh d?ng th?i gian
	/// </summary>
	/// <remarks>
	/// Chuy?n ??i DateTime sang chu?i ??nh d?ng "HH:mm:ss"
	/// </remarks>
	public class TimeFormatConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is DateTime time)
            {
                return time.ToString("HH:mm:ss");
            }
            return string.Empty;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 