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
    public class ScheduleManager
    {
        private readonly ListView ScheduleListView;
        private readonly ScheduleService _scheduleService;

        public ScheduleManager(ListView scheduleListView)
        {
            ScheduleListView = scheduleListView;
            _scheduleService = new ScheduleService();
        }

        public void UpdateSchedule(DateTime date)
        {
            var scheduleItems = new List<ScheduleItem>
            {
                new ScheduleItem { Time = "2 PM", Activity = "Adobe XD Live Class" },
                new ScheduleItem { Time = "4 PM", Activity = "Team Meeting" }
            };

            ScheduleListView.ItemsSource = scheduleItems;
        }
        public async Task UpdateSchedulesAsync()
        {
            var schedules = await _scheduleService.GetSchedulesAsync();
            ScheduleListView.ItemsSource = schedules;
        }

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