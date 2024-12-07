using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using login_full.Context;
using Newtonsoft.Json;


namespace login_full.Services
{
	public class ReadingTestService : IReadingTestService
	{
		private readonly Dictionary<string, ReadingTestDetail> _mockTests;
		private readonly List<TestHistory> _testHistory;
		private readonly LocalStorageService _localStorageService;
		private readonly HttpClient _httpClient;

		public ReadingTestService(LocalStorageService localStorageService)
		{
			_localStorageService = localStorageService;
			_testHistory = _localStorageService.GetTestHistory();
			//_mockTests = new Dictionary<string, ReadingTestDetail>
			//{


			//};
			_mockTests = new Dictionary<string, ReadingTestDetail>();
			_httpClient = new HttpClient();
		}

		public async Task<ReadingTestDetail> GetTestDetailAsync(string testId)
		{
			if (_mockTests.ContainsKey(testId))
			{
				// Fetch from local cache if available
				return _mockTests[testId];
			}

			try
			{
				string accessToken = GlobalState.Instance.AccessToken;
				_httpClient.DefaultRequestHeaders.Authorization =
					new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

				string apiUrl = $"https://ielts-app-api-4.onrender.com/v1/quizzes/{testId}";
				HttpResponseMessage response = await _httpClient.GetAsync(apiUrl);

				if (response.IsSuccessStatusCode)
				{
					string content = await response.Content.ReadAsStringAsync();
					var apiResponse = JsonConvert.DeserializeObject<QuizDetailApiResponse>(content);

					if (apiResponse?.Data != null)
					{
						var quizData = apiResponse.Data;

						// Map API data to ReadingTestDetail
						var readingTestDetail = new ReadingTestDetail
						{
							Id = quizData.Id.ToString(),
							Title = quizData.Title,
							Content = quizData.Content,
							TimeLimit = quizData.Time,
							Questions = quizData.Parts
								.SelectMany(part => part.Questions)
								.Select(q => new ReadingTestQuestion
								{
									Id = q.Id.ToString(),
									QuestionText = q.Title,
									Type = MapQuestionType(q.QuestionType),
									Options = q.Selection?.Select(opt => opt.Text).ToList(),
									CorrectAnswer = q.Selection?.FirstOrDefault(opt => opt.Correct)?.Text,
									Explanation = q.Explain
								})
								.ToList(),
							Progress = new TestProgress
							{
								TotalQuestions = quizData.Parts.Sum(part => part.Questions.Count),
								AnsweredQuestions = 0,
								RemainingTime = quizData.Time * 60,
								IsCompleted = false
							}
						};

						// Cache the fetched test in _mockTests
						_mockTests[testId] = readingTestDetail;

						return readingTestDetail;
					}
				}

				throw new Exception($"Failed to fetch test details: {response.ReasonPhrase}");
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in GetTestDetailAsync: {ex.Message}");
				throw;
			}
		}


		//public async Task<ReadingTestDetail> GetTestDetailAsync(string testId)
		//{
		//	// Giả lập delay của network
		//	await Task.Delay(500);
		//	return _mockTests.GetValueOrDefault(testId) ??
		//		throw new Exception("Test not found");
		//}
		// tuog tu
		public async Task SaveAnswerAsync(string testId, string questionId, string answer)
		{
			await Task.Delay(100); // Giả lập delay
			if (_mockTests.TryGetValue(testId, out var test))
			{
				var question = test.Questions.FirstOrDefault(q => q.Id == questionId);
				if (question != null)
				{
					question.UserAnswer = answer;
				}
			}
		}

		public async Task<bool> UpdateTestCompletionStatus(string testId, bool isCompleted)
		{
			await Task.Delay(100);
			if (_mockTests.ContainsKey(testId))
			{
				_mockTests[testId].Progress.IsCompleted = isCompleted;
				return true;
			}
			return false;
		}


		// đã update vs testID bên cs
		public async Task<bool> SubmitTestAsync(string testId)
		{
			try
			{
				if (_mockTests.ContainsKey(testId))
				{
					var test = _mockTests[testId];
					test.Progress.IsCompleted = true;

					// Tính toán kết quả
					var correctAnswers = test.Questions.Count(q => q.UserAnswer == q.CorrectAnswer);
					var wrongAnswers = test.Questions.Count(q => !string.IsNullOrEmpty(q.UserAnswer) && q.UserAnswer != q.CorrectAnswer);
					var skippedAnswers = test.Questions.Count(q => string.IsNullOrEmpty(q.UserAnswer));

					// Tạo đối tượng lịch sử mới
					var testHistory = new TestHistory
					{
						TestId = testId,
						Title = test.Title,
						SubmitTime = DateTime.Now,
						Duration = TimeSpan.FromSeconds(test.TimeLimit * 60 - test.Progress.RemainingTime),
						TotalQuestions = test.Questions.Count,
						CorrectAnswers = correctAnswers,
						WrongAnswers = wrongAnswers,
						SkippedAnswers = skippedAnswers
					};

					// Thêm vào list (đã được khởi tạo trong constructor)
					_testHistory.Add(testHistory);
					_localStorageService.SaveTestHistory(_testHistory);


					// Cập nhật Progress
					test.Progress.AnsweredQuestions = correctAnswers;
					await UpdateTestCompletionStatus(testId, true);
					return true;
				}
				return false;
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in SubmitTestAsync: {ex.Message}");
				return false;
			}
		}

		public async Task<List<TestHistory>> GetTestHistoryAsync()
		{
			try
			{
				await Task.Delay(100); // Simulate network delay
				return _localStorageService.GetTestHistory().OrderByDescending(h => h.SubmitTime).ToList();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in GetTestHistoryAsync: {ex.Message}");
				return new List<TestHistory>();
			}
		}

		private QuestionType MapQuestionType(string questionType)
		{
			return questionType switch
			{
				"MULTIPLE_CHOICE_ONE" => QuestionType.MultipleChoice,
				"FILL-IN-THE-BLANK" => QuestionType.GapFilling,
				"TRUE_FALSE" => QuestionType.TrueFalseNotGiven,
				"YES_NO_NOT_GIVEN" => QuestionType.YesNoNotGiven,
				"MATCHING_HEADING" => QuestionType.GapFilling,
				_ => throw new ArgumentException($"Unknown question type: {questionType}")
			};
		}

	}
}

// await Task.Delay(500); // Giả lập delay