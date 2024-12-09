using System;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;

namespace login_full.Converters
{
    public class BoolToGreenConverter : IValueConverter
    {
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