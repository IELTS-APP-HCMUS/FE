using CommunityToolkit.Mvvm.Input;
using login_full.Models;
using login_full.Services;
using login_full.Views;
using Microsoft.UI.Xaml.Controls;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Threading.Tasks;
using login_full.API_Services;

namespace login_full.ViewModels
{
    /// <summary>
    // ViewModel quản lý và hiển thị kết quả tổng quan của bài kiểm tra.
    // Cung cấp thống kê, biểu đồ và phân tích kết quả làm bài.
    // </summary>
    public class TestResultViewModel : INotifyPropertyChanged
    {
        /// <summary>
        /// Chi tiết bài kiểm tra hiện tại
        /// </summary>
        /// <remarks>
        /// Chứa thông tin về:
        /// - Danh sách câu hỏi
        /// - Đáp án và câu trả lời
        /// - Thời gian làm bài
        /// </remarks>
        private readonly ReadingTestDetail _testDetail;
        /// <summary>
        /// Service vẽ biểu đồ thống kê
        /// </summary>
        /// <remarks>
        /// Hỗ trợ vẽ các loại biểu đồ:
        /// - Biểu đồ tròn thể hiện tỷ lệ đúng/sai
        /// - Biểu đồ cột thể hiện kết quả theo loại câu hỏi
        /// </remarks>
        private readonly IChartService _chartService;
        /// <summary>
        /// Service điều hướng giữa các trang
        /// </summary>
        private readonly INavigationService _navigationService;
        /// <summary>
        /// Tổng hợp kết quả bài kiểm tra
        /// </summary>
        /// <remarks>
        /// Dictionary chứa các chỉ số:
        /// - "total": Tổng số câu hỏi
        /// - "correct": Số câu đúng
        /// - "wrong": Số câu sai
        /// - "skip": Số câu bỏ qua
        /// </remarks>
        private Dictionary<string, int> _summary;
        /// <summary>
        /// Service gọi API
        /// </summary>
        /// <remarks>
        /// Xử lý các request đến server
        /// Quản lý cache và retry logic
        /// </remarks>
        private readonly ClientCaller _clientCaller;

        // Điều hướng
        public IRelayCommand BackCommand { get; }
        public IRelayCommand RetryCommand { get; }
        public IRelayCommand HomeCommand { get; }
        public IRelayCommand ViewDetailCommand { get; set; }


        public string TestDuration { get; }
        public int TotalQuestions => _summary["total"];
        public int CorrectAnswers => _summary["correct"];
        public int WrongAnswers => _summary["wrong"];
        public int UnansweredQuestions => _summary["skip"];

        // Thêm properties cho biểu đồ
        public double CorrectPercentage => (double)CorrectAnswers / TotalQuestions * 100;
        public double WrongPercentage => (double)WrongAnswers / TotalQuestions * 100;
        public double UnansweredPercentage => (double)UnansweredQuestions / TotalQuestions * 100;

        
        public ObservableCollection<QuestionTypeStats> QuestionTypeStatistics { get; private set; }

        private void InitializeQuestionTypeStats()
        {

            var stats = _testDetail.Questions
                .GroupBy(q => q.Type)
                .Select(g => new QuestionTypeStats
                {
                    QuestionType = GetQuestionTypeDisplayName(g.Key),
                    TotalQuestions = g.Count(),
                    CorrectAnswers = g.Count(q => q.UserAnswer == q.CorrectAnswer),
                    WrongAnswers = g.Count(q => q.UserAnswer != null && q.UserAnswer != q.CorrectAnswer),
                    UnansweredQuestions = g.Count(q => q.UserAnswer == null)
                });

            QuestionTypeStatistics = new ObservableCollection<QuestionTypeStats>(stats);
        }

        public TestResultViewModel(ReadingTestDetail testDetail, TimeSpan duration, IChartService chartService, INavigationService navigationService, string answerID)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _testDetail = testDetail;

            _clientCaller = new ClientCaller();

