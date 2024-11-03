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
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Windows;

using Microsoft.UI.Windowing;
using System.Data;
using System.Text;
using Windows.Networking;
using YamlDotNet.Core.Tokens;
using System.Threading.Tasks;




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

            LoadUserProfile();
			LoadUserTarget();

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
					HttpResponseMessage response = await client.GetAsync("https://ielts-app-api-4.onrender.com/api/users");

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

                        // Lưu user profile vào GlobalState
                        UserProfile user_profile = new UserProfile
                        {
                            Name = userProfile["data"]["first_name"].ToString() + " " + userProfile["data"]["last_name"].ToString(),
                            Email = userProfile["data"]["email"].ToString(),
					    };
                        GlobalState.Instance.UserProfile = user_profile;
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
                    HttpResponseMessage response = await client.GetAsync("https://ielts-app-api-4.onrender.com/api/users/target");

                    // Kiểm tra phản hồi từ API
                    if (response.IsSuccessStatusCode)
                    {
						// Đọc dữ liệu JSON từ phản hồi
						string stringResponse = await response.Content.ReadAsStringAsync();

                        // Parse JSON thành đối tượng UserProfile
                        JObject jsonResponse = JObject.Parse(stringResponse);
                        JObject dataResponse = (JObject)jsonResponse["data"];
                        dataResponse.Remove("id");
                        UserTarget userTarget = dataResponse.ToObject<UserTarget>();

						//var readingTarget = dataResponse["target_reading"];
						//var listeningTarget = dataResponse["target_listening"];
						//var writingTarget = dataResponse["target_writing"];
						//var speakingTarget = dataResponse["target_speaking"];


						ReadingScoreTextBlock.Text = userTarget.TargetReading == -1 ? "-" : userTarget.TargetReading.ToString();
						ListeningScoreTextBlock.Text = userTarget.TargetListening == -1 ? "-" : userTarget.TargetListening.ToString();
						WritingScoreTextBlock.Text = userTarget.TargetWriting == -1 ? "-" : userTarget.TargetWriting.ToString();
						SpeakingScoreTextBlock.Text = userTarget.TargetSpeaking == -1 ? "-" : userTarget.TargetSpeaking.ToString();
						RemainingDaysText.Text = userTarget.TargetStudyDuration.ToString() + " ngày";
						DateOnly dateTime = DateOnly.Parse(userTarget.NextExamDate.Split(" ")[0]);
						int remainingDays = (dateTime.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;
						RemainingDaysText.Text = $"{remainingDays} ngày";
						ExamDateButton.Content = dateTime.ToString();


						double overallTarget = -1;
						if (userTarget.TargetReading != -1 && userTarget.TargetListening != -1 && userTarget.TargetWriting != -1 && userTarget.TargetSpeaking != -1)
						{
							overallTarget = (userTarget.TargetReading + userTarget.TargetListening + userTarget.TargetWriting + userTarget.TargetSpeaking) / 4;
							overallTarget = Math.Round(overallTarget * 2) / 2;
						}
						OverallScoreTextBlock.Text = overallTarget == -1 ? "-" : overallTarget.ToString();

						// Ẩn thông báo "Loading..."
						//LoadingText.Visibility = Visibility.Collapsed;
					}
					else
				    {
						// Thông báo lỗi nếu không lấy được dữ liệu
						//LoadingText.Text = "Failed to load user information.";
						string stringResponse = await response.Content.ReadAsStringAsync();
					}
			}
}
			catch (Exception ex)
			{
				// Xử lý lỗi nếu có ngoại lệ
				//LoadingText.Text = $"Error: {ex.Message}";
			}
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


        //      // click nằm ở popup exit
        private void ExitButton_Click(object sender, RoutedEventArgs e)
        {
            IeltsScorePopup.IsOpen = false;
        }

        //      //click này nằm ở popup save
        private async void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            double readingScore = double.TryParse(ReadingScoreTextBox.Text, out readingScore) ? readingScore : 0;
            double listeningScore = double.TryParse(ListeningScoreTextBox.Text, out listeningScore) ? listeningScore : 0;
            double writingScore = double.TryParse(WritingScoreTextBox.Text, out writingScore) ? writingScore : 0;
            double speakingScore = double.TryParse(SpeakingScoreTextBox.Text, out speakingScore) ? speakingScore : 0;

			var targetRequest = new
			{
				target_reading = readingScore,
				target_listening = listeningScore,
				target_writing = writingScore,
				target_speaking = speakingScore
			};

			string json = JsonConvert.SerializeObject(targetRequest);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

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
					HttpResponseMessage response = await client.PatchAsync("https://ielts-app-api-4.onrender.com/api/users/target", content);

					// Kiểm tra phản hồi từ API
					if (response.IsSuccessStatusCode)
					{
                        LoadUserTarget();

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
			localSettings.Values.Clear();

			GlobalState.Instance.AccessToken = null;
			GlobalState.Instance.UserProfile = null;  // Clear user profile if you store it in GlobalState
			

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


        private async void ExamDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs args)
        {
            if (args.NewDate.HasValue)
            {
                DateTime selectedDate = args.NewDate.Value.Date;
                ExamDateButton.Content = selectedDate.ToString("dd / MM / yyyy");

				var targetRequest = new
				{
					next_exam_date = selectedDate.ToString("yyyy-MM-dd")
				};

				string json = JsonConvert.SerializeObject(targetRequest);
				var content = new StringContent(json, Encoding.UTF8, "application/json");

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
						HttpResponseMessage response = await client.PatchAsync("https://ielts-app-api-4.onrender.com/api/users/target", content);

						// Kiểm tra phản hồi từ API
						if (response.IsSuccessStatusCode)
						{
							//LoadUserTarget();

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
				UpdateRemainingDays(selectedDate);
            }
            else
            {
                ExamDateButton.Content = "- / - / -";
                RemainingDaysText.Text = "- ngày";
            }
        }

		private async Task UpdateExamDateInDatabase(DateTime examDate)
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					
					string accessToken = GlobalState.Instance.AccessToken;
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

					
					string formattedDate = examDate.ToString("yyyy-MM-dd 00:00:00");

					
					var updateData = new
					{
						next_exam_date = formattedDate
					};

					
					string json = JsonConvert.SerializeObject(updateData);
					var content = new StringContent(json, Encoding.UTF8, "application/json");

					
					HttpResponseMessage response = await client.PatchAsync("https://ielts-app-api-4.onrender.com/api/users/target", content);

					if (response.IsSuccessStatusCode)
					{
						
						System.Diagnostics.Debug.WriteLine("Exam date updated successfully.");
					}
					else
					{
						
						System.Diagnostics.Debug.WriteLine("Failed to update exam date.");
					}
				}
			}
			catch (Exception ex)
			{
				
				System.Diagnostics.Debug.WriteLine($"Error updating exam date: {ex.Message}");
			}
		}



		private void UpdateRemainingDays(DateTime examDate)
        {
            int remainingDays = (examDate - DateTime.Today).Days;
            RemainingDaysText.Text = $"{remainingDays} ngày";
        }


        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
            Frame.Navigate(typeof(AboutUsPage));
        }

    }


}