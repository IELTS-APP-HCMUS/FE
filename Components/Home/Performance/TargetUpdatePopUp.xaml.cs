using login_full.Context;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Newtonsoft.Json;
using System;
using System.Net.Http;
using System.Text;
using login_full.API_Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home.Performance
{
	public sealed partial class TargetUpdatePopUp : UserControl
	{
		public Popup IeltsScorePopupControl => IeltsScorePopup;
		private readonly ClientCaller _clientCaller;
        public TargetUpdatePopUp()
		{
			this.InitializeComponent();
            _clientCaller = new ClientCaller();
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
					string url = $"/api/users/target";
					// Gửi yêu cầu GET đến API  
					HttpResponseMessage response = await _clientCaller.PatchAsync(url, content);

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
