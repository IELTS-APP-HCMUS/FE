using login_full.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace login_full.Converters
{
    public class QuestionTypeToVisibilityConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is QuestionType questionType && parameter is string paramString)
            {
                var parameters = paramString.Split(',');
                if (parameters.Length > 0)
                {
                    var parsedType = Enum.Parse<QuestionType>(parameters[0]);
                    bool isMatch = questionType == parsedType;

                    if (parameters.Length > 1 && parameters[1] == "Inverse")
                    {
                        isMatch = !isMatch;
                    }

                    return isMatch ? Visibility.Visible : Visibility.Collapsed;
                }
            }
            return Visibility.Collapsed;
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
} 