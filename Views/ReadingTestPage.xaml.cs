using login_full.Models;
using login_full.Services;
using login_full.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.UI.Text;
using Microsoft.UI.Text;
using Microsoft.UI;



namespace login_full.Views
{
	public sealed partial class ReadingTestPage : Page
	{
		public ReadingTestViewModel ViewModel { get; }

		public ReadingTestPage()
		{
			this.InitializeComponent();
            var readingTestService = ServiceLocator.GetService<IReadingTestService>();
            var navigationService = App.NavigationService;


            // Sử dụng NavigationService từ App
            ViewModel = new ReadingTestViewModel(
                readingTestService,
                navigationService
            );
			//  Loaded += ReadingTestPage_Loaded;
		}

		//private async void ReadingTestPage_Loaded(object sender, RoutedEventArgs e)
		//{
		//    await ViewModel.LoadTestAsync("test1");
		//}
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter is ReadingItemModels item)
			{
				// Load test dựa trên TestId của item được chọn
				await ViewModel.LoadTestAsync(item.TestId);
			}
		}
	}
}