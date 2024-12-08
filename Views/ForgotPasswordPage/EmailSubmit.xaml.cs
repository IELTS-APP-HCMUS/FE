using login_full.Context;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using login_full.API_Services;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using System;
using System.Net.Http;

using System.Text;
using System.Threading.Tasks;

using Google.Apis.Util;
using Microsoft.Extensions.Configuration;
using System.Threading;



// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Views.ForgotPasswordPage
{
	/// <summary>
	/// Trang xử lý việc nhập email để gửi OTP và hỗ trợ các phương thức xác thực.
	/// </summary>
	public sealed partial class EmailSubmit : Page
	{
		private readonly LoginApiService _loginApiService;

		/// <summary>
		/// Khởi tạo lớp EmailSubmit và thiết lập LoginApiService.
		/// </summary>
		public EmailSubmit()
		{
			this.InitializeComponent();
			_loginApiService = new LoginApiService();
		}

		/// <summary>
		/// Gửi yêu cầu OTP đến email của người dùng thông qua API.
		/// </summary>
		/// <param name="email">Địa chỉ email của người dùng.</param>
		/// <returns>Chuỗi JSON phản hồi từ API hoặc thông báo lỗi.</returns>
		private async Task<string> SendOtpToEmail(string email)
		{
			string json = JsonConvert.SerializeObject(new { email });
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				HttpClient client = new HttpClient();
			
				HttpResponseMessage response = await client.PostAsync("https://ielts-app-api-4.onrender.com/api/auth/request-reset-password", content);

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
		/// Xử lý sự kiện khi người dùng nhấn nút Submit, kiểm tra email và gửi yêu cầu OTP.
		/// </summary>
		/// <param name="sender">Nguồn sự kiện.</param>
		/// <param name="e">Thông tin sự kiện.</param>
		private async void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			string email = EmailTextBox.Text;
			if (string.IsNullOrEmpty(email))
			{
				ErrorMessageTextBlock.Text = "Please enter your email.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}
			else
			{
				try
				{
					string response = await SendOtpToEmail(email);
					var jsonResponse = JObject.Parse(response);

					if (jsonResponse["code"].ToString() == "200")
					{
						await NavigateToOTPVerify(email);
					}
					else
					{
						ErrorMessageTextBlock.Text = jsonResponse["message"].ToString();
						ErrorMessageTextBlock.Visibility = Visibility.Visible;
					}
				}
				catch (Exception ex)
				{
					ErrorMessageTextBlock.Text = "An error occurred. Please try again later.";
					Console.WriteLine(ex.Message);
				}
			}
		}
		private static readonly HttpClient client = new HttpClient();

		private async Task<string> SendEmail(string email)
		{
			string json = JsonConvert.SerializeObject(new {email});
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				HttpResponseMessage response = await client.PostAsync("https://ielts-app-api-4.onrender.com/api/users/login", content);

				if (response.IsSuccessStatusCode)
				{
					string responseBody = await response.Content.ReadAsStringAsync();
					return responseBody;
				}
				else
				{
					return $"Error: {response.StatusCode}";
				}
			}
			catch (Exception ex)
			{
				return $"Exception: {ex.Message}";
			}
		}
		/// <summary>
		/// Xử lý sự kiện khi người dùng nhấn nút Google Sign-In, thực hiện xác thực qua OAuth và xử lý phản hồi.
		/// </summary>
		/// <param name="sender">Nguồn sự kiện.</param>
		/// <param name="e">Thông tin sự kiện.</param>
		public async void GoogleSignInButton_Click(object sender, RoutedEventArgs e)
		{
			try
			{
				var configuration = new ConfigurationBuilder()
					.SetBasePath(AppContext.BaseDirectory)
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					.Build();

				var googleAuthService = new GoogleAuthService(configuration);
				var credential = await googleAuthService.AuthenticateAsync(CancellationToken.None);

				if (credential == null)
				{
					await ShowErrorDialogAsync("Authentication failed. No credential returned.");
					return;
				}

				if (credential.Token.IsExpired(SystemClock.Default))
				{
					try
					{
						await credential.RefreshTokenAsync(CancellationToken.None);
					}
					catch (Exception refreshEx)
					{
						await ShowErrorDialogAsync($"Failed to refresh token: {refreshEx.Message}");
						return;
					}
				}

				string idToken = credential.Token.IdToken;

				if (string.IsNullOrEmpty(idToken))
				{
					await ShowErrorDialogAsync("No ID token returned from Google.");
					return;
				}

				var response = await _loginApiService.LoginWithOAuthAsync(idToken);

				if (response.StartsWith("Error"))
				{
					await ShowErrorDialogAsync(response);
					return;
				}

				await HandleLoginResponseAsync(response);
			}
			catch (Google.Apis.Auth.OAuth2.Responses.TokenResponseException trEx)
			{
				await ShowErrorDialogAsync($"Token response error: {trEx.Message}");
			}
			catch (Exception ex)
			{
				await ShowErrorDialogAsync($"Unexpected error: {ex.Message}");
			}
		}

		/// <summary>
		/// Hiển thị hộp thoại thông báo lỗi khi xảy ra sự cố trong quá trình xử lý.
		/// </summary>
		/// <param name="message">Thông báo lỗi cần hiển thị.</param>
		private async Task ShowErrorDialogAsync(string message)
		{
			DispatcherQueue.TryEnqueue(async () =>
			{
				var dialog = new ContentDialog
				{
					Title = "Sign In Error",
					Content = message,
					CloseButtonText = "OK",
					XamlRoot = this.XamlRoot
				};
				await dialog.ShowAsync();
			});
		}
		/// <summary>
		/// Xử lý phản hồi sau khi đăng nhập bằng OAuth Google, kiểm tra trạng thái và điều hướng.
		/// </summary>
		/// <param name="response">Chuỗi JSON phản hồi từ API đăng nhập Google.</param>
		private async Task HandleLoginResponseAsync(string response)
		{
			try
			{
				var jsonResponse = JObject.Parse(response);

				if (jsonResponse["code"].ToString() == "200")
				{
					App.IsLoggedInWithGoogle = true;
					GlobalState.Instance.AccessToken = jsonResponse["data"].ToString();
					await ShowSuccessDialogAsync("Login successful with Google!");
					_ = NavigateToHomePage();
				}
				else
				{
					string errorMessage = jsonResponse["error_detail"].ToString();
					await ShowErrorDialogAsync($"Login failed: {errorMessage}");
				}
			}
			catch (Exception ex)
			{
				await ShowErrorDialogAsync($"Unexpected error during login: {ex.Message}");
			}
		}

		/// <summary>
		/// Hiển thị hộp thoại thông báo thành công khi người dùng thực hiện đăng nhập hoặc hành động thành công.
		/// </summary>
		/// <param name="message">Thông báo cần hiển thị.</param>
		private async Task ShowSuccessDialogAsync(string message)
		{
			DispatcherQueue.TryEnqueue(async () =>
			{
				var dialog = new ContentDialog
				{
					Title = "Sign In Successful",
					Content = message,
					CloseButtonText = "OK",
					XamlRoot = this.XamlRoot
				};
				await dialog.ShowAsync();
			});
		}

		/// <summary>
		/// Điều hướng đến trang xác minh OTP với email đã nhập.
		/// </summary>
		/// <param name="email">Địa chỉ email cần xác minh.</param>
		private async Task NavigateToOTPVerify(string email)
		{
			await App.NavigationService.NavigateToAsync(typeof(OTPVerify), email);
		}
		/// <summary>
		/// Điều hướng đến trang chính sau khi người dùng đăng nhập thành công.
		/// </summary>
		private async Task NavigateToHomePage()
		{
			await App.NavigationService.NavigateToAsync(typeof(HomePage)); // Navigate như thế này đây
		}
	}
}
