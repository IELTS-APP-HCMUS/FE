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
	public sealed partial class PasswordReset : Page
	{
		public PasswordReset()
		{
			this.InitializeComponent();
		}
		private async void SubmitButton_Click(object sender, RoutedEventArgs e)
		{
			// check same password
			if (NewPassword	.Password != ConfirmPassword.Password)
			{
				ErrorMessageTextBlock.Text = "Mật khẩu không khớp";
				ErrorMessageTextBlock.Visibility = Visibility.Visible;
				return;
			}

			var mainWindow = App.MainWindow;
			if (mainWindow == null) return;

			ContentDialog submitDialog = new()
			{
				Title = "Đổi mật khẩu",
				Content = "Bạn đã khởi tạo mật khẩu thành công, quay về trang đăng nhập",
				PrimaryButtonText = "Đã hiểu",
				XamlRoot = mainWindow.Content.XamlRoot
			};

			var result = await submitDialog.ShowAsync();

			if (result == ContentDialogResult.Primary)
			{
				await App.NavigationService.NavigateToAsync(typeof(LoginPage));
			}
			}
	}
}
