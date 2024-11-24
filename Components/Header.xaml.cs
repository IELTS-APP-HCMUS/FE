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
		
		public Header()
		{
			this.InitializeComponent();

			GlobalState.Instance.PropertyChanged += GlobalState_PropertyChanged;
			UserProfile userProfile = GlobalState.Instance.UserProfile;
			if (userProfile != null)
			{
				UserProfile_Name.Text = userProfile.Name;
				UserProfile_Email.Text = userProfile.Email;
				UserNameTag.Text = userProfile.Name;
			}
			
		}
		
		private void GlobalState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(GlobalState.UserProfile))
			{
				Console.WriteLine("GlobalState_PropertyChanged");
				UserProfile userProfile = GlobalState.Instance.UserProfile;
				if (userProfile != null)
				{
					UserProfile_Name.Text = userProfile.Name;
					UserProfile_Email.Text = userProfile.Email;
					UserNameTag.Text = userProfile.Name;
				}
			}
		}

		private void NavigateToPage(Type pageType)
		{
			if (App.MainWindow is MainWindow mainWindow && mainWindow.MainFrame != null)
			{
				if (mainWindow.MainFrame.Content?.GetType() != pageType)
				{
					mainWindow.MainFrame.Navigate(pageType);
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("MainFrame is not initialized or accessible.");
			}
		}
		private void Home_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(typeof(HomePage));
		}
		private void AboutUs_Click(object sender, RoutedEventArgs e)
		{
			NavigateToPage(typeof(AboutUsPage));
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
			GlobalState.Instance.UserProfile = null;  


			if (App.IsLoggedInWithGoogle)
			{
				var configuration = new ConfigurationBuilder()
					.SetBasePath(AppContext.BaseDirectory)
					.AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
					.Build();

				var googleAuthService = new GoogleAuthService(configuration);
				await googleAuthService.SignOutAsync();
			}

			var window = App.MainWindow;

			if (window != null)
			{
				var newMainWindow = new MainWindow();
				newMainWindow.Activate();
				window.Close();
			}
		}
	}
}
