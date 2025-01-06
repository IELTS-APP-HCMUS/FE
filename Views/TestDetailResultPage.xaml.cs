using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using login_full.ViewModels;
using login_full.Models;
using login_full.Services;
using System.Collections.Generic;

namespace login_full.Views
{
    public sealed partial class TestDetailResultPage : Page
    {
        public TestDetailResultViewModel ViewModel { get; private set; }

        public TestDetailResultPage()
        {
            this.InitializeComponent();

        }

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter is Dictionary<string, string> parameters &&
				parameters.TryGetValue("testId", out string testId) &&
				parameters.TryGetValue("answerId", out string answerId))
			{
				System.Diagnostics.Debug.WriteLine($"Navigated with Test ID: {testId} and Answer ID: {answerId}");

				var readingTestService = ServiceLocator.GetService<IReadingTestService>();
				var navigationService = App.NavigationService;

				
				ViewModel = new TestDetailResultViewModel(
					readingTestService,
					navigationService,
					new ReadingTestDetail());

				this.DataContext = ViewModel;

				await ViewModel.LoadDataAsync(testId, answerId);
			}
			else
			{
				System.Diagnostics.Debug.WriteLine("Invalid navigation parameters.");
			}
		}
	}
}
