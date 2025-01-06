using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Net.Http;
using login_full.Context;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using login_full.API_Services;



namespace login_full.Services
{
	public class ReadingTestService : IReadingTestService
	{
        /// <summary>
        /// Bộ nhớ cache cho các bài kiểm tra giả lập.
        /// </summary>
        private readonly Dictionary<string, ReadingTestDetail> _mockTests;
        /// <summary>
        /// Lịch sử các bài kiểm tra.
        /// </summary>
        private readonly List<TestHistory> _testHistory;
		private readonly ClientCaller _clientCaller;
		private readonly DictionaryService _dictionaryService;
        /// <summary>
        /// Khởi tạo một instance mới của <see cref="ReadingTestService"/>.
        /// </summary>
        /// <param name="localStorageService">Service lưu trữ cục bộ</param>
        /// <param name="dictionaryService">Service từ điển</param>
        public ReadingTestService(LocalStorageService localStorageService, DictionaryService dictionaryService)
		{
			_mockTests = new Dictionary<string, ReadingTestDetail>();
			_clientCaller = new ClientCaller();
			_dictionaryService = dictionaryService;
		}
        /// <summary>
        /// Lấy chi tiết bài kiểm tra từ API hoặc bộ nhớ cache.
        /// </summary>
        /// <param name="testId">ID của bài kiểm tra</param>
        /// <returns>Chi tiết bài kiểm tra</returns>
        public async Task<ReadingTestDetail> GetTestDetailAsync(string testId)
		{
			if (_mockTests.ContainsKey(testId))
			{
				// Fetch từ bộ nhớ cache
				return _mockTests[testId];
			}

			try
			{
				// Gọi API để lấy chi tiết bài kiểm tra
				HttpResponseMessage response = await _clientCaller.GetAsync($"/v1/quizzes/{testId}");

				if (response.IsSuccessStatusCode)
				{
					// Đọc dữ liệu trả về
					string content = await response.Content.ReadAsStringAsync();
					var apiResponse = JsonConvert.DeserializeObject<QuizDetailApiResponse>(content);

					// Kiểm tra dữ liệu hợp lệ
					if (apiResponse?.Data != null)
					{
						var quizData = apiResponse.Data;

						// Map dữ liệu API sang Model
						var readingTestDetail = new ReadingTestDetail
						{
							Id = quizData.Id.ToString(),
							Title = quizData.Title,
							Content = quizData.Content,
							TimeLimit = quizData.Time,
							Questions = quizData.Parts
								.SelectMany(part => part.Questions)
								.Select(q =>
								{
									bool isGapFilling = q.QuestionType == "FILL-IN-THE-BLANK";
					
									var question = new ReadingTestQuestion
									{
										Id = q.Id.ToString(),
										QuestionText = q.Title,
										Type = MapQuestionType(q.QuestionType),

										CorrectAnswer = isGapFilling
											? q.GapFillInBlankCorrectAnswer 
											: q.Selection?.FirstOrDefault(opt => opt.Correct)?.Text,

										Options = isGapFilling
											? null // Gap Filling không có options
											: q.Selection?.Select(opt => opt.Text).ToList(),

										Explanation = q.Explain
									};

									question.InitializeOptionModels();

									return question;
								})
								.ToList(),

							// Thêm thông tin tiến độ
							Progress = new TestProgress
							{
								TotalQuestions = quizData.Parts.Sum(part => part.Questions.Count),
								AnsweredQuestions = 0,
								RemainingTime = quizData.Time * 60,
								IsCompleted = false
							}
						};

						// Lưu cache để dùng sau
						_mockTests[testId] = readingTestDetail;

						// Log dữ liệu nhận được
						string jsonOutput = JsonConvert.SerializeObject(readingTestDetail, Formatting.Indented);
						System.Diagnostics.Debug.WriteLine($"Fetched test detail: {jsonOutput}");

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
        /// <summary>
        /// Lưu câu trả lời của người dùng cho một câu hỏi.
        /// </summary>
        /// <param name="testId">ID của bài kiểm tra</param>
        /// <param name="questionId">ID của câu hỏi</param>
        /// <param name="answer">Câu trả lời của người dùng</param>
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
        /// <summary>
        /// Cập nhật trạng thái hoàn thành của bài kiểm tra.
        /// </summary>
        /// <param name="testId">ID của bài kiểm tra</param>
        /// <param name="isCompleted">Trạng thái hoàn thành</param>
        /// <returns>Trả về true nếu cập nhật thành công, ngược lại false</returns>
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
        /// <summary>
        /// Nộp bài kiểm tra và gửi dữ liệu lên server.
        /// </summary>
        /// <param name="testId">ID của bài kiểm tra</param>
        /// <returns>ID của bài kiểm tra đã nộp</returns>
        public async Task<string> SubmitTestAsync(string testId)
		{
			try
			{
				if (_mockTests.ContainsKey(testId))
				{
					var test = _mockTests[testId];

					
					var payload = new
					{
						
						question = test.Questions
							.Where(q => !string.IsNullOrWhiteSpace(q.UserAnswer)) 
							.Select(q => new
							{
								id = int.Parse(q.Id), 
								success_count = q.Type == QuestionType.GapFilling
									? (q.UserAnswer == q.CorrectAnswer ? 1 : 0) 
									: (q.IsCorrectAnswer ? 1 : 0), 
								total = 1 
							}),

						answer = new
						{
							detail = new Dictionary<string, object>
					{
						{
							"0", test.Questions
								.Select((q, index) => new
								{
									answer = new
									{
										title = q.Type == QuestionType.GapFilling
											? (!string.IsNullOrWhiteSpace(q.UserAnswer) ? new List<string> { q.UserAnswer } : new List<string>())
											: q.Options?.Where(opt => opt == q.UserAnswer).ToList()
									},
									type = MapQuestionTypeForApi(q.Type),
									correct = q.Type == QuestionType.GapFilling
										? q.UserAnswer == q.CorrectAnswer 
                                        : q.IsCorrectAnswer, 
                                    question = index + 1,
									id_question = int.Parse(q.Id)
								})
								.Where(q => q.answer.title.Count > 0) 
                                .ToList()
						}
					},
							quiz = int.Parse(testId),
							type = 1,
							status = "reviewed",
							completed_duration = test.Progress.RemainingTime,

							// Thống kê tổng số câu hỏi, số câu trả lời đúng và thời gian còn lại
							summary = new
							{
								correct = test.Questions.Count(q =>
									!string.IsNullOrWhiteSpace(q.UserAnswer) && // Chỉ đếm câu đã trả lời
									(q.Type == QuestionType.GapFilling
										? q.UserAnswer == q.CorrectAnswer
										: q.IsCorrectAnswer)),

								total = test.Questions.Count, // Tổng số câu hỏi bao gồm cả bỏ qua
								left_time = TimeSpan.FromSeconds(test.Progress.RemainingTime).ToString(@"hh\:mm\:ss"),
								mocktest_time = test.TimeLimit,
								type = "practice"
							}
						}
					};

					// Log payload để kiểm tra nội dung trước khi gửi
					string jsonPayload = JsonConvert.SerializeObject(payload, Formatting.Indented);
					System.Diagnostics.Debug.WriteLine($"Submitting payload: {jsonPayload}");

					// Tạo HTTP request
					string apiUrl = $"/v1/quizzes/{testId}/answer";
					var requestContent = new StringContent(jsonPayload, Encoding.UTF8, "application/json");

					// Gửi request
					HttpResponseMessage response = await _clientCaller.PostAsync(apiUrl, requestContent);

					// Kiểm tra phản hồi từ server
					if (response.IsSuccessStatusCode)
					{
						string responseContent = await response.Content.ReadAsStringAsync();
						System.Diagnostics.Debug.WriteLine($"Submit API Response: {responseContent}");

						var result = JsonConvert.DeserializeObject<dynamic>(responseContent);
						return result.data.id.ToString();
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"Error submitting test: {response.StatusCode} - {response.ReasonPhrase}");
					}
				}

				return "";
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in SubmitTestAsync: {ex.Message}");
				return "";
			}
		}



        /// <summary>
        /// Lấy lịch sử các bài kiểm tra.
        /// </summary>
        /// <returns>Danh sách lịch sử các bài kiểm tra</returns>
        public async Task<List<TestHistory>> GetTestHistoryAsync()
		{
			try
			{
				HttpResponseMessage response = await _clientCaller.GetAsync($"/v1/answers/statistics?skill_id=1&type=1&page=1&page_size=20");

				if (response.IsSuccessStatusCode)
				{
					string stringResponse = await response.Content.ReadAsStringAsync();
					JObject jsonResponse = JObject.Parse(stringResponse);

					JObject dataResponse = (JObject)jsonResponse["data"];
					JArray items = (JArray)dataResponse["items"];
					var histories = new List<TestHistory>();
					foreach (var i in items)
					{
						JObject detail = (JObject)i["detail"];
                        TestHistory history = new()
                        {
							AnswerId = i["id"].ToString(),
							TestId = i["quiz_id"].ToString(),
							Title = detail["title"].ToString(),
                            SubmitTime = DateTime.Parse(i["date_created"].ToString()),
							Duration = TimeSpan.Parse(i["completed_duration"].ToString()),
							TotalQuestions = int.Parse(i["total"].ToString()),
							CorrectAnswers = int.Parse(i["success"].ToString()),
							WrongAnswers = int.Parse(i["failed"].ToString()),
							SkippedAnswers = int.Parse(i["skipped"].ToString())
						};
						histories.Add(history);
						//HttpResponseMessage i_response = await _clientCaller.GetAsync($"/v1/quizzes/" + quizID);
						//if (i_response.IsSuccessStatusCode)
						//{
						//	string i_stringResponse = await i_response.Content.ReadAsStringAsync();
						//	JObject i_jsonResponse = JObject.Parse(i_stringResponse);

						//	JObject i_dataResponse = (JObject)i_jsonResponse["data"];
						//	string i_title = i_dataResponse["title"].ToString();
						//	DateTime date_created = DateTime.Parse(i["date_created"].ToString());
						//	TestHistory history = new TestHistory
						//	{
						//		AnswerId = i["id"].ToString(),
						//		TestId = quizID.ToString(),
						//		Title = i_title,
						//		SubmitTime = date_created,
						//		Duration = TimeSpan.Parse(i["completed_duration"].ToString()),
						//		TotalQuestions = int.Parse(i["total"].ToString()),
						//		CorrectAnswers = int.Parse(i["success"].ToString()),
						//		WrongAnswers = int.Parse(i["failed"].ToString()),
						//		SkippedAnswers = int.Parse(i["skipped"].ToString())
						//	};
						//	histories.Add(history);
						//}
					}
                    return histories;
				}
				return new List<TestHistory>();
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error in GetTestHistoryAsync: {ex.Message}");
				return new List<TestHistory>();
			}
		}
        /// <summary>
        /// Chuyển đổi loại câu hỏi từ chuỗi sang enum.
        /// </summary>
        /// <param name="questionType">Chuỗi loại câu hỏi</param>
        /// <returns>Enum QuestionType tương ứng</returns>
        private QuestionType MapQuestionType(string questionType)
		{
			return questionType switch
			{
				"MULTIPLE_CHOICE_ONE" => QuestionType.MultipleChoice,
				"MULTIPLE_CHOICE_MANY" => QuestionType.MultipleChoice,
				"FILL_BLANK" => QuestionType.GapFilling,
				"FILL-IN-THE-BLANK" => QuestionType.GapFilling,
				"TRUE_FALSE" => QuestionType.TrueFalseNotGiven,
				"YES_NO_NOT_GIVEN" => QuestionType.YesNoNotGiven,
				"MATCHING_HEADING" => QuestionType.GapFilling,
				_ => throw new ArgumentException($"Unknown question type: {questionType}")
			};
		}
        /// <summary>
        /// Chuyển đổi loại câu hỏi từ enum sang chuỗi cho API.
        /// </summary>
        /// <param name="questionType">Enum QuestionType</param>
        /// <returns>Chuỗi loại câu hỏi cho API</returns>
        private string MapQuestionTypeForApi(QuestionType questionType)
		{
			return questionType switch
			{
				QuestionType.MultipleChoice => "MULTIPLE_CHOICE",
				QuestionType.GapFilling => "FILL_IN_THE_BLANK",
				QuestionType.TrueFalseNotGiven => "TRUE_FALSE",
				QuestionType.YesNoNotGiven => "YES_NO",
				_ => throw new ArgumentException($"Unknown question type: {questionType}")
			};
		}
        /// <summary>
        /// Lấy chi tiết câu trả lời từ API.
        /// </summary>
        /// <param name="answerId">ID của câu trả lời</param>
        /// <returns>Đối tượng AnswerResultModel chứa chi tiết câu trả lời</returns>
        public async Task<AnswerResultModel> GetAnswerDetailAsync(string answerId)
		{
			string apiUrl = $"/v1/answers/{answerId}";

			HttpResponseMessage response = await _clientCaller.GetAsync(apiUrl);

			if (response.IsSuccessStatusCode)
			{
				string content = await response.Content.ReadAsStringAsync();

				System.Diagnostics.Debug.WriteLine($"API Response Content: {content}");

				var apiResponse = JsonConvert.DeserializeObject<ApiResponseModel<AnswerResultModel>>(content);

				if (apiResponse != null && apiResponse.Code == 0)
				{
					System.Diagnostics.Debug.WriteLine($"Parsed Data: Id={apiResponse.Data.Id}, QuizId={apiResponse.Data.QuizId}");
					return apiResponse.Data;
				}
				else
				{
					throw new Exception($"API Error: {apiResponse?.Message}");
				}
			}
			throw new Exception($"Failed to fetch answer details: {response.ReasonPhrase}");
		}


	}
}

