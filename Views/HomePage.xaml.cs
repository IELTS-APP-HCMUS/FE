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
using login_full.Models;
using login_full.Context;
using login_full.Components.Home.Performance;
using System.ComponentModel;




namespace login_full
{
    public sealed partial class HomePage : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        // size of home page
        // size of the window
		public UserProfile Profile { get; } = new UserProfile();
		public UserTarget Target { get; } = new UserTarget();

		public HomePage()
        {
            this.InitializeComponent();
			this.DataContext = this;
			LoadUserProfile();
			LoadUserTarget();
			// Gắn sự kiện RequestLoadUserTarget từ Target component
			PerformanceComponent.TargetComponentControl.TargetUpdatePopUpCompControl.RequestLoadUserTarget += TargetComponent_RequestLoadUserTarget;
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

		//private void UserHeaderButton_Click(object sender, RoutedEventArgs e)
  //      {
  //          // VisualStateManager.GoToState(this, "ExpandedHeader", true);
  //          //var flyout = (sender as Button)?.Flyout;
  //          //if (flyout != null)
  //          //{
  //          //    flyout.ShowAt(sender as FrameworkElement);
  //          //}

  //      }
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
						string stringResponse = await response.Content.ReadAsStringAsync();

						// Parse JSON thành đối tượng UserProfile
						//UserProfile userProfile = JsonConvert.DeserializeObject<UserProfile>(jsonResponse);
						var jsonResponse = JObject.Parse(stringResponse);

						// Cập nhật giao diện với thông tin người dùng
						//UserProfile_Name.Text = userProfile["data"]["first_name"].ToString() + " " + userProfile["data"]["last_name"].ToString();
						//UserProfile_Email.Text = userProfile["data"]["email"].ToString();
      //                  UserNameTag.Text = userProfile["data"]["first_name"].ToString() + " " + userProfile["data"]["last_name"].ToString();

                        // Lưu user profile vào GlobalState
                        UserProfile userProfile = new()
						{
                            Name = jsonResponse["data"]["first_name"].ToString() + " " + jsonResponse["data"]["last_name"].ToString(),
                            Email = jsonResponse["data"]["email"].ToString(),
					    };
                        GlobalState.Instance.UserProfile = userProfile;
						Profile.Name = userProfile.Name;
						Profile.Email = userProfile.Email;
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
		public async void LoadUserTarget()
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

						DateOnly dateTime = DateOnly.Parse(userTarget.NextExamDate.Split(" ")[0]);
						int remainingDays = (dateTime.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;

						//ReadingScoreTextBlock.Text = userTarget.TargetReading == -1 ? "-" : userTarget.TargetReading.ToString();
						//ListeningScoreTextBlock.Text = userTarget.TargetListening == -1 ? "-" : userTarget.TargetListening.ToString();
						//WritingScoreTextBlock.Text = userTarget.TargetWriting == -1 ? "-" : userTarget.TargetWriting.ToString();
						//SpeakingScoreTextBlock.Text = userTarget.TargetSpeaking == -1 ? "-" : userTarget.TargetSpeaking.ToString();

						//RemainingDaysText.Text = userTarget.TargetStudyDuration.ToString() + " ngày";
						//RemainingDaysText.Text = $"{remainingDays} ngày";
						//ExamDateButton.Content = dateTime.ToString();

						Target.TargetListening = userTarget.TargetListening;
						Target.TargetReading = userTarget.TargetReading;
						Target.TargetSpeaking = userTarget.TargetSpeaking;
						Target.TargetWriting = userTarget.TargetWriting;
						Target.TargetStudyDuration = remainingDays;
						Target.NextExamDate = userTarget.NextExamDate;

						//double overallTarget = -1;
						//if (userTarget.TargetReading != -1 && userTarget.TargetListening != -1 && userTarget.TargetWriting != -1 && userTarget.TargetSpeaking != -1)
						//{
						//	overallTarget = (userTarget.TargetReading + userTarget.TargetListening + userTarget.TargetWriting + userTarget.TargetSpeaking) / 4;
						//	overallTarget = Math.Round(overallTarget * 2) / 2;
						//}
						//OverallScoreTextBlock.Text = overallTarget == -1 ? "-" : overallTarget.ToString();

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
			// click này ở 5 button điểm
		}
		// Xử lý khi sự kiện được gọi
		private void TargetComponent_RequestLoadUserTarget(object sender, EventArgs e)
		{
			LoadUserTarget();
		}

		private void GlobalState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(GlobalState.UserProfile))
			{
				// Notify the UI that the UserProfile has changed
				//this.DataContext = null;
				//this.DataContext = GlobalState.Instance.UserProfile; // This will refresh the XAML bindings
				Console.WriteLine("GlobalState_PropertyChanged");
				UserTarget userTarget = GlobalState.Instance.UserTarget;
				if (userTarget != null)
				{
					// Cập nhật giao diện với thông tin người dùng
					Target.TargetListening = userTarget.TargetListening;
					Target.TargetReading = userTarget.TargetReading;
					Target.TargetSpeaking = userTarget.TargetSpeaking;
					Target.TargetWriting = userTarget.TargetWriting;
					Target.NextExamDate = userTarget.NextExamDate;
				}
			}
		}

		//private void DayButton_Click(object sender, RoutedEventArgs e)
		//{
		//    Button clickedButton = (Button)sender;
		//    DateTime selectedDate = calendarManager.DayButtonClick(clickedButton);
		//    scheduleManager.UpdateSchedule(selectedDate);
		//}




		//private async Task UpdateExamDateInDatabase(DateTime examDate)
		//{
		//	try
		//	{
		//		using (HttpClient client = new HttpClient())
		//		{

		//			string accessToken = GlobalState.Instance.AccessToken;
		//			client.DefaultRequestHeaders.Authorization =
		//				new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);


		//			string formattedDate = examDate.ToString("yyyy-MM-dd 00:00:00");


		//			var updateData = new
		//			{
		//				next_exam_date = formattedDate
		//			};


		//			string json = JsonConvert.SerializeObject(updateData);
		//			var content = new StringContent(json, Encoding.UTF8, "application/json");


		//			HttpResponseMessage response = await client.PatchAsync("https://ielts-app-api-4.onrender.com/api/users/target", content);

		//			if (response.IsSuccessStatusCode)
		//			{

		//				System.Diagnostics.Debug.WriteLine("Exam date updated successfully.");
		//			}
		//			else
		//			{

		//				System.Diagnostics.Debug.WriteLine("Failed to update exam date.");
		//			}
		//		}
		//	}
		//	catch (Exception ex)
		//	{

		//		System.Diagnostics.Debug.WriteLine($"Error updating exam date: {ex.Message}");
		//	}
		//}

		//      private void AboutUs_Click(object sender, RoutedEventArgs e)
		//      {
		//          Frame.Navigate(typeof(AboutUsPage));
		//      }
	}
}