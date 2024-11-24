﻿using System;
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
using Microsoft.UI;
using Windows.Graphics;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using System.Windows;

using Microsoft.UI.Windowing;
using System.Data;
using System.Text;
using Windows.Networking;
using YamlDotNet.Core.Tokens;
using System.Threading.Tasks;
using login_full.Models;
using login_full.Context;
using login_full.Components.Home.Performance;
using System.ComponentModel;




namespace login_full
{
    public sealed partial class HomePage : Page
    {
        private ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
  
		public UserProfile Profile { get; } = new UserProfile();
		public UserTarget Target { get; } = new UserTarget();

		public HomePage()
        {
            this.InitializeComponent();
			this.DataContext = this;
			LoadUserProfile();
			LoadUserTarget();
			PerformanceComponent.TargetComponentControl.TargetUpdatePopUpCompControl.RequestLoadUserTarget += TargetComponent_RequestLoadUserTarget;
		}
        private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
        {
            
        }

		
		private async void LoadUserProfile()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					string accessToken = GlobalState.Instance.AccessToken;
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					HttpResponseMessage response = await client.GetAsync("https://ielts-app-api-4.onrender.com/api/users");

					if (response.IsSuccessStatusCode)
					{
						
						string stringResponse = await response.Content.ReadAsStringAsync();

						var jsonResponse = JObject.Parse(stringResponse);

                        UserProfile userProfile = new()
						{
                            Name = jsonResponse["data"]["first_name"].ToString() + " " + jsonResponse["data"]["last_name"].ToString(),
                            Email = jsonResponse["data"]["email"].ToString(),
					    };
                        GlobalState.Instance.UserProfile = userProfile;
						Profile.Name = userProfile.Name;
						Profile.Email = userProfile.Email;
					}
					else
					{
						
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error loading user profile: {ex.Message}");
			}
		}

		
		public async void LoadUserTarget()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					string accessToken = GlobalState.Instance.AccessToken;
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					HttpResponseMessage response = await client.GetAsync("https://ielts-app-api-4.onrender.com/api/users/target");

					if (response.IsSuccessStatusCode)
					{
						string stringResponse = await response.Content.ReadAsStringAsync();

						JObject jsonResponse = JObject.Parse(stringResponse);
						JObject dataResponse = (JObject)jsonResponse["data"];
						dataResponse.Remove("id");
						UserTarget userTarget = dataResponse.ToObject<UserTarget>();

						DateOnly dateTime = DateOnly.Parse(userTarget.NextExamDate.Split(" ")[0]);
						int remainingDays = (dateTime.ToDateTime(TimeOnly.MinValue) - DateTime.Today).Days;

						
						Target.TargetListening = userTarget.TargetListening;
						Target.TargetReading = userTarget.TargetReading;
						Target.TargetSpeaking = userTarget.TargetSpeaking;
						Target.TargetWriting = userTarget.TargetWriting;
						Target.TargetStudyDuration = remainingDays;
						Target.NextExamDate = userTarget.NextExamDate;

					}
					else
					{
						string stringResponse = await response.Content.ReadAsStringAsync();
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error loading user target: {ex.Message}");
			}
			
		}
	
		private void TargetComponent_RequestLoadUserTarget(object sender, EventArgs e)
		{
			LoadUserTarget();
		}

		private void GlobalState_PropertyChanged(object sender, PropertyChangedEventArgs e)
		{
			if (e.PropertyName == nameof(GlobalState.UserProfile))
			{
				Console.WriteLine("GlobalState_PropertyChanged");
				UserTarget userTarget = GlobalState.Instance.UserTarget;
				if (userTarget != null)
				{
					Target.TargetListening = userTarget.TargetListening;
					Target.TargetReading = userTarget.TargetReading;
					Target.TargetSpeaking = userTarget.TargetSpeaking;
					Target.TargetWriting = userTarget.TargetWriting;
					Target.NextExamDate = userTarget.NextExamDate;
				}
			}
		}

		
	}
}