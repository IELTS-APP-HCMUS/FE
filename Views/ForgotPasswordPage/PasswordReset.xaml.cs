using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Net.Http;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using Newtonsoft.Json.Linq;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Views.ForgotPasswordPage
{
	/// <summary>
	/// Trang đặt lại mật khẩu, hỗ trợ người dùng nhập mật khẩu mới và xác nhận.
	/// </summary>
	public sealed partial class PasswordReset : Page
	{
		private string Email { get; set; }
		private readonly string _baseUrl;
		/// <summary>
		/// Trang đặt lại mật khẩu, hỗ trợ người dùng nhập mật khẩu mới và xác nhận.
		/// </summary>
		public PasswordReset()
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
				Email = email;
			}
			else
			{
				ErrorMessageTextBlock.Text = "No email provided for password reset.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
			}
		}
		/// <summary>
		/// Kiểm tra xem mật khẩu có đáp ứng yêu cầu về độ mạnh hay không.
		/// </summary>
		/// <param name="password">Mật khẩu cần kiểm tra.</param>
		/// <returns>True nếu mật khẩu mạnh, ngược lại là False.</returns>
		private bool IsPasswordStrong(string password)
		{
			// At least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character
			var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
			return regex.IsMatch(password);
		}
		/// <summary>
		/// Kiểm tra xem email có hợp lệ không.
		/// </summary>
		/// <param name="email">Địa chỉ email cần kiểm tra.</param>
		/// <returns>True nếu email hợp lệ, ngược lại là False.</returns>
		private bool IsEmailValid(string email)
		{
			var regex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
			return regex.IsMatch(email);
		}
		/// <summary>
		/// Gửi yêu cầu đặt lại mật khẩu đến API với email và mật khẩu mới.
		/// </summary>
		/// <param name="email">Địa chỉ email của người dùng.</param>
		/// <param name="newPassword">Mật khẩu mới của người dùng.</param>
		/// <returns>Kết quả phản hồi từ API dưới dạng chuỗi JSON hoặc thông báo lỗi.</returns>
		private async Task<string> ResetPasswordAsync(string email, string newPassword)
		{
			string url = $"{_baseUrl}/api/auth/reset-password";
			string json = JsonConvert.SerializeObject(new { email, new_password = newPassword });
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				HttpClient client = new HttpClient();
				

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
		/// Xử lý sự kiện khi người dùng nhấn nút Submit, kiểm tra mật khẩu mới và gửi yêu cầu đặt lại mật khẩu.
		/// </summary>
		/// <param name="sender">Nguồn sự kiện.</param>
		/// <param name="e">Thông tin sự kiện.</param>
		private async void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			string newPassword = NewPassword.Password;
			string confirmPassword = ConfirmPassword.Password;

			if (string.IsNullOrEmpty(newPassword) || string.IsNullOrEmpty(confirmPassword))
			{
				ErrorMessageTextBlock.Text = "Both password fields are required.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}

			if (newPassword != confirmPassword)
			{
				ErrorMessageTextBlock.Text = "Passwords do not match.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}

			if (!IsPasswordStrong(newPassword))
			{
				ErrorMessageTextBlock.Text = "Password must be at least 8 characters long, include uppercase, lowercase, digits, and special characters.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}

			if (!IsEmailValid(Email))
			{
				ErrorMessageTextBlock.Text = "The provided email is invalid.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}

			try
			{
				string response = await ResetPasswordAsync(Email, newPassword);
				var jsonResponse = JObject.Parse(response);

				if (jsonResponse["code"]?.ToString() == "200")
				{
					await ShowSuccessDialogAsync("Password reset successful! Redirecting to login page.");
					await App.NavigationService.NavigateToAsync(typeof(LoginPage));
				}
				else
				{
					ErrorMessageTextBlock.Text = jsonResponse["message"]?.ToString() ?? "Password reset failed.";
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
		/// Hiển thị hộp thoại thông báo thành công khi đặt lại mật khẩu thành công.
		/// </summary>
		/// <param name="message">Thông báo cần hiển thị.</param>

		private async Task ShowSuccessDialogAsync(string message)
		{
			var mainWindow = App.MainWindow;
			if (mainWindow == null) return;

			ContentDialog successDialog = new ContentDialog
			{
				Title = "Password Reset",
				Content = message,
				CloseButtonText = "OK",
				XamlRoot = this.XamlRoot
			};

			await successDialog.ShowAsync();
		}

		private async void BackToLoginButton_Click(object sender, RoutedEventArgs e)
		{
			await App.NavigationService.NavigateToAsync(typeof(LoginPage)); 
		}

	}
}
