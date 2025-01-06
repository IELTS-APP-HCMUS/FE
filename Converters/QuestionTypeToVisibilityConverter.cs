using login_full.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Data;
using System;
using System.Globalization;
using Windows.UI.Xaml;
using Windows.UI.Xaml.Data;

namespace login_full.Converters
{
	/// <summary>
	/// Converter chuyển đổi loại câu hỏi thành trạng thái hiển thị
	/// </summary>
	/// <remarks>
	/// Chuyển đổi:
	/// - Nếu loại câu hỏi trùng với loại được chỉ định trong parameter, trả về Visibility.Visible
	/// - Ngược lại, trả về Visibility.Collapsed
	/// - Hỗ trợ parameter "Inverse" để đảo ngược kết quả
	/// </remarks>
	public class QuestionTypeToVisibilityConverter : IValueConverter
    {
		/// <summary>
		/// Chuyển đổi loại câu hỏi thành trạng thái hiển thị
		/// </summary>
		/// <returns>Trạng thái hiển thị tương ứng với loại câu hỏi</returns>
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