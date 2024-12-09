using Microsoft.UI.Xaml.Data;
using System;

namespace login_full.Converters
{
    public class BoolToExplanationTextConverter : IValueConverter
    {
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