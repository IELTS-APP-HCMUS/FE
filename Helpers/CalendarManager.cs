﻿using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI;
using System;
using Microsoft.UI.Xaml;

namespace login_full.Helpers
{
    public class CalendarManager
    {
        private readonly Grid CalendarGrid;
        private readonly TextBlock MonthYearDisplay;
        private DateTime currentDate;
        private Button selectedDateButton;

        public CalendarManager(Grid calendarGrid, TextBlock monthYearDisplay)
        {
            CalendarGrid = calendarGrid;
            MonthYearDisplay = monthYearDisplay;
            currentDate = DateTime.Now;

            // Create the CalendarDayButtonStyle programmatically
            var calendarDayButtonStyle = new Style(typeof(Button));
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
                    VerticalAlignment = VerticalAlignment.Center
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
                Button dayButton = new Button
                {
                    Content = day.ToString(),
                    Style = (Style)Application.Current.Resources["CalendarDayButtonStyle"]
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