using System;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using login_full.Models;
using login_full.Context;
using System.ComponentModel;




namespace login_full
{
	/// <summary>
	/// Trang chính của ứng dụng, hiển thị thông tin hồ sơ người dùng và mục tiêu học tập.
	/// </summary>
	public sealed partial class HomePage : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
  
		public UserProfile Profile { get; } = new UserProfile();
		public UserTarget Target { get; } = new UserTarget();
		/// <summary>
		/// Khởi tạo lớp `HomePage`, thiết lập giao diện người dùng và đăng ký các sự kiện cần thiết.
		/// </summary>
		public HomePage()
        {
            this.InitializeComponent();
			this.DataContext = this;
			LoadUserProfile();
			LoadUserTarget();
			PerformanceComponent.TargetComponentControl.TargetUpdatePopUpCompControl.RequestLoadUserTarget += TargetComponent_RequestLoadUserTarget;
		}
		/// <summary>
		/// Xử lý sự kiện khi chế độ xem của ScrollViewer thay đổi.
		/// </summary>
		/// <param name="sender">Nguồn sự kiện.</param>
		/// <param name="e">Thông tin sự kiện.</param>
		private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            
        }

		/// <summary>
		/// Tải thông tin hồ sơ người dùng từ API và cập nhật vào trạng thái toàn cục.
		/// </summary>
		private async void LoadUserProfile()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					string accessToken = GlobalState.Instance.AccessToken;
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					HttpResponseMessage response = await client.GetAsync("https://ielts-app-api-4.onrender.com/api/users");

					if (response.IsSuccessStatusCode)
					{
						
						string stringResponse = await response.Content.ReadAsStringAsync();

						var jsonResponse = JObject.Parse(stringResponse);

                        UserProfile userProfile = new()
						{
                            Name = jsonResponse["data"]["first_name"].ToString() + " " + jsonResponse["data"]["last_name"].ToString(),
                            Email = jsonResponse["data"]["email"].ToString(),
					    };
                        GlobalState.Instance.UserProfile = userProfile;
						Profile.Name = userProfile.Name;
						Profile.Email = userProfile.Email;
					}
					else
					{
						
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error loading user profile: {ex.Message}");
			}
		}

		/// <summary>
		/// Xử lý sự kiện yêu cầu tải lại thông tin mục tiêu học tập từ giao diện người dùng.
		/// </summary>
		public async void LoadUserTarget()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					string accessToken = GlobalState.Instance.AccessToken;
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					HttpResponseMessage response = await client.GetAsync("https://ielts-app-api-4.onrender.com/api/users/target");

					if (response.IsSuccessStatusCode)
					{
						string stringResponse = await response.Content.ReadAsStringAsync();
						System.Diagnostics.Debug.WriteLine(stringResponse);

						JObject jsonResponse = JObject.Parse(stringResponse);
						JObject dataResponse = (JObject)jsonResponse["data"];
						dataResponse.Remove("id");
						UserTarget userTarget = dataResponse.ToObject<UserTarget>();

						DateTime dateTime = DateTime.ParseExact(userTarget.NextExamDate, "MM/dd/yyyy HH:mm:ss", System.Globalization.CultureInfo.InvariantCulture);

						// Check if the date is in the past
						if (dateTime < DateTime.Now)
						{
							dateTime = DateTime.Now; // Use today's date if it's in the past
						}

						// Format the date as dd/MM/yyyy
						string formattedDate = dateTime.ToString("dd/MM/yyyy");

						// Remaning days to the exam
						int remainingDays = (dateTime - DateTime.Now).Days;


						Target.TargetListening = userTarget.TargetListening == -1 ? 0 : userTarget.TargetListening;
						Target.TargetReading = userTarget.TargetReading == -1 ? 0 : userTarget.TargetReading;
						Target.TargetSpeaking = userTarget.TargetSpeaking == -1 ? 0 : userTarget.TargetSpeaking;
						Target.TargetWriting = userTarget.TargetWriting == -1 ? 0 : userTarget.TargetWriting;
						Target.TargetStudyDuration = remainingDays >= 0 ? remainingDays : 0;
						Target.NextExamDate = formattedDate;

					}
					else
					{
						string stringResponse = await response.Content.ReadAsStringAsync();
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error loading user target: {ex.Message}");
			}
			
		}

		/// <summary>
		/// Xử lý sự kiện yêu cầu tải lại thông tin mục tiêu học tập từ giao diện người dùng.
		/// </summary>
		/// <param name="sender">Nguồn sự kiện.</param>
		/// <param name="e">Thông tin sự kiện.</param>
		private void TargetComponent_RequestLoadUserTarget(object sender, EventArgs e)
		{
			LoadUserTarget();
		}

		/// <summary>
		/// Xử lý sự kiện khi trạng thái toàn cục (`GlobalState`) thay đổi.
		/// </summary>
		/// <param name="sender">Nguồn sự kiện.</param>
		/// <param name="e">Thông tin sự kiện.</param>
		private void GlobalState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(GlobalState.UserProfile))
			{
				Console.WriteLine("GlobalState_PropertyChanged");
				UserTarget userTarget = GlobalState.Instance.UserTarget;
				if (userTarget != null)
				{
					Target.TargetListening = userTarget.TargetListening;
					Target.TargetReading = userTarget.TargetReading;
					Target.TargetSpeaking = userTarget.TargetSpeaking;
					Target.TargetWriting = userTarget.TargetWriting;
					Target.NextExamDate = userTarget.NextExamDate;
				}
			}
		}

		
	}
}