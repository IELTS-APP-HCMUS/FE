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
			string email = OTPTextBox.Text;
			if (string.IsNullOrEmpty(email))
			{
				// Hiển thị thông báo lỗi
				//ErrorMessageTextBlock.Text = "Please enter your email.";
			}
			else
			{
				// Xử lý gửi yêu cầu khôi phục mật khẩu
			}
			await NavigateToPasswordReset();
		}
		private static async Task NavigateToPasswordReset()
		{
			await App.NavigationService.NavigateToAsync(typeof(PasswordReset));
		}
	}
}
