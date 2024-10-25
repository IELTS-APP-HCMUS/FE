using Google.Apis.Util;
using login_full;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.UI;
using Windows.Storage;
using System.Security.Cryptography;
using System.Text;
using Windows.Services.Maps;
using Microsoft.UI.Dispatching;
using login_full.API_Services;
using Newtonsoft.Json.Linq;



namespace login_full
{

    public sealed partial class MainWindow : Window
    {
        private readonly UserAuthenticationService _authService;
		private readonly LoginApiService _loginApiService;
		public MainWindow()
        {
            this.InitializeComponent();
            (Application.Current as App).MainWindow = this;
            _authService = new UserAuthenticationService();
			_loginApiService = new LoginApiService();
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
				await googleAuthService.SignOutAsync();
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
					//await ShowErrorDialogAsync("Login with Google failed.");
					await ShowErrorDialogAsync(response);
					return;
				}

				try
				{
					var jsonResponse = JObject.Parse(response);

					if (jsonResponse["code"].ToString() == "200")
					{
						string token = jsonResponse["data"].ToString();
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
				ContentDialog dialog = new ContentDialog
				{
					Title = "Sign In Error",
					Content = message,
					CloseButtonText = "OK",
					XamlRoot = this.Content.XamlRoot
				};
				_ = await dialog.ShowAsync();
			});
			
		}

        private async Task ShowSuccessDialogAsync(string message)
        {
			DispatcherQueue.TryEnqueue(async () =>
			{
				ContentDialog dialog = new ContentDialog
				{
					Title = "Sign In Successfull",
					Content = message,
					CloseButtonText = "OK",
					XamlRoot = this.Content.XamlRoot
				};
				_ = await dialog.ShowAsync();
			});
		}

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.Background = new SolidColorBrush(Colors.White);
            RegisterButton.Background = new SolidColorBrush(Colors.Transparent);
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Collapsed;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.Background = new SolidColorBrush(Colors.Transparent);
            RegisterButton.Background = new SolidColorBrush(Colors.White);
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
        }

        private void CheckSavedCredentials()
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
					if (RememberMeCheckbox.IsChecked == true)
					{
						_authService.SaveCredentials(username, password);
					}
					NavigateToHomePage();
				}
				else
				{
					// Handle login failure
					string errorMessage = jsonResponse["error_detail"].ToString();
					ErrorMessageTextBlock.Text = errorMessage;
					ErrorMessageTextBlock.Visibility = Visibility.Visible;
				}
			}
			catch (Exception ex)
			{
				// Handle any JSON parsing or unexpected exceptions
				ErrorMessageTextBlock.Text = ex.Message;
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
			}
		}

        private void NavigateToHomePage()
        {

            LoginGrid.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(typeof(HomePage));
        }
	}

}

