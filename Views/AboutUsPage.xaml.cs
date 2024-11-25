using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Windows.Storage;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using System.Net.Http;
using login_full.Models;
using login_full.Context;




namespace login_full
{
    public sealed partial class AboutUsPage : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
        public AboutUsPage()
        {
            this.InitializeComponent();
			System.Diagnostics.Debug.WriteLine("Loading About Us Page successfully");
			this.DataContext = this;
			System.Diagnostics.Debug.WriteLine("Start loading user profile");
			LoadUserProfile();
			System.Diagnostics.Debug.WriteLine("done");

		}
		
		private void LoadUserProfile()
		{
			try
			{
				UserProfile userProfile = GlobalState.Instance.UserProfile;
				//UserProfile_Name2.Text = userProfile.Name;
				//UserProfile_Email2.Text = userProfile.Email;
				//UserName_Tag2.Text = userProfile.Name;
				//System.Diagnostics.Debug.WriteLine("User Profile Loaded: ", UserProfile_Name2.Text);
				
			}
			catch (Exception ex)
			{
				
			}
		}
		
        private void AboutUs_Click(object sender, RoutedEventArgs e)
        {
			App.MainFrame.Navigate(typeof(AboutUsPage));
		}

        private void Home_Click(object sender, RoutedEventArgs e)
        {
			App.MainFrame.Navigate(typeof(HomePage));
        }
        private async void LogoutButton_Click(object sender, RoutedEventArgs e)
        {
			localSettings.Values.Remove("Username");
			localSettings.Values.Remove("PasswordInBase64");
			localSettings.Values.Remove("EntropyInBase64");

			
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
				var newFrame = new Frame();
				App.MainFrame = newFrame;
				App.MainWindow.Content = newFrame;

				// Navigate to LoginPage
				App.MainFrame.Navigate(typeof(LoginPage));
			}
			
		}

        private void UserProfileButton_Click(object sender, RoutedEventArgs e)
        {
            var flyout = (sender as Button)?.Flyout;
            if (flyout != null)
            {
                flyout.ShowAt(sender as FrameworkElement);
            }
        }
    }
}
