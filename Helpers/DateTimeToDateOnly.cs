using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Globalization;
using Microsoft.UI.Xaml.Data;
using login_full.Models;

namespace login_full.Helpers
{
	class DateTimeToDateOnly : IValueConverter
	{
		public object Convert(object value, Type targetType, object parameter, string language)
		{
			string dateTime = (string)value;
			if(dateTime == "- / - / -")
			{
				// return date now
				return DateOnly.Parse(DateTime.Now.ToString("yyyy-MM-dd"));
			}
            // DateOnly date = DateOnly.Parse(dateTime.Split(" ")[0]);
            var dateFormat = "dd/MM/yyyy";
            DateOnly date = DateOnly.ParseExact(dateTime, dateFormat);
            return date;
		}
		public object ConvertBack(object value, Type targetType, object parameter, string language)
		{
			throw new NotImplementedException();
		}
	}
}
