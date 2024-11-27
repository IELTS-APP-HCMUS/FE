using login_full.Context;
using login_full.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
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
	public sealed partial class ExamRemain : UserControl
	{
		public ExamRemain()
		{
			this.InitializeComponent();
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
							// Đọc dữ liệu JSON từ phản hồi
							string stringResponse = await response.Content.ReadAsStringAsync();

							// Parse JSON thành đối tượng UserTarget
							JObject jsonResponse = JObject.Parse(stringResponse);
							JObject dataResponse = (JObject)jsonResponse["data"];
							dataResponse.Remove("id");
							UserTarget userTarget = dataResponse.ToObject<UserTarget>();
							GlobalState.Instance.UserTarget = userTarget;

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
		private void UpdateRemainingDays(DateTime examDate)
		{
			int remainingDays = (examDate - DateTime.Today).Days;
			RemainingDaysText.Text = $"{remainingDays} ngày";
		}
	}
}
