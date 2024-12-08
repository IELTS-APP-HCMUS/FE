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
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class PasswordReset : Page
	{
		private string Email { get; set; }
		public PasswordReset()
		{
			this.InitializeComponent();
		}

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

		private bool IsPasswordStrong(string password)
		{
			// At least 8 characters, one uppercase letter, one lowercase letter, one digit, and one special character
			var regex = new Regex(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]{8,}$");
			return regex.IsMatch(password);
		}

		private bool IsEmailValid(string email)
		{
			var regex = new Regex(@"^[^\s@]+@[^\s@]+\.[^\s@]+$");
			return regex.IsMatch(email);
		}

		private async Task<string> ResetPasswordAsync(string email, string newPassword)
		{
			string json = JsonConvert.SerializeObject(new { email, new_password = newPassword });
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				HttpClient client = new HttpClient();
				

				HttpResponseMessage response = await client.PostAsync("https://ielts-app-api-4.onrender.com/api/auth/reset-password", content);

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

	}
}
