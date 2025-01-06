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
using System.Threading.Tasks;
using login_full.Views;


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

		private async Task NavigateToPage(Type pageType)
		{
			if (App.NavigationService != null)
			{
				try
				{
					System.Diagnostics.Debug.WriteLine($"Attempting to navigate to {pageType.Name}");
					await App.NavigationService.NavigateToAsync(pageType);
					System.Diagnostics.Debug.WriteLine($"Current content: {App.MainFrame.Content?.GetType().Name}");
				}
				catch (Exception ex)
				{
					System.Diagnostics.Debug.WriteLine($"Navigation failed: {ex.Message}");
				}
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("NavigationService is not initialized.");
			}
		}
		private async void Home_Click(object sender, RoutedEventArgs e)
		{
			await NavigateToPage(typeof(HomePage));
		}

        private async void Vocal_Click(object sender, RoutedEventArgs e)
        {
            await NavigateToPage(typeof(VocabularyPage));
        }


        private async void AboutUs_Click(object sender, RoutedEventArgs e)
		{
			await NavigateToPage(typeof(AboutUsPage));
		}
		private void UserProfileButton_Click(object sender, RoutedEventArgs e)
		{
			var flyout = (sender as Button)?.Flyout;
			flyout?.ShowAt(sender as FrameworkElement);
		}

		private async void Reading_Click(object sender, RoutedEventArgs e)
		{
			await NavigateToPage(typeof(login_full.Views.reading_Item_UI));
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

			//Return to LoginPage
			await NavigateToPage(typeof(LoginPage));

		}

	}
}
