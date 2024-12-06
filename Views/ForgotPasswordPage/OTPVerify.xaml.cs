using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Views.ForgotPasswordPage
{
	/// <summary>
	/// An empty page that can be used on its own or navigated to within a Frame.
	/// </summary>
	public sealed partial class OTPVerify : Page
	{
		public OTPVerify()
		{
			this.InitializeComponent();
		}
		private async void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			string otp = OTPTextBox.Text;
			if (string.IsNullOrEmpty(otp))
			{
				// Hiển thị thông báo lỗi
				ErrorMessageTextBlock.Text = "Please enter your otp.";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}
			else
			{
				try
				{
					// call api
					//string response = await SendEmail(email);
					string response = "{\"code\":\"200\", \"otp\":123456}";
					var jsonResponse = JObject.Parse(response);
					if (jsonResponse["code"].ToString() == "200")
					{
						try
						{
							if(otp == jsonResponse["otp"].ToString())
							{
								await NavigateToPasswordReset();
							}
							else
							{
								ErrorMessageTextBlock.Text = "OTP is incorrect.";
								ErrorMessageTextBlock.Visibility = Visibility.Visible;
								return;
							}
						}
						catch (Exception ex)
						{
							ErrorMessageTextBlock.Text = ex.Message;
							Console.WriteLine(ex.Message);
						}
					}
				}
				catch (Exception ex)
				{
					//ErrorMessageTextBlock.Text = ex.Message;
					Console.WriteLine(ex.Message);
				}
			}
			await NavigateToPasswordReset();
		}
		private static async Task NavigateToPasswordReset()
		{
			await App.NavigationService.NavigateToAsync(typeof(PasswordReset));
		}
	}
}
