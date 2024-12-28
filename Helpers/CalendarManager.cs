using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using Microsoft.UI.Xaml;
using login_full.Components.Home;
using System.Collections.Generic;
using Windows.UI;
using login_full.API_Services;
using System.Threading.Tasks;
using login_full.Context;
using login_full.Models;
using Newtonsoft.Json.Linq;

namespace login_full.Helpers
{
    public class SubmitTestTimeItem
    {
        public string DateCreated { get; set; }
    }
    public class CalendarManager
    {
        private readonly Grid CalendarGrid;
        private readonly TextBlock MonthYearDisplay;
        private DateTime currentDate;
        private Button selectedDateButton;
        private Dictionary<string, int> dateCount;
        private readonly ClientCaller clientCaller = new();

        public CalendarManager(Grid calendarGrid, TextBlock monthYearDisplay)
        {
            CalendarGrid = calendarGrid;
            MonthYearDisplay = monthYearDisplay;
            currentDate = DateTime.Now;

            // Create the CalendarDayButtonStyle programmatically
            var calendarDayButtonStyle = new Style(typeof(Button));
            calendarDayButtonStyle.Setters.Add(new Setter(Control.ForegroundProperty, new SolidColorBrush(Colors.Black)));
            calendarDayButtonStyle.Setters.Add(new Setter(Control.BackgroundProperty, new SolidColorBrush(Colors.Transparent)));
            calendarDayButtonStyle.Setters.Add(new Setter(Control.BorderThicknessProperty, new Thickness(0)));
            calendarDayButtonStyle.Setters.Add(new Setter(Control.PaddingProperty, new Thickness(5)));
            calendarDayButtonStyle.Setters.Add(new Setter(FrameworkElement.MarginProperty, new Thickness(2)));
            calendarDayButtonStyle.Setters.Add(new Setter(FrameworkElement.HorizontalAlignmentProperty, HorizontalAlignment.Stretch));
            calendarDayButtonStyle.Setters.Add(new Setter(FrameworkElement.VerticalAlignmentProperty, VerticalAlignment.Stretch));
            calendarDayButtonStyle.Setters.Add(new Setter(Control.FontSizeProperty, 14.0));
            calendarDayButtonStyle.Setters.Add(new Setter(FrameworkElement.MinWidthProperty, 40.0));
            calendarDayButtonStyle.Setters.Add(new Setter(FrameworkElement.MinHeightProperty, 40.0));

            // Add the style to the application resources
            Application.Current.Resources["CalendarDayButtonStyle"] = calendarDayButtonStyle;
        }
        public async Task<List<SubmitTestTimeItem>> GetSubmitTimeHistoryAsync()
        {
            try
            {
                var response = await clientCaller.GetAsync($"v1/answers/statistics?skill_id=1&type=1");

                if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(stringResponse);

                    JObject dataResponse = (JObject)jsonResponse["data"];
                    JArray items = (JArray)dataResponse["items"];
                    var histories = new List<SubmitTestTimeItem>();
                    foreach (var i in items)
                    {
                        string date_created = i["date_created"].ToString();
                        histories.Add(new SubmitTestTimeItem { DateCreated = date_created });
                    }
                    return histories;
                }
                return [];
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in GetTestHistoryAsync: {ex.Message}");
                return [];
            }
        }
        public void GenerateCalendarDays(DateTime date)
        {
            CalendarGrid.Children.Clear();
            CalendarGrid.RowDefinitions.Clear();
            CalendarGrid.ColumnDefinitions.Clear();

            for (int i = 0; i < 7; i++)
            {
                CalendarGrid.ColumnDefinitions.Add(new ColumnDefinition());
            }

            for (int i = 0; i < 7; i++)
            {
                CalendarGrid.RowDefinitions.Add(new RowDefinition());
            }

            string[] daysOfWeek = { "Sun", "Mon", "Tue", "Wed", "Thu", "Fri", "Sat" };
            for (int i = 0; i < 7; i++)
            {
                TextBlock dayHeader = new TextBlock
                {
                    Text = daysOfWeek[i],
                    HorizontalAlignment = HorizontalAlignment.Center,
                    VerticalAlignment = VerticalAlignment.Center,
                    Foreground = new SolidColorBrush(Colors.Black)
                };
                Grid.SetRow(dayHeader, 0);
                Grid.SetColumn(dayHeader, i);
                CalendarGrid.Children.Add(dayHeader);
            }

            DateTime firstDayOfMonth = new DateTime(date.Year, date.Month, 1);
            int daysInMonth = DateTime.DaysInMonth(date.Year, date.Month);

            int row = 1;
            int col = (int)firstDayOfMonth.DayOfWeek;

            for (int day = 1; day <= daysInMonth; day++)
            {
                DateTime currentDate = new DateTime(date.Year, date.Month, day);
                string dateString = currentDate.ToString("yyyy-MM-dd");
                int count = dateCount.ContainsKey(dateString) ? dateCount[dateString] : 0;

                Button dayButton = new Button
                {
                    Content = day.ToString(),
                    Style = (Style)Application.Current.Resources["CalendarDayButtonStyle"],
                    Background = GetColorBasedOnCount(count)
                };
                dayButton.Click += (sender, e) => { DayButtonClick((Button)sender); };

                Grid.SetRow(dayButton, row);
                Grid.SetColumn(dayButton, col);
                CalendarGrid.Children.Add(dayButton);

                col++;
                if (col > 6)
                {
                    col = 0;
                    row++;
                }
            }

            MonthYearDisplay.Text = date.ToString("MMMM yyyy");
        }
        public static SolidColorBrush GetColorBasedOnCount(int count)
        {
            // Tô màu xanh lá từ nhạt đến đậm dựa trên ngưỡng.
            if (count == 0)
                return new SolidColorBrush(Colors.Transparent);
            else if (count <= 2)
                return new SolidColorBrush(Color.FromArgb(255, 198, 255, 198)); // Rất nhạt
            else if (count <= 5)
                return new SolidColorBrush(Color.FromArgb(255, 144, 238, 144)); // Nhạt
            else if (count <= 10)
                return new SolidColorBrush(Color.FromArgb(255, 34, 139, 34)); // Trung bình
            else
                return new SolidColorBrush(Color.FromArgb(255, 0, 100, 0)); // Đậm
        }
        public void ProcessDateCreatedCount(List<SubmitTestTimeItem> items)
        {
            var tempdateCount = new Dictionary<string, int>();

            foreach (var item in items)
            {
                var date = DateTime.Parse(item.DateCreated).ToString("yyyy-MM-dd");
                if (tempdateCount.ContainsKey(date))
                {
                    tempdateCount[date]++;
                }
                else
                {
                    tempdateCount[date] = 1;
                }
            }
            dateCount = tempdateCount;
        }
        public void PreviousMonth()
        {
            currentDate = currentDate.AddMonths(-1);
            GenerateCalendarDays(currentDate);
        }

        public void NextMonth()
        {
            currentDate = currentDate.AddMonths(1);
            GenerateCalendarDays(currentDate);
        }

        public DateTime DayButtonClick(Button clickedButton)
        {
            // Reset previously selected date button
            if (selectedDateButton != null)
            {
                selectedDateButton.Foreground = new SolidColorBrush(Colors.Black);
            }

            // Highlight the clicked date
            clickedButton.Foreground = new SolidColorBrush(Colors.Red);
            selectedDateButton = clickedButton;

            int day = int.Parse(clickedButton.Content.ToString());
            return new DateTime(currentDate.Year, currentDate.Month, day);
        }
    }
}