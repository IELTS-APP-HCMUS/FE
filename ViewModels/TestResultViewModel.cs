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
    public class TestResultViewModel : INotifyPropertyChanged
    {
        private readonly ReadingTestDetail _testDetail;

        private readonly IChartService _chartService;

        private readonly INavigationService _navigationService;

        private Dictionary<string, int> _summary;

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
		private async Task RetryTest()
        {
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
