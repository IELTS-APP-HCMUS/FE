using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Input;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using login_full.Helpers;
using login_full.Models;
using Microsoft.UI.Xaml;
using Microsoft.Extensions.Configuration;

namespace login_full.ViewModels
{
	public class HomeViewModel : INotifyPropertyChanged
	{
		public ObservableCollection<ScheduleItem> ScheduleItems { get; set; } = new ObservableCollection<ScheduleItem>();

		private UserProfile userProfile;
		private UserTarget userTarget;
		private DateTime? examDate;
		private int remainingDays;
		private readonly CalendarManager calendarManager;

		public event PropertyChangedEventHandler PropertyChanged;

		public UserProfile UserProfile
		{
			get => userProfile;
			set { userProfile = value; OnPropertyChanged(); }
		}

		public UserTarget UserTarget
		{
			get => userTarget;
			set { userTarget = value; OnPropertyChanged(); }
		}

		public DateTime? ExamDate
		{
			get => examDate;
			private set
			{
				examDate = value;
				UpdateRemainingDays();
				OnPropertyChanged();
			}
		}

		public int RemainingDays
		{
			get => remainingDays;
			private set { remainingDays = value; OnPropertyChanged(); }
		}

		// Bindable properties for UI
		public string ReadingScore { get; set; }
		public string ListeningScore { get; set; }
		public string WritingScore { get; set; }
		public string SpeakingScore { get; set; }
		public string OverallScore { get; set; }
		public string RemainingDaysText { get; set; }
		public string ExamDateText { get; set; }

		public ICommand SaveCommand { get; }
		public ICommand LogoutCommand { get; }
		public ICommand NavigatePreviousMonthCommand { get; }
		public ICommand NavigateNextMonthCommand { get; }

		public HomeViewModel(Grid calendarGrid, TextBlock monthYearDisplay)
		{
			calendarManager = new CalendarManager(calendarGrid, monthYearDisplay);
			SaveCommand = new RelayCommand(async () => await SaveUserTargetAsync());
			LogoutCommand = new RelayCommand(async () => await LogoutAsync());
			NavigatePreviousMonthCommand = new RelayCommand(NavigateToPreviousMonth);
			NavigateNextMonthCommand = new RelayCommand(NavigateToNextMonth);
		}

		public async Task LoadUserProfileAsync()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					string accessToken = GlobalState.Instance.AccessToken;
					client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					HttpResponseMessage response = await client.GetAsync("https://ielts-app-api-4.onrender.com/api/users");

					if (response.IsSuccessStatusCode)
					{
						string jsonResponse = await response.Content.ReadAsStringAsync();
						var userProfile = JObject.Parse(jsonResponse);

						if (userProfile.ContainsKey("data") && userProfile["data"] is JObject data)
						{
							string firstName = data.Value<string>("first_name") ?? "";
							string lastName = data.Value<string>("last_name") ?? "";
							string email = data.Value<string>("email") ?? "";

							UserProfile = new UserProfile
							{
								Name = $"{firstName} {lastName}".Trim(),
								Email = email
							};
							Console.WriteLine($"User Profile Loaded: {UserProfile?.Name}");

							GlobalState.Instance.UserProfile = UserProfile;
						}
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading user profile: {ex.Message}");
			}
		}

