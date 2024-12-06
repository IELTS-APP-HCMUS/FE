using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using login_full.Models;
using login_full.Services;
using login_full.ViewModels;
using login_full.Views;
using Microsoft.Extensions.Logging;


namespace login_full.ViewModels
{
    public class TestDetailResultViewModel : ObservableObject
    {
		private readonly IReadingTestService _testService;
		private readonly INavigationService _navigationService;
        private ReadingTestDetail _testDetail;
        private double _score;

        public TestDetailResultViewModel(IReadingTestService testService, INavigationService navigationService)

		{
            //_testDetail = testDetail;
            _testService = testService;
            _navigationService = navigationService;
            
            // Tính điểm
            CalculateScore();
            
            // Khởi tạo commands
            BackCommand = new RelayCommand(GoBack);
            HomeCommand = new RelayCommand(GoHome);
            ToggleExplanationCommand = new RelayCommand<Question>(ToggleExplanation);
        }

        //public ReadingTestDetail TestDetail => _testDetail;
		public ReadingTestDetail TestDetail
		{
			get => _testDetail;
			private set
			{
				_testDetail = value;
				OnPropertyChanged();
			}
		}

		public double Score
        {
            get => _score;
            private set => SetProperty(ref _score, value);
        }

        public IRelayCommand BackCommand { get; }
        public IRelayCommand HomeCommand { get; }
        public IRelayCommand<Question> ToggleExplanationCommand { get; }

        private void CalculateScore()
        {
            if (_testDetail?.Questions == null || !_testDetail.Questions.Any())
            {
                Score = 0;
                return;
            }

            int correctAnswers = _testDetail.Questions.Count(q => 
                q.UserAnswer == q.CorrectAnswer);
            
            Score = Math.Round((double)correctAnswers / _testDetail.Questions.Count * 10, 1);
        }

        private void GoBack()
        {
            _navigationService.NavigateToAsync(typeof(TestResultPage));
        }

        private void GoHome()
        {
            _navigationService.NavigateToAsync(typeof(HomePage));
        }
		public async Task LoadTestAsync(string testId)
		{
			TestDetail = await _testService.GetTestDetailAsync(testId);
		}
		private void ToggleExplanation(Question question)
        {
            if (question != null)
            {
                System.Diagnostics.Debug.WriteLine($"Toggling explanation for question: {question.QuestionText}");
                System.Diagnostics.Debug.WriteLine($"Current visibility: {question.IsExplanationVisible}");
                
                question.ToggleExplanation();
                
                System.Diagnostics.Debug.WriteLine($"New visibility: {question.IsExplanationVisible}");
                System.Diagnostics.Debug.WriteLine($"Explanation text: {question.Explanation}");
            }
        }
    }
} 