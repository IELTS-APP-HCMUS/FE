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
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine("Error loading user profile: " + ex.Message);
			}
		}
    }
}
