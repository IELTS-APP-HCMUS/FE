using login_full.Context;
using login_full.Models;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using login_full.API_Services;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home.Performance
{
	/// <summary>
	/// Component hiển thị thông tin về ngày thi và thời gian còn lại
	/// </summary>
	/// <remarks>
	/// Chức năng:
	/// - Hiển thị ngày thi dự kiến
	/// - Tính và hiển thị số ngày còn lại đến ngày thi
	/// - Cho phép cập nhật ngày thi
	/// </remarks>
	public sealed partial class ExamRemain : UserControl
	{
		private readonly ClientCaller _clientCaller;
        public ExamRemain()
		{
			this.InitializeComponent();
            _clientCaller = new ClientCaller();
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
						string url = $"/api/users/target";
						// Gửi yêu cầu GET đến API
						HttpResponseMessage response = await _clientCaller.PatchAsync(url, content);

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

		/// <summary>
		/// Cập nhật hiển thị số ngày còn lại
		/// </summary>
		/// <param name="examDate">Ngày thi đã chọn</param>
		private void UpdateRemainingDays(DateTime examDate)
		{
			int remainingDays = (examDate - DateTime.Today).Days;
			RemainingDaysText.Text = $"{remainingDays} ngày";
		}
	}
}
