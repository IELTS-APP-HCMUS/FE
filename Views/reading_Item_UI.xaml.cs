using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using login_full.ViewModels;
using login_full.Services;
using login_full.Models;
using System.Collections.Generic;
using System.Linq;

namespace login_full.Views
{
	public sealed partial class reading_Item_UI : Page
	{
		public ReadingItemsViewModel ViewModel { get; }

		public reading_Item_UI()
		{
			this.InitializeComponent();
			System.Diagnostics.Debug.WriteLine("reading_Item_UI, start getting quizzes");
			// Create instances of the services
			var readingItemsService = new ReadingItemsService();
			var navigationService = new NavigationService();
			var paginationService = new PaginationService();
			// Lấy danh sách items từ ReadingItemsService

			var searchService = new SearchService(new List<ReadingItemModels>(), paginationService);

			var completedItemsService = new CompletedItemsService(new List<ReadingItemModels>(), paginationService);

			// Initialize the ViewModel with the services
			ViewModel = new ReadingItemsViewModel(
				readingItemsService,
				navigationService,
				searchService,
				paginationService,
				completedItemsService
			);

			this.DataContext = ViewModel;
		}

		protected override void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);
			ViewModel.LoadItemsCommand.Execute(null);
		}

		private void Home_Click(object sender, RoutedEventArgs e)
		{
			Frame.Navigate(typeof(HomePage));
		}

		private void AboutUs_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.AboutUsCommand.Execute(null);
		}

		private void UserProfileButton_Click(object sender, RoutedEventArgs e)
		{
			var flyout = (sender as Button)?.Flyout;
			if (flyout != null)
			{
				flyout.ShowAt(sender as FrameworkElement);
			}
		}

		private void LogoutButton_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.LogoutCommand.Execute(null);
		}

		private void StartTest_Click(object sender, RoutedEventArgs e)
		{
			var button = sender as Button;
			var item = button.DataContext as ReadingItemModels;

			// Nếu bài đã làm, chuyển thẳng đến trang kết quả
			if (item.IsSubmitted)
			{
				//var testService = new MockReadingTestService();
				//var test = testService.GetTestDetailAsync(item.testId).Result;
				//var resultViewModel = new TestResultViewModel(test, TimeSpan.Zero, (App.Current as App).ChartService,);
				//Frame.Navigate(typeof(Views.TestResultPage), resultViewModel);
			}
			else
			{
				Frame.Navigate(typeof(Views.ReadingTestPage), item.TestId);
			}
		}

		private void ClearSearch_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ClearSearchCommand.Execute(SearchBox);
		}


		private async void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
		{
			if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
			{
				var suggestions = await ViewModel.SearchService.GetSuggestionsAsync(sender.Text);
				sender.ItemsSource = suggestions;
			}
		}

		private void PreviousPage_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.PreviousPageCommand.Execute(null);
		}

		private void NextPage_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.NextPageCommand.Execute(null);
		}

		private void PageNumber_Click(object sender, RoutedEventArgs e)
		{
			if (sender is Button button && int.TryParse(button.Content.ToString(), out int pageNumber))
			{
				ViewModel.GoToPageCommand.Execute(pageNumber);
			}
		}

		private void SearchBox_SuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
		{
			if (args.SelectedItem is ReadingItemModels selectedItem)
			{
				sender.Text = selectedItem.Title;
			}
		}

		private async void SearchBox_QuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
		{
			if (args.ChosenSuggestion != null)
			{
				// Người dùng chọn một gợi ý
				await ViewModel.SearchService.HandleSearchQueryAsync(sender.Text, true);
			}
			else
			{
				// Người dùng nhấn Enter
				await ViewModel.SearchService.HandleSearchQueryAsync(sender.Text, false);
			}
		}

		private void UncompletedFilter_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.FilterCommand.Execute(false);
		}

		private void CompletedFilter_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.FilterCommand.Execute(true);
		}
		private void ToggleFilter_Click(object sender, RoutedEventArgs e)
		{
			ViewModel.ToggleFilterCommand.Execute(null);
		}
	}
}