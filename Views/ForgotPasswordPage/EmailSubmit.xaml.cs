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
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class EmailSubmit : Page
	{
		private readonly LoginApiService _loginApiService;
		public EmailSubmit()
		{
			this.InitializeComponent();
			_loginApiService = new LoginApiService();
		}
		private async void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			string email = EmailTextBox.Text;
			if (string.IsNullOrEmpty(email))
			{
				// Hiển thị thông báo lỗi
				ErrorMessageTextBlock.Text = "Please enter your email.";
			}
			else
			{
				// Xử lý gửi yêu cầu khôi phục mật khẩu
				try
				{
					// call api
					//string response = await SendEmail(email);
					string response = "{\"code\":\"200\"}";
					var jsonResponse = JObject.Parse(response);
					if (jsonResponse["code"].ToString() != "200")
					{
						await NavigateToOTPVerify();
					}
				}
				catch (Exception ex) {
					//ErrorMessageTextBlock.Text = ex.Message;
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

		private static async Task NavigateToOTPVerify()
		{
			await App.NavigationService.NavigateToAsync(typeof(OTPVerify));
		}

		private async Task NavigateToHomePage()
		{
			await App.NavigationService.NavigateToAsync(typeof(HomePage)); // Navigate như thế này đây
		}
	}
}
