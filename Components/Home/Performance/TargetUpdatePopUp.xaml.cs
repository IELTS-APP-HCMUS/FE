using login_full.Context;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Text;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home.Performance
{
	public sealed partial class TargetUpdatePopUp : UserControl
	{
		public Popup IeltsScorePopupControl => IeltsScorePopup;
		private readonly string _baseUrl;
		public TargetUpdatePopUp()
		{
			this.InitializeComponent();
			var configService = new ConfigService();
			_baseUrl = configService.GetBaseUrl();
		}
		// Sự kiện để thông báo HomePage
		public event EventHandler RequestLoadUserTarget;
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
					string url = $"{_baseUrl}/api/users/target";
					// Lấy access token từ GlobalState  
					string accessToken = GlobalState.Instance.AccessToken;
					// Thêm access token vào header Authorization  
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					// Gửi yêu cầu GET đến API  
					HttpResponseMessage response = await client.PatchAsync(url, content);

					// Kiểm tra phản hồi từ API  
					if (response.IsSuccessStatusCode)
					{
						RequestLoadUserTarget?.Invoke(this, EventArgs.Empty);

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
				Console.WriteLine(ex.ToString());
				// Xử lý lỗi nếu có ngoại lệ  
				//LoadingText.Text = $"Error: {ex.Message}";  
			}

		}
		// click nằm ở popup exit  
		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			IeltsScorePopup.IsOpen = false;
		}
	}
}
