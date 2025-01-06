using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using login_full.Services;
using login_full.Models;

namespace login_full.Helpers
{
    /// <summary>
    ///Quản lý lịch trình và sự kiện.
    // Cung cấp các chức năng cập nhật và thêm sự kiện mới.
    // </summary>
    public class ScheduleManager
    {
        /// <summary>
        /// ListView hiển thị lịch trình.
        /// </summary>
        private readonly ListView ScheduleListView;
        /// <summary>
        /// Service quản lý lịch trình.
        /// </summary>
        private readonly ScheduleService _scheduleService;

        public ScheduleManager(ListView scheduleListView)
        {
            ScheduleListView = scheduleListView;
            _scheduleService = new ScheduleService();
        }
        /// <summary>
        /// Cập nhật lịch trình cho ngày cụ thể.
        /// </summary>
        /// <param name="date">Ngày cần cập nhật lịch trình</param>
        public void UpdateSchedule(DateTime date)
        {
            var scheduleItems = new List<ScheduleItem>
            {
                new ScheduleItem { Time = "2 PM", Activity = "Adobe XD Live Class" },
                new ScheduleItem { Time = "4 PM", Activity = "Team Meeting" }
            };

            ScheduleListView.ItemsSource = scheduleItems;
        }
        /// <summary>
        /// Cập nhật danh sách lịch trình từ API.
        /// </summary>
        /// <returns>Task hoàn thành việc cập nhật</returns>
        public async Task UpdateSchedulesAsync()
        {
            var schedules = await _scheduleService.GetSchedulesAsync();
            ScheduleListView.ItemsSource = schedules;
        }
        /// <summary>
        /// Thêm sự kiện mới vào lịch trình.
        /// </summary>
        /// <param name="xamlRoot">XamlRoot để hiển thị dialog</param>
        public async void AddNewEvent(XamlRoot xamlRoot)
        {
            ContentDialog dialog = new ContentDialog
            {
                Title = "Add New Event",
                PrimaryButtonText = "Add",
                CloseButtonText = "Cancel",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = xamlRoot
            };

            TextBox timeInput = new TextBox { PlaceholderText = "Time (e.g., 2 PM)" };
            TextBox activityInput = new TextBox { PlaceholderText = "Activity" };

            StackPanel panel = new StackPanel();
            panel.Children.Add(timeInput);
            panel.Children.Add(activityInput);

            dialog.Content = panel;

            ContentDialogResult result = await dialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                string time = timeInput.Text;
                string activity = activityInput.Text;

                if (!string.IsNullOrWhiteSpace(time) && !string.IsNullOrWhiteSpace(activity))
                {
                    ScheduleItem newItem = new ScheduleItem { Time = time, Activity = activity };

                    bool isSuccess = await _scheduleService.AddScheduleAsync(newItem);

                    var currentItems = ScheduleListView.ItemsSource as List<ScheduleItem> ?? new List<ScheduleItem>();
                    currentItems.Add(newItem);

                    ScheduleListView.ItemsSource = null;
                    ScheduleListView.ItemsSource = currentItems;
                }
            }
        }
    }
}