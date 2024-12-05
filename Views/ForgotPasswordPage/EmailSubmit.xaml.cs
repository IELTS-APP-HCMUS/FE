using login_full.Context;
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
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Windows.Media.Protection.PlayReady;
using login_full.Views;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Views.ForgotPasswordPage
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class EmailSubmit : Page
	{
		public EmailSubmit()
		{
			this.InitializeComponent();
		}
		private async void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			string email = EmailTextBox.Text;
			if (string.IsNullOrEmpty(email))
			{
				// Hiển thị thông báo lỗi
				//ErrorMessageTextBlock.Text = "Please enter your email.";
			}
			else
			{
				// Xử lý gửi yêu cầu khôi phục mật khẩu
				await NavigateToOTPVerify();
				return;
				try
				{
					string response = await SendEmail(email);
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
			//
		}
		private static async Task NavigateToOTPVerify()
		{
			await App.NavigationService.NavigateToAsync(typeof(OTPVerify));
		}
	}
}
