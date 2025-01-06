using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using login_full.ViewModels;
using login_full.Models;
using login_full.Services;
using System.Collections.Generic;

namespace login_full.Views
{
    /// <summary>
    /// Tang hiển thị chi tiết kết quả bài kiểm tra.
    // Hiển thị các thông tin chi tiết về câu trả lời, điểm số và phân tích kết quả.
    // </summary>
    public sealed partial class TestDetailResultPage : Page
    {
        /// <summary>
        /// ViewModel quản lý logic và dữ liệu cho trang Test Detail Result
        /// </summary>
        public TestDetailResultViewModel ViewModel { get; private set; }
        /// <summary>
        /// Khởi tạo trang Test Detail Result
        /// </summary>
        public TestDetailResultPage()
        {
            this.InitializeComponent();

        }
        /// <summary>
        /// Xử lý sự kiện khi điều hướng đến trang
        /// </summary>
        /// <param name="e">Tham số điều hướng chứa testId và answerId để tải kết quả chi tiết</param>
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
