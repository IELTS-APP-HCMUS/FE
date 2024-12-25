using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Views.ForgotPasswordPage
{
	/// <summary>
	/// Trang xác minh OTP được sử dụng để người dùng nhập mã OTP và xác nhận.
	/// </summary>

	public sealed partial class OTPVerify : Page
	{
		private string Email { get; set; }
		private readonly string _baseUrl;
		/// <summary>
		/// Khởi tạo lớp `OTPVerify` và thiết lập giao diện người dùng.
		/// </summary>
		public OTPVerify()
		{
			this.InitializeComponent();
			var configService = new ConfigService();
			_baseUrl = configService.GetBaseUrl();
		}
		/// <summary>
		/// Được gọi khi điều hướng đến trang này, nhận email từ tham số điều hướng.
		/// </summary>
		/// <param name="e">Đối tượng sự kiện điều hướng chứa tham số truyền vào.</param>
		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter is string email)
			{
				Email = email; // Store the email parameter
			}
			else
			{
				ErrorMessageTextBlock.Text = "No email was provided for OTP verification.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
			}
		}
		/// <summary>
		/// Gửi yêu cầu xác minh OTP đến API với email và mã OTP.
		/// </summary>
		/// <param name="email">Địa chỉ email của người dùng.</param>
		/// <param name="otp">Mã OTP được người dùng nhập.</param>
		/// <returns>Kết quả phản hồi từ API dưới dạng chuỗi JSON hoặc thông báo lỗi.</returns>

		private async Task<string> VerifyOtpAsync(string email, string otp)
		{
			string url = $"{_baseUrl}/api/auth/validate-otp";
			string json = JsonConvert.SerializeObject(new { email, otp });
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				using HttpClient client = new HttpClient();
			
				HttpResponseMessage response = await client.PostAsync(url, content);

				if (response.IsSuccessStatusCode)
				{
					return await response.Content.ReadAsStringAsync();
				}
				else
				{
					return $"Error: {response.StatusCode} - {response.ReasonPhrase}";
				}
			}
			catch (Exception ex)
			{
				return $"Exception: {ex.Message}";
			}
		}
		/// <summary>
		/// Xử lý sự kiện khi người dùng nhấn nút Submit, kiểm tra mã OTP và gửi yêu cầu xác minh.
		/// </summary>
		/// <param name="sender">Nguồn sự kiện.</param>
		/// <param name="e">Thông tin sự kiện.</param>

		private async void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			string otp = OTPTextBox.Text;

			if (string.IsNullOrEmpty(otp))
			{
				ErrorMessageTextBlock.Text = "Please enter the OTP.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}

			if (string.IsNullOrEmpty(Email))
			{
				ErrorMessageTextBlock.Text = "Email is missing. Please try again.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}

			try
			{
				string response = await VerifyOtpAsync(Email, otp);
				var jsonResponse = JObject.Parse(response);

				if (jsonResponse["code"]?.ToString() == "200")
				{
					await NavigateToPasswordReset(Email);
				}
				else
				{
					ErrorMessageTextBlock.Text = jsonResponse["message"]?.ToString() ?? "Verification failed.";
					ErrorMessageTextBlock.Visibility = Visibility.Visible;
				}
			}
			catch (JsonException)
			{
				ErrorMessageTextBlock.Text = "Unexpected response from the server.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
			}
			catch (Exception ex)
			{
				ErrorMessageTextBlock.Text = "An error occurred. Please try again later.";
				Console.WriteLine($"Error: {ex.Message}");
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
			}
		}
		/// <summary>
		/// Điều hướng đến trang đặt lại mật khẩu với email đã xác minh.
		/// </summary>
		/// <param name="email">Địa chỉ email của người dùng đã được xác minh.</param>

		private async Task NavigateToPasswordReset(string email)
		{
			await App.NavigationService.NavigateToAsync(typeof(PasswordReset), email);
		}
	}
}
