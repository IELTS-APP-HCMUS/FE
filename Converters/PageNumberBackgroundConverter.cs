using login_full.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Media;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.UI;

namespace login_full.Converters
{
    public class PageNumberBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value != null && parameter != null)
            {
                int pageNumber = System.Convert.ToInt32(value);
                int currentPage = System.Convert.ToInt32(parameter);
                return pageNumber == currentPage
                    ? new SolidColorBrush(Colors.CornflowerBlue)
                    : new SolidColorBrush(Color.FromArgb(255, 39, 80, 81));
            }
            return new SolidColorBrush(Color.FromArgb(255, 39, 80, 81));
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
