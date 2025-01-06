using Microsoft.UI.Xaml.Data;
using Windows.UI;
using System;

namespace login_full.Converters
{
    public class PageNumberBackgroundConverter : IValueConverter
    {
        public object Convert(object value, Type targetType, object parameter, string language)
        {
            if (value is int currentPage && parameter is int pageNumber)
            {
                return currentPage == pageNumber 
                    ? Color.FromArgb(255, 39, 80, 81)  // #275051
                    : Color.FromArgb(255, 73, 187, 189); // #49BBBD
            }
            return Color.FromArgb(255, 73, 187, 189); // #49BBBD
        }

        public object ConvertBack(object value, Type targetType, object parameter, string language)
        {
            throw new NotImplementedException();
        }
    }
}
