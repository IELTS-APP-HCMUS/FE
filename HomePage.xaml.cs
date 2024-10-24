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
using Windows.Storage;
using Microsoft.UI;




namespace login_full
{
    public sealed partial class HomePage : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private CalendarManager calendarManager;
        private ScheduleManager scheduleManager;

        public HomePage()
        {
            this.InitializeComponent();

            calendarManager = new CalendarManager(CalendarGrid, MonthYearDisplay);
            scheduleManager = new ScheduleManager(ScheduleListView);
            calendarManager.GenerateCalendarDays(DateTime.Now);
        }

        private void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
            localSettings.Values.Remove("Username");
            localSettings.Values.Remove("PasswordInBase64");
            localSettings.Values.Remove("EntropyInBase64");

            // Get the current window
            var window = (Application.Current as App)?.MainWindow;

            if (window != null)
            {
                // Create and navigate to a new instance of MainWindow
                var newMainWindow = new MainWindow();
                newMainWindow.Activate();
                window.Close();
            }
        }

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var flyout = (sender as Button)?.Flyout;
            if (flyout != null)
            {
                flyout.ShowAt(sender as FrameworkElement);
            }
        }
        private void PreviousMonth_Click(object sender, RoutedEventArgs e)
        {
            calendarManager.PreviousMonth();
        }

        private void NextMonth_Click(object sender, RoutedEventArgs e)
        {
           calendarManager.NextMonth();
        }

        private void DayButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = (Button)sender;
            DateTime selectedDate = calendarManager.DayButtonClick(clickedButton);
            scheduleManager.UpdateSchedule(selectedDate);
        }


        private void AddNewEvent_Click(object sender, RoutedEventArgs e)
        {
            scheduleManager.AddNewEvent(this.XamlRoot);
        }


        private void ExamDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (args.NewDate.HasValue)
            {
                DateTime selectedDate = args.NewDate.Value.Date;
                ExamDateButton.Content = selectedDate.ToString("dd / MM / yyyy");
                UpdateRemainingDays(selectedDate);
            }
            else
            {
                ExamDateButton.Content = "- / - / -";
                RemainingDaysText.Text = "- ngày";
            }
        }

        private void UpdateRemainingDays(DateTime examDate)
        {
            int remainingDays = (examDate - DateTime.Today).Days;
            RemainingDaysText.Text = $"{remainingDays} ngày";
        }
        private void ScoreCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                string category = clickedButton.Content.ToString();
                // Handle the score category selection
                // You might want to update the UI or store the selected category
            }
        }
        //aboutus
        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutUsPage));
        }

    }


}