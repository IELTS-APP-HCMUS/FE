using Google.Apis.Util;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using System;
using System.Threading;
using System.Threading.Tasks;
using Windows.Storage;
using Windows.Graphics;
using Microsoft.UI.Windowing;
using Microsoft.UI.Dispatching;
using login_full.API_Services;
using login_full.Context;
using Microsoft.UI.Xaml.Media;
using System.Linq;
using Microsoft.UI;

namespace login_full
{
	public sealed partial class LoginPage : Page
	{
		private readonly UserAuthenticationService _authService;
		private readonly LoginApiService _loginApiService;

		public LoginPage()
		{
			this.InitializeComponent();
			
			_authService = new UserAuthenticationService();
			_loginApiService = new LoginApiService();

			// Check for saved credentials on page load
			CheckSavedCredentials();
		}

		private async void GoogleSignInButton_Click(object sender, RoutedEventArgs e)
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
					NavigateToHomePage();
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

		private void LoginButton_Click(object sender, RoutedEventArgs e)
		{
			LoginButton.Background = new SolidColorBrush(Colors.White);
			RegisterButton.Background = new SolidColorBrush(Colors.Transparent);
			LoginPanel.Visibility = Visibility.Visible;
			RegisterPanel.Visibility = Visibility.Collapsed;
		}

		private void RegisterButtonToggle_Click(object sender, RoutedEventArgs e)
		{
			LoginButton.Background = new SolidColorBrush(Colors.Transparent);
			RegisterButton.Background = new SolidColorBrush(Colors.White);
			LoginPanel.Visibility = Visibility.Collapsed;
			RegisterPanel.Visibility = Visibility.Visible;
		}

		private async void CheckSavedCredentials()
		{
			if (_authService.HasSavedCredentials())
			{
				NavigateToHomePage();
			}
		}

		private async void Login1Button_Click(object sender, RoutedEventArgs e)
		{
			string username = UsernameTextBox.Text;
			string password = PasswordBox.Password;

			string response = await _loginApiService.LoginAsync(username, password);

			if (response.StartsWith("Error"))
			{
				ErrorMessageTextBlock.Text = "Invalid email or password";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}

			try
			{
				var jsonResponse = JObject.Parse(response);

				if (jsonResponse["code"].ToString() == "200")
				{
					string token = jsonResponse["data"].ToString();
					System.Diagnostics.Debug.WriteLine(token);

					// Lưu access token vào GlobalState

					GlobalState.Instance.AccessToken = token;
					App.IsLoggedInWithGoogle = false;
					if (RememberMeCheckbox.IsChecked == true)
					{
						_authService.SaveCredentials(username, password);
					}
					NavigateToHomePage();
				}
				else
				{
					string errorMessage = jsonResponse["error_detail"].ToString();
					ErrorMessageTextBlock.Text = errorMessage;
					ErrorMessageTextBlock.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				ErrorMessageTextBlock.Text = ex.Message;
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
			}
		}

		private void NavigateToHomePage()
		{
			LoginGrid.Visibility = Visibility.Collapsed;
			Frame.Visibility = Visibility.Visible;
			App.MainFrame.Navigate(typeof(HomePage));
		}

		private async void RegisterButton_Click(object sender, RoutedEventArgs e)
		{
			string email = RegisterEmailTextBox.Text;
			string password = RegisterPasswordBox.Password;
			string confirmPassword = ConfirmPasswordBox.Password;
			string fullName = FullNameTextBox.Text;

			if (password != confirmPassword)
			{
				await ShowErrorDialogAsync("Passwords do not match.");
				return;
			}

			string[] nameParts = fullName.Split(' ');
			string lastName = nameParts.Length > 0 ? nameParts[0] : "";
			string firstName = nameParts.Length > 1 ? string.Join(" ", nameParts.Skip(1)) : "";
			string role = "end_user";

			string response = await _loginApiService.SignupAsync(email, password, firstName, lastName, role);

			if (response.StartsWith("Error") || response.StartsWith("Exception"))
			{
				await ShowErrorDialogAsync(response);
				return;
			}

			try
			{
				var jsonResponse = JObject.Parse(response);

				if (jsonResponse["code"].ToString() == "0")
				{
					await ShowSuccessDialogAsync("User created successfully!");
					LoginPanel.Visibility = Visibility.Visible;
					RegisterPanel.Visibility = Visibility.Collapsed;
					LoginButton.Background = new SolidColorBrush(Colors.Transparent);
					RegisterButton.Background = new SolidColorBrush(Colors.White);
				}
				else
				{
					string errorMessage = jsonResponse["message"]?.ToString() ?? "Signup failed.";
					await ShowErrorDialogAsync($"Signup failed: {errorMessage}");
				}
			}
			catch (Exception ex)
			{
				await ShowErrorDialogAsync($"Unexpected error during signup: {ex.Message}");
			}
		}
	}
}
