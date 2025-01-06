using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using login_full.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home
{
	/// <summary>
	/// Component hiển thị lịch
	/// </summary>
	/// <remarks>
	/// Chức năng:
	/// - Hiển thị calendar grid
	/// - Navigation giữa các tháng
	/// - Đánh dấu ngày hiện tại
	/// </remarks>
	public sealed partial class Calendar : UserControl
    {
        private readonly CalendarManager calendarManager = null;
        public Calendar()
        {
            this.InitializeComponent();
            // Giả lập dữ liệu trả về từ API
            //List<Item> items = new List<Item>
            //{
            //    new Item { date_created = "2024-12-01T09:51:28.986821Z" },

            //    new Item { date_created = "2024-12-02T09:50:26.518248Z" },
            //    new Item { date_created = "2024-12-02T09:50:04.712262Z" },

            //    new Item { date_created = "2024-12-03T09:49:57.712262Z" },
            //    new Item { date_created = "2024-12-03T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-03T09:50:26.518248Z" },

            //    new Item { date_created = "2024-12-04T09:50:04.712262Z" },
            //    new Item { date_created = "2024-12-04T09:49:57.712262Z" },
            //    new Item { date_created = "2024-12-04T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-04T09:50:26.518248Z" },

            //    new Item { date_created = "2024-12-05T09:50:04.712262Z" },
            //    new Item { date_created = "2024-12-05T09:49:57.712262Z" },
            //    new Item { date_created = "2024-12-05T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-05T09:50:26.518248Z" },
            //    new Item { date_created = "2024-12-05T09:50:04.712262Z" },

            //    new Item { date_created = "2024-12-06T09:49:57.712262Z" },
            //    new Item { date_created = "2024-12-06T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-06T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-06T09:50:26.518248Z" },
            //    new Item { date_created = "2024-12-06T09:50:04.712262Z" },
            //    new Item { date_created = "2024-12-06T09:49:57.712262Z" },

            //    new Item { date_created = "2024-12-07T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-07T09:50:26.518248Z" },
            //    new Item { date_created = "2024-12-07T09:50:04.712262Z" },
            //    new Item { date_created = "2024-12-07T09:49:57.712262Z" },
            //    new Item { date_created = "2024-12-07T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-07T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-07T09:50:26.518248Z" },

            //    new Item { date_created = "2024-12-08T09:50:04.712262Z" },
            //    new Item { date_created = "2024-12-08T09:49:57.712262Z" },
            //    new Item { date_created = "2024-12-08T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-08T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-08T09:50:26.518248Z" },
            //    new Item { date_created = "2024-12-08T09:50:04.712262Z" },
            //    new Item { date_created = "2024-12-08T09:49:57.712262Z" },
            //    new Item { date_created = "2024-12-08T09:51:28.986821Z" },
            //    new Item { date_created = "2024-12-08T09:50:26.518248Z" },
            //    new Item { date_created = "2024-12-08T09:50:04.712262Z" },
            //    new Item { date_created = "2024-12-08T09:49:57.712262Z" },
            //    new Item { date_created = "2024-12-08T09:51:28.986821Z" },
            //};
            calendarManager = new CalendarManager(CalendarGrid, MonthYearDisplay);
            //List<SubmitTestTimeItem> items = await calendarManager.GetSubmitTimeHistoryAsync();
            //calendarManager.ProcessDateCreatedCount(items);

            //// Tạo lịch với dữ liệu được tô màu
            //calendarManager.GenerateCalendarDays(DateTime.Now);
            ////calendarManager.GenerateCalendarDays(DateTime.Now);
            InitializeAsync();
        }
        private async void InitializeAsync()
        {
            // Lấy dữ liệu bất đồng bộ
            List<SubmitTestTimeItem> items = await calendarManager.GetSubmitTimeHistoryAsync();

            // Xử lý dữ liệu
            calendarManager.ProcessDateCreatedCount(items);

            // Tạo lịch
            calendarManager.GenerateCalendarDays(DateTime.Now);
        }
        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            calendarManager.PreviousMonth();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
            calendarManager.NextMonth();
        }
    }
}