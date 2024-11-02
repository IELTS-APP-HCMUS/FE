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
using Windows.Graphics;
using System.Windows;

using Microsoft.UI.Windowing;




namespace login_full
{
    public sealed partial class HomePage : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        private CalendarManager calendarManager;
        private ScheduleManager scheduleManager;
        // size of home page
        // size of the window
        
        public HomePage()
        {
            this.InitializeComponent();

            calendarManager = new CalendarManager(CalendarGrid, MonthYearDisplay);
            scheduleManager = new ScheduleManager(ScheduleListView);
            calendarManager.GenerateCalendarDays(DateTime.Now);

            // set size of home page
            


        }
        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            //var scrollViewer = sender as ScrollViewer;
            //if (scrollViewer.VerticalOffset > 0) // Adjust the offset value as needed
            //{
            //    VisualStateManager.GoToState(this, "CollapsedHeader", true);
            //}
            //else
            //{
            //    VisualStateManager.GoToState(this, "ExpandedHeader", true);
            //}
        }

        private void Home_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(HomePage));
        }
        private void UserHeaderButton_Click(object sender, RoutedEventArgs e)
        {
            // VisualStateManager.GoToState(this, "ExpandedHeader", true);
            //var flyout = (sender as Button)?.Flyout;
            //if (flyout != null)
            //{
            //    flyout.ShowAt(sender as FrameworkElement);
            //}

        }

        // click này ở 5 button điểm
        private void ScoreCategoryButton_Click(object sender, RoutedEventArgs e)
        {
            Button clickedButton = sender as Button;
            if (clickedButton != null)
            {
                string category = clickedButton.Content.ToString();
                IeltsScorePopup.IsOpen = true;
            }
        }


        // click nằm ở popup exit
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            IeltsScorePopup.IsOpen = false;
        }

        //click này nằm ở popup save
        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            double readingScore = double.TryParse(ReadingScoreTextBox.Text, out readingScore) ? readingScore : 0;
            double listeningScore = double.TryParse(ListeningScoreTextBox.Text, out listeningScore) ? listeningScore : 0;
            double writingScore = double.TryParse(WritingScoreTextBox.Text, out writingScore) ? writingScore : 0;
            double speakingScore = double.TryParse(SpeakingScoreTextBox.Text, out speakingScore) ? speakingScore : 0;

            double averageScore = (readingScore + listeningScore + writingScore + speakingScore) / 4;

            OverallScoreTextBlock.Text = $"{averageScore:0.0}";

            // Update the individual score buttons=
            ReadingScoreTextBlock.Text = $"{readingScore:0.0}";
            ListeningScoreTextBlock.Text = $"{listeningScore:0.0}";
            WritingScoreTextBlock.Text = $"{writingScore:0.0}";
            SpeakingScoreTextBlock.Text = $"{speakingScore:0.0}";

            IeltsScorePopup.IsOpen = false;
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

        //aboutus
        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutUsPage));
        }

    }


}