		public async Task LoadUserTargetAsync()
		{
			try
			{
				using (HttpClient client = new HttpClient())
				{
					string accessToken = GlobalState.Instance.AccessToken;
					client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);
					HttpResponseMessage response = await client.GetAsync("https://ielts-app-api-4.onrender.com/api/users/target");

					if (response.IsSuccessStatusCode)
					{
						string jsonResponse = await response.Content.ReadAsStringAsync();
						var data = JObject.Parse(jsonResponse)["data"];
						UserTarget = data.ToObject<UserTarget>();

						UpdateScoresAndDates(UserTarget);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine($"Error loading user target: {ex.Message}");
			}
		}

		private void UpdateScoresAndDates(UserTarget userTarget)
		{
			ReadingScore = userTarget.TargetReading == -1 ? "-" : userTarget.TargetReading.ToString();
			ListeningScore = userTarget.TargetListening == -1 ? "-" : userTarget.TargetListening.ToString();
			WritingScore = userTarget.TargetWriting == -1 ? "-" : userTarget.TargetWriting.ToString();
			SpeakingScore = userTarget.TargetSpeaking == -1 ? "-" : userTarget.TargetSpeaking.ToString();

			if (DateTime.TryParse(userTarget.NextExamDate, out var parsedExamDate))
			{
				ExamDate = parsedExamDate;
				RemainingDaysText = $"{(parsedExamDate - DateTime.Today).Days} ngày";
				ExamDateText = parsedExamDate.ToString("yyyy-MM-dd");
			}
			else
			{
				RemainingDaysText = "-";
				ExamDateText = "-";
			}

			double overallTarget = -1;
			if (userTarget.TargetReading != -1 && userTarget.TargetListening != -1 &&
				userTarget.TargetWriting != -1 && userTarget.TargetSpeaking != -1)
			{
				overallTarget = (userTarget.TargetReading + userTarget.TargetListening +
								 userTarget.TargetWriting + userTarget.TargetSpeaking) / 4.0;
				overallTarget = Math.Round(overallTarget * 2) / 2; // Rounded to nearest 0.5
			}
			OverallScore = overallTarget == -1 ? "-" : overallTarget.ToString();
			Console.WriteLine($"User Profile Loaded: {UserTarget?.TargetListening}");
		}

		public async Task SetExamDateAsync(DateTime selectedDate)
		{
			ExamDate = selectedDate;
			ExamDateText = selectedDate.ToString("dd / MM / yyyy");

			var targetRequest = new { next_exam_date = selectedDate.ToString("yyyy-MM-dd") };
			string json = JsonConvert.SerializeObject(targetRequest);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			try
			{
				using (HttpClient client = new HttpClient())
				{
					string accessToken = GlobalState.Instance.AccessToken;
					client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

					HttpResponseMessage response = await client.PatchAsync("https://ielts-app-api-4.onrender.com/api/users/target", content);
					if (response.IsSuccessStatusCode)
					{
						UpdateRemainingDays();
					}
					else
					{
						Console.WriteLine("Failed to update exam date. Status code: " + response.StatusCode);
					}
				}
			}
			catch (Exception ex)
			{
				Console.WriteLine("Error updating exam date: " + ex.Message);
			}
		}

		private async Task SaveUserTargetAsync()
		{
			if (UserTarget == null) return;

			using HttpClient client = new HttpClient();
			string json = JsonConvert.SerializeObject(UserTarget);
			var content = new StringContent(json, Encoding.UTF8, "application/json");

			string accessToken = GlobalState.Instance.AccessToken;
			client.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

			HttpResponseMessage response = await client.PatchAsync("https://ielts-app-api-4.onrender.com/api/users/target", content);
			if (response.IsSuccessStatusCode)
			{
				await LoadUserTargetAsync();
			}
		}

		private async Task LogoutAsync()
		{
			var localSettings = Windows.Storage.ApplicationData.Current.LocalSettings;
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
				App.IsLoggedInWithGoogle = false;
			}

			var window = (Application.Current as App)?.MainWindow;
			if (window != null)
			{
				var newMainWindow = new MainWindow();
				newMainWindow.Activate();
				window.Close();
			}
		}

		public void AddNewEvent(string time, string activity)
		{
			ScheduleItems.Add(new ScheduleItem { Time = time, Activity = activity });
		}

		public void NavigateToPreviousMonth() => calendarManager.PreviousMonth();
		public void NavigateToNextMonth() => calendarManager.NextMonth();

		private void UpdateRemainingDays()
		{
			RemainingDays = ExamDate.HasValue ? (ExamDate.Value - DateTime.Today).Days : -1;
		}

		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

	public class RelayCommand : ICommand
	{
		private readonly Action execute;
		private readonly Func<bool> canExecute;

		public RelayCommand(Action execute, Func<bool> canExecute = null)
		{
			this.execute = execute;
			this.canExecute = canExecute;
		}

		public bool CanExecute(object parameter) => canExecute == null || canExecute();
		public void Execute(object parameter) => execute();
		public event EventHandler CanExecuteChanged;
		public void RaiseCanExecuteChanged() => CanExecuteChanged?.Invoke(this, EventArgs.Empty);
	}
}
