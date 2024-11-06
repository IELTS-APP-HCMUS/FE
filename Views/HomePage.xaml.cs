using System;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using login_full.ViewModels;
using System.Threading.Tasks;

namespace login_full
{
	public sealed partial class HomePage : Page
	{
		public HomeViewModel viewModel { get; }

		public HomePage()
		{
			this.InitializeComponent();
			viewModel = new HomeViewModel(CalendarGrid, MonthYearDisplay);
			DataContext = viewModel;
			Loaded += HomePage_Loaded;
		}

		private async void HomePage_Loaded(object sender, RoutedEventArgs e)
		{
			await viewModel.LoadUserProfileAsync();
			await viewModel.LoadUserTargetAsync();
		}

		private void AboutUs_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(AboutUsPage));
		}

		private async void AddNewEvent_Click(object sender, RoutedEventArgs e)
		{
			var dialog = new ContentDialog
			{
				Title = "Add New Event",
				PrimaryButtonText = "Add",
				CloseButtonText = "Cancel",
				XamlRoot = this.XamlRoot
			};

			StackPanel panel = new StackPanel();
			TextBox timeInput = new TextBox { PlaceholderText = "Time (e.g., 2 PM)" };
			TextBox activityInput = new TextBox { PlaceholderText = "Activity" };
			panel.Children.Add(timeInput);
			panel.Children.Add(activityInput);
			dialog.Content = panel;

			ContentDialogResult result = await dialog.ShowAsync();
			if (result == ContentDialogResult.Primary)
			{
				viewModel.AddNewEvent(timeInput.Text, activityInput.Text);
			}
		}

		private void ScoreCategoryButton_Click(object sender, RoutedEventArgs e)
		{
			IeltsScorePopup.IsOpen = true;
		}

		private void ExitButton_Click(object sender, RoutedEventArgs e)
		{
			IeltsScorePopup.IsOpen = false;
		}

		private async void ExamDatePicker_DateChanged(CalendarDatePicker sender, CalendarDatePickerDateChangedEventArgs e)
		{
			if (e.NewDate.HasValue)
			{
				
				DateTime selectedDate = e.NewDate.Value.Date;
				await viewModel.SetExamDateAsync(selectedDate);
			}
			else
			{
				
				viewModel.ExamDateText = "- / - / -";
				viewModel.RemainingDaysText = "- ngày";
			}
		}


		private void PreviousMonth_Click(object sender, RoutedEventArgs e)
		{
			viewModel.NavigateToPreviousMonth();
		}

		private void NextMonth_Click(object sender, RoutedEventArgs e)
		{
			viewModel.NavigateToNextMonth();
		}

		private void Home_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(HomePage));
		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			viewModel.LogoutCommand.Execute(null);
		}

		private void SaveButton_Click(object sender, RoutedEventArgs e)
		{
			viewModel.SaveCommand.Execute(null);
		}

		private void ScrollViewer_ViewChanged(object sender, ScrollViewerViewChangedEventArgs e)
		{
			// Add code if needed
		}

		private void UserHeaderButton_Click(object sender, RoutedEventArgs e)
		{
			var flyout = (sender as Button)?.Flyout;
			if (flyout != null)
			{
				flyout.ShowAt(sender as FrameworkElement);
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
