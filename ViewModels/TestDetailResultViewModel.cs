using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using login_full.Models;
using login_full.Services;
using login_full.Views;

namespace login_full.ViewModels
{
	public class TestDetailResultViewModel : ObservableObject
	{
		// Services
		private readonly IReadingTestService _testService;
		private readonly INavigationService _navigationService;

		// Fields
		private ReadingTestDetail _testDetail;
		private double _score;

		// Constructor
		public TestDetailResultViewModel(
			IReadingTestService testService,
			INavigationService navigationService,
			ReadingTestDetail testDetail = null)
		{
			_testService = testService;
			_navigationService = navigationService;
			_testDetail = testDetail;

			// Khởi tạo điểm nếu có sẵn bài test
			if (_testDetail != null)
			{
				CalculateScore();
			}

			// Commands
			BackCommand = new RelayCommand(GoBack);
			HomeCommand = new RelayCommand(GoHome);
			ToggleExplanationCommand = new RelayCommand<ReadingTestQuestion>(ToggleExplanation);
		}

		// Properties
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

		// Commands
		public IRelayCommand BackCommand { get; }
		public IRelayCommand HomeCommand { get; }
		public IRelayCommand<ReadingTestQuestion> ToggleExplanationCommand { get; }

		// Tính điểm từ dữ liệu đã có
		private void CalculateScore()
		{
			if (_testDetail?.Questions == null || !_testDetail.Questions.Any())
			{
				Score = 0;
				return;
			}

			int correctAnswers = _testDetail.Questions.Count(q => q.UserAnswer == q.CorrectAnswer);
			Score = Math.Round((double)correctAnswers / _testDetail.Questions.Count * 10, 1);
		}

		// Điều hướng về trang trước
		private void GoBack()
		{
			_navigationService.NavigateToAsync(typeof(TestResultPage));
		}

		// Điều hướng về trang chính
		private void GoHome()
		{
			_navigationService.NavigateToAsync(typeof(HomePage));
		}

		// Tải dữ liệu test khi cần
		public async Task LoadTestAsync(string testId)
		{
			TestDetail = await _testService.GetTestDetailAsync(testId);
			CalculateScore(); 
		}

		// Toggle hiển thị lời giải thích
		private void ToggleExplanation(ReadingTestQuestion question)
		{
			if (question != null)
			{
				System.Diagnostics.Debug.WriteLine($"Toggling explanation for question: {question.QuestionText}");
				question.ToggleExplanation();
				System.Diagnostics.Debug.WriteLine($"Explanation text: {question.Explanation}");
			}
		}

		public async Task LoadDataAsync(string testId, string answerId)
		{
			// Debug log để kiểm tra Test ID và Answer ID
			System.Diagnostics.Debug.WriteLine($"Getting quiz Test ID: {testId}, Answer ID: {answerId}");

			// Lấy chi tiết bài test
			TestDetail = await _testService.GetTestDetailAsync(testId);

			System.Diagnostics.Debug.WriteLine($"Getting answer from : {testId}, Answer ID: {answerId}");

			// Lấy chi tiết câu trả lời
			var answerResult = await _testService.GetAnswerDetailAsync(answerId);

			// Nếu không có dữ liệu trả về, dừng lại
			if (answerResult == null || !answerResult.Detail.ContainsKey("0"))
			{
				System.Diagnostics.Debug.WriteLine("Không có dữ liệu trả lời.");
				return;
			}

			var answers = answerResult.Detail["0"];

			// Duyệt từng câu hỏi trong TestDetail để ghép đáp án
			foreach (var question in TestDetail.Questions)
			{
				var answer = answers.FirstOrDefault(a => a.IdQuestion == int.Parse(question.Id));

				if (answer != null && answer.Answer?.Title != null)
				{
					// Gán câu trả lời của người dùng
					question.UserAnswer = answer.Answer.Title.FirstOrDefault();

					// Nếu là câu hỏi điền từ (GapFilling)
					if (question.Type == QuestionType.GapFilling)
					{
						// Debug kiểm tra câu hỏi điền từ
						System.Diagnostics.Debug.WriteLine($"GapFilling Question: {question.QuestionText}");
						System.Diagnostics.Debug.WriteLine($"UserAnswer: {question.UserAnswer}, CorrectAnswer: {question.CorrectAnswer}");

						// Sử dụng IsCorrectAnswer để kiểm tra đúng/sai
						bool isCorrect = question.IsCorrectAnswer;

						// Log kết quả đúng/sai
						System.Diagnostics.Debug.WriteLine($"IsCorrect: {isCorrect}");
					}
					else
					{
						// Xử lý các loại câu hỏi khác (MultipleChoice, TrueFalseNotGiven, etc.)
						question.InitializeOptionModels();

						foreach (var option in question.OptionModels)
						{
							option.IsSelected = option.Text == question.UserAnswer;
							option.IsCorrect = option.Text == question.CorrectAnswer;
							option.IsWrong = option.IsSelected && option.Text != question.CorrectAnswer;

							// Debug log để kiểm tra thông tin câu hỏi trắc nghiệm
							System.Diagnostics.Debug.WriteLine($"Option: {option.Text}, Selected: {option.IsSelected}, Correct: {option.IsCorrect}");
						}
					}
				}
			}

			// Tính toán điểm số nếu có dữ liệu tổng hợp
			if (answerResult.Summary != null)
			{
				int correctAnswers = answerResult.Summary.Correct;
				int totalQuestions = answerResult.Summary.Total;

				// Tính điểm dựa trên thông tin trả về từ API
				Score = Math.Round((double)correctAnswers / totalQuestions * 10, 1);
			}

			// Cập nhật UI
			OnPropertyChanged(nameof(TestDetail));
			OnPropertyChanged(nameof(Score));
		}

		// Map loại câu hỏi từ API
		private QuestionType MapQuestionType(string questionType)
		{
			return questionType switch
			{
				"MULTIPLE_CHOICE" => QuestionType.MultipleChoice,
				"FILL_IN_THE_BLANK" => QuestionType.GapFilling,
				"TRUE_FALSE" => QuestionType.TrueFalseNotGiven,
				"YES_NO_NOT_GIVEN" => QuestionType.YesNoNotGiven,
				_ => throw new ArgumentException($"Unknown question type: {questionType}")
			};
		}
	}
}
