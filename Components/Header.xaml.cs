using login_full.Context;
using Microsoft.Extensions.Configuration;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using login_full.Models;
using System.ComponentModel;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components
{
	public sealed partial class Header : UserControl
	{
		private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
		//public UserProfile ViewModel
		//{
		//	get; set;
		//}
		//public GlobalState ViewModel { get; } = GlobalState.Instance;
		//public UserProfile UserProfile
		//{
		//	//get { return GlobalState.Instance.UserProfile; }
		//	get; set;
		//}
		public Header()
		{
			this.InitializeComponent();

			// Subscribe to changes in the GlobalState instance
			GlobalState.Instance.PropertyChanged += GlobalState_PropertyChanged;
			//this.DataContext = ViewModel;
			//ViewModel = GlobalState.Instance.UserProfile;
			UserProfile userProfile = GlobalState.Instance.UserProfile;
			if (userProfile != null)
			{
				// Cập nhật giao diện với thông tin người dùng
				UserProfile_Name.Text = userProfile.Name;
				UserProfile_Email.Text = userProfile.Email;
				UserNameTag.Text = userProfile.Name;
			}
			//UserProfile = new UserProfile()
			//{
			//	Name = "Tuan Nhat",
			//	Email = "tn1@gmail.com"
			//};
		}
		//private void Page_Loaded(object sender, RoutedEventArgs e)
		//{

		//}
		private void GlobalState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(GlobalState.UserProfile))
			{
				// Notify the UI that the UserProfile has changed
				//this.DataContext = null;
				//this.DataContext = GlobalState.Instance.UserProfile; // This will refresh the XAML bindings
				Console.WriteLine("GlobalState_PropertyChanged");
				UserProfile userProfile = GlobalState.Instance.UserProfile;
				if (userProfile != null)
				{
					// Cập nhật giao diện với thông tin người dùng
					UserProfile_Name.Text = userProfile.Name;
					UserProfile_Email.Text = userProfile.Email;
					UserNameTag.Text = userProfile.Name;
				}
			}
		}
		private void Home_Click(object sender, RoutedEventArgs e)
		{
			var frame = Window.Current.Content as Frame;
			frame?.Navigate(typeof(HomePage));
		}
		private void AboutUs_Click(object sender, RoutedEventArgs e)
		{
			var frame = Window.Current.Content as Frame;
			frame?.Navigate(typeof(AboutUsPage));
		}
		private void UserProfileButton_Click(object sender, RoutedEventArgs e)
		{
			var flyout = (sender as Button)?.Flyout;
			flyout?.ShowAt(sender as FrameworkElement);
		}
		private async void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			localSettings.Values.Remove("Username");
			localSettings.Values.Remove("PasswordInBase64");
			localSettings.Values.Remove("EntropyInBase64");
			localSettings.Values.Clear();

			GlobalState.Instance.AccessToken = null;
			GlobalState.Instance.UserProfile = null;  // Clear user profile if you store it in GlobalState


			if (App.IsLoggedInWithGoogle)
			{
				var configuration = new ConfigurationBuilder()
					.SetBasePath(AppContext.BaseDirectory)
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					.Build();

				var googleAuthService = new GoogleAuthService(configuration);
				await googleAuthService.SignOutAsync();
			}

			var window = (Application.Current as App)?.MainWindow;

			if (window != null)
			{
				var newMainWindow = new MainWindow();
				newMainWindow.Activate();
				window.Close();
			}
		}
	}
}
