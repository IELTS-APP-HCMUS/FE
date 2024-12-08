using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using login_full.ViewModels;
using login_full.Models;
using login_full.Services;

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

			if (e.Parameter is string testId)
			{
				System.Diagnostics.Debug.WriteLine($"Navigated with Test ID: {testId}");

	
				var readingTestService = ServiceLocator.GetService<IReadingTestService>();
				var navigationService = App.NavigationService;

				var testDetail = await readingTestService.GetTestDetailAsync(testId);
				if (testDetail == null)
				{
					System.Diagnostics.Debug.WriteLine("TestDetail is null. Exiting.");
					return;
				}

				System.Diagnostics.Debug.WriteLine($"TestDetail fetched: {testDetail.Title} with {testDetail.Questions?.Count} questions");

				
				ViewModel = new TestDetailResultViewModel(
					readingTestService,
					navigationService,
					testDetail
				);


				foreach (var question in testDetail.Questions)
				{
					System.Diagnostics.Debug.WriteLine($"Initializing options for question: {question.QuestionText}");
					question.InitializeOptionModels();
				}

				this.DataContext = ViewModel;
				System.Diagnostics.Debug.WriteLine("DataContext set successfully.");
			}
		}
    }
}
