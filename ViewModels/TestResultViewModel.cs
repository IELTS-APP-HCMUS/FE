using CommunityToolkit.Mvvm.Input;
using login_full.Context;
using login_full.Models;
using login_full.Services;
using login_full.Views;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices.WindowsRuntime;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;

namespace login_full.ViewModels
{
    public class TestResultViewModel : INotifyPropertyChanged
    {
        private readonly ReadingTestDetail _testDetail;

        private readonly IChartService _chartService;

        private readonly INavigationService _navigationService;

        private Dictionary<string, int> _summary;

        // Điều hướng
        public IRelayCommand BackCommand { get; }
        public IRelayCommand RetryCommand { get; }
        public IRelayCommand HomeCommand { get; }
        public IRelayCommand ViewDetailCommand { get; }


        public string TestDuration { get; }
        public int TotalQuestions => _summary["total"];
        public int CorrectAnswers => _summary["correct"];
        public int WrongAnswers => _summary["wrong"];
        public int UnansweredQuestions => _summary["skip"];

        // Thêm properties cho biểu đồ
        public double CorrectPercentage => (double)CorrectAnswers / TotalQuestions * 100;
        public double WrongPercentage => (double)WrongAnswers / TotalQuestions * 100;
        public double UnansweredPercentage => (double)UnansweredQuestions / TotalQuestions * 100;

        // Dictionary lưu thống kê theo loại câu hỏi
        //public Dictionary<QuestionType, QuestionTypeStats> QuestionTypeStatistics { get; }

        //public TestResultViewModel(ReadingTestDetail testDetail, TimeSpan duration)
        //{
        //    _testDetail = testDetail;
        //    TestDuration = $"Thời gian làm bài: {duration.Minutes:D2}:{duration.Seconds:D2}";
        //    QuestionTypeStatistics = CalculateQuestionTypeStats();
        //}

        //private Dictionary<QuestionType, QuestionTypeStats> CalculateQuestionTypeStats()
        //{
        //    return _testDetail.Questions
        //        .GroupBy(q => q.Type)
        //        .ToDictionary(
        //            g => g.Key,
        //            g => new QuestionTypeStats
        //            {
        //                QuestionType = GetQuestionTypeDisplayName(g.Key),
        //                TotalQuestions = g.Count(),
        //                CorrectAnswers = g.Count(q => q.UserAnswer == q.CorrectAnswer),
        //                WrongAnswers = g.Count(q => q.UserAnswer != null && q.UserAnswer != q.CorrectAnswer),
        //                UnansweredQuestions = g.Count(q => q.UserAnswer == null)
        //            }
        //        );
        //}
        // Thay đổi kiểu dữ liệu từ Dictionary sang ObservableCollection
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

        public TestResultViewModel(ReadingTestDetail testDetail, TimeSpan duration, IChartService chartService, INavigationService navigationService)
        {
            _navigationService = navigationService;
            _chartService = chartService;
            _testDetail = testDetail;

            BackCommand = new RelayCommand(async () => await _navigationService.NavigateToAsync(typeof(Views.reading_Item_UI)));
            RetryCommand = new RelayCommand(async () => await RetryTest());
            HomeCommand = new RelayCommand(async () => await _navigationService.NavigateToAsync(typeof(HomePage)));
            ViewDetailCommand = new RelayCommand(async () => await _navigationService.NavigateToAsync(typeof(TestDetailResultPage), testDetail.Id));


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
		public async Task LoadSummaryAsync(string testId)
		{
			HttpClient client = new HttpClient();
			string accessToken = GlobalState.Instance.AccessToken;

			client.DefaultRequestHeaders.Authorization =
				new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

            try
            {
                HttpResponseMessage response = await client.GetAsync($"https://ielts-app-api-4.onrender.com/v1/answers/{testId}");

                if (response.IsSuccessStatusCode)
                {
                    string stringResponse = await response.Content.ReadAsStringAsync();
                    JObject jsonResponse = JObject.Parse(stringResponse);

                    JObject dataResponse = (JObject)jsonResponse["data"];
                    JObject summary = (JObject)dataResponse["summary"];
                    int did = dataResponse["detail"]["0"].Count();

                    //Dictionary<string, int> summary = 
                    //       {
                    //        { "correct", int.Parse(summary["correct"].ToString()) },
                    //        { "total", int.Parse(summary["total"].ToString()) },
                    //        { "skip", int.Parse(summary["total"].ToString()) - did }
                    //       };
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
                //Summary = await _testService.GetTestDetailAsync(testId);
                Summary = new Dictionary<string, int>
                {
                    { "correct", 0 },
                    { "wrong", 0 },
                    { "total", 0 },
                    { "skip", 0 }
                };
            }
		}
		private async Task RetryTest()
        {
            // Logic để làm lại bài thi
            await _navigationService.NavigateToAsync(typeof(ReadingTestPage), _testDetail.Id);
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
