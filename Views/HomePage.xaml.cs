using System;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using System.Net.Http;
using Newtonsoft.Json.Linq;
using login_full.Models;
using login_full.Context;
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

						if (dateTime.Year == 1900)
						{
							dateTime = DateOnly.FromDateTime(DateTime.Now); 
						}

						Target.TargetListening = userTarget.TargetListening == -1 ? 0 : userTarget.TargetListening;
						Target.TargetReading = userTarget.TargetReading == -1 ? 0 : userTarget.TargetReading;
						Target.TargetSpeaking = userTarget.TargetSpeaking == -1 ? 0 : userTarget.TargetSpeaking;
						Target.TargetWriting = userTarget.TargetWriting == -1 ? 0 : userTarget.TargetWriting;
						Target.TargetStudyDuration = remainingDays >= 0 ? remainingDays : 0;
						Target.NextExamDate = dateTime.ToString("yyyy-MM-dd");

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