            BackCommand = new RelayCommand(async () => await _navigationService.NavigateToAsync(typeof(Views.reading_Item_UI)));
            RetryCommand = new RelayCommand(async () => await RetryTest());
            HomeCommand = new RelayCommand(async () => await _navigationService.NavigateToAsync(typeof(HomePage)));
			ViewDetailCommand = new RelayCommand(async () =>
			{
				var parameters = new Dictionary<string, string>
		        {
			        { "testId", _testDetail.Id },
			        { "answerId", answerID } 
                };

				await _navigationService.NavigateToAsync(typeof(TestDetailResultPage), parameters);
			});


			TestDuration = $"Thời gian làm bài: {duration.Minutes:D2}:{duration.Seconds:D2}";
            InitializeQuestionTypeStats();
        }
		public Dictionary<string, int> Summary
		{
			get => _summary;
			private set
			{
				_summary = value;
				OnPropertyChanged();
			}
		}
		public async Task LoadSummaryAsync(string answerID)
		{
            try
            {
				HttpResponseMessage response = await _clientCaller.GetAsync($"/v1/answers/{answerID}");

				if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(stringResponse);

                    JObject dataResponse = (JObject)jsonResponse["data"];
                    JObject summary = (JObject)dataResponse["summary"];
                    int did = dataResponse["detail"]["0"].Count();

                  
                    Summary = new Dictionary<string, int>
                {
                    { "correct", int.Parse(summary["correct"].ToString()) },
                    { "wrong", did - int.Parse(summary["correct"].ToString())},
                    { "total", int.Parse(summary["total"].ToString()) },
                    { "skip", int.Parse(summary["total"].ToString()) - did }
                };
                    return;
                }
            }
            catch
            {
                
                Summary = new Dictionary<string, int>
                {
                    { "correct", 0 },
                    { "wrong", 0 },
                    { "total", 0 },
                    { "skip", 0 }
                };
            }
		}
        /// <summary>
        /// Làm mới bài kiểm tra
        /// </summary>
        /// <remarks>
        /// 1. Xóa cache câu trả lời cũ
        /// 2. Điều hướng đến trang làm bài mới
        /// </remarks>
        private async Task RetryTest()
        {
			await ClearCachedAnswers();
			await _navigationService.NavigateToAsync(typeof(ReadingTestPage), _testDetail.Id);
        }

		private async Task ClearCachedAnswers()
		{
			try
			{
				// Example of clearing answers stored in the cache or local storage
				var cacheService = new CacheService(); // Replace with your actual cache service
				await cacheService.ClearTestAnswersAsync(_testDetail.Id);

				System.Diagnostics.Debug.WriteLine($"Cleared cached answers for test ID: {_testDetail.Id}");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error clearing cached answers: {ex.Message}");
			}
		}
		private string GetQuestionTypeDisplayName(QuestionType type)
        {
            return type switch
            {
                QuestionType.MultipleChoice => "MultipleChoice",
                QuestionType.GapFilling => "GapFilling",
                QuestionType.TrueFalseNotGiven => "TrueFalseNotGiven",
                QuestionType.YesNoNotGiven => "YesNoNotGiven",
                _ => type.ToString()
            };
        }
        /// <summary>
        /// Vẽ biểu đồ tròn thể hiện kết quả
        /// </summary>
        /// <param name="canvas">Canvas để vẽ biểu đồ</param>
        /// <param name="centerX">Tọa độ X của tâm</param>
        /// <param name="centerY">Tọa độ Y của tâm</param>
        /// <param name="radius">Bán kính biểu đồ</param>
        /// <remarks>
        /// Vẽ 3 phần:
        /// - Câu đúng (màu xanh)
        /// - Câu sai (màu đỏ)
        /// - Câu chưa trả lời (màu xám)
        /// </remarks>
        public void DrawChart(Canvas canvas, double centerX, double centerY, double radius)
        {
            _chartService.DrawPieChart(canvas, centerX, centerY, radius,
                CorrectPercentage, WrongPercentage, UnansweredPercentage);
        }

        public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

    public class QuestionTypeStats
    {
        public string QuestionType { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int UnansweredQuestions { get; set; }
    }
}
