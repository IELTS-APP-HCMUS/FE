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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;




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

            LoadUserProfile();
			LoadUserTarget();

            calendarManager = new CalendarManager(CalendarGrid, MonthYearDisplay);
            scheduleManager = new ScheduleManager(ScheduleListView);
            calendarManager.GenerateCalendarDays(DateTime.Now);
        }
		// Hàm gọi API để lấy dữ liệu người dùng
		private async void LoadUserProfile()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					// Lấy access token từ GlobalState
					string accessToken = GlobalState.Instance.AccessToken;
					// Thêm access token vào header Authorization
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					// Gửi yêu cầu GET đến API
					HttpResponseMessage response = await client.GetAsync("http://localhost:8080/api/users");

					// Kiểm tra phản hồi từ API
					if (response.IsSuccessStatusCode)
					{
						// Đọc dữ liệu JSON từ phản hồi
						string jsonResponse = await response.Content.ReadAsStringAsync();

						// Parse JSON thành đối tượng UserProfile
						//UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonResponse);
						var userProfile = JObject.Parse(jsonResponse);

						// Cập nhật giao diện với thông tin người dùng
						UserProfile_Name.Text = userProfile["data"]["first_name"].ToString() + " " + userProfile["data"]["last_name"].ToString();
						UserProfile_Email.Text = userProfile["data"]["email"].ToString();
                        UserNameTag.Text = userProfile["data"]["first_name"].ToString() + " " + userProfile["data"]["last_name"].ToString();

						// Ẩn thông báo "Loading..."
						//LoadingText.Visibility = Visibility.Collapsed;
					}
					else
					{
						// Thông báo lỗi nếu không lấy được dữ liệu
						//LoadingText.Text = "Failed to load user information.";
					}
				}
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có ngoại lệ
				//LoadingText.Text = $"Error: {ex.Message}";
			}
		}
		// Hàm gọi API để lấy dữ liệu người dùng
		private async void LoadUserTarget()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					// Lấy access token từ GlobalState
					string accessToken = GlobalState.Instance.AccessToken;
					// Thêm access token vào header Authorization
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					// Gửi yêu cầu GET đến API
					HttpResponseMessage response = await client.GetAsync("http://localhost:8080/api/users/target");

					// Kiểm tra phản hồi từ API
					if (response.IsSuccessStatusCode)
					{
						// Đọc dữ liệu JSON từ phản hồi
						string jsonResponse = await response.Content.ReadAsStringAsync();

						// Parse JSON thành đối tượng UserProfile
						//UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonResponse);
						var userTarget = JObject.Parse(jsonResponse);

						// Cập nhật giao diện với thông tin người dùng

						ReadingTarget.Text = userTarget["data"]["target_reading"].ToString();
						ListeningTarget.Text = userTarget["data"]["target_listening"].ToString();
						WritingTarget.Text = userTarget["data"]["target_writing"].ToString();
						SpeakingTarget.Text = userTarget["data"]["target_speaking"].ToString();


						// Ẩn thông báo "Loading..."
						//LoadingText.Visibility = Visibility.Collapsed;
					}
					else
					{
						// Thông báo lỗi nếu không lấy được dữ liệu
						//LoadingText.Text = "Failed to load user information.";
					}
				}
			}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có ngoại lệ
				//LoadingText.Text = $"Error: {ex.Message}";
			}
		}
		private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
			localSettings.Values.Remove("Username");
			localSettings.Values.Remove("PasswordInBase64");
			localSettings.Values.Remove("EntropyInBase64");


			if (App.IsLoggedInWithGoogle)
			{
				var configuration = new ConfigurationBuilder()
					.SetBasePath(AppContext.BaseDirectory)
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					.Build();

				var googleAuthService = new GoogleAuthService(configuration);
				await googleAuthService.SignOutAsync(); 
			}

			var window = (Application.Current as App)?.MainWindow;

			if (window != null)
			{
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