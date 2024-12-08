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
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class OTPVerify : Page
	{
		private string Email { get; set; }
		public OTPVerify()
		{
			this.InitializeComponent();
		}

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

		private async Task<string> VerifyOtpAsync(string email, string otp)
		{
			string json = JsonConvert.SerializeObject(new { email, otp });
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				using HttpClient client = new HttpClient();
			
				HttpResponseMessage response = await client.PostAsync("https://ielts-app-api-4.onrender.com/api/auth/validate-otp", content);

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
		private async Task NavigateToPasswordReset(string email)
		{
			await App.NavigationService.NavigateToAsync(typeof(PasswordReset), email);
		}
	}
}
