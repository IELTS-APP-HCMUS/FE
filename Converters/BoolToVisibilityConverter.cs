using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;

namespace login_full.Converters
{
    public class BoolToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is bool boolValue)
            {
                // Nếu parameter là "Inverse", đảo ngược kết quả
                if (parameter is string param && param.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
                {
                    return boolValue ? Visibility.Collapsed : Visibility.Visible;
                }
                
                // Mặc định: true -> Visible, false -> Collapsed
                return boolValue ? Visibility.Visible : Visibility.Collapsed;
            }

            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            if (value is Visibility visibility)
            {
                // Nếu parameter là "Inverse", đảo ngược kết quả
                if (parameter is string param && param.Equals("Inverse", StringComparison.OrdinalIgnoreCase))
                {
                    return visibility != Visibility.Visible;
                }

                // Mặc định: Visible -> true, Collapsed -> false
                return visibility == Visibility.Visible;
            }

            return false;
        }
    }
} 