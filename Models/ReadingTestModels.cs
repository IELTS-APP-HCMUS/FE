using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Newtonsoft.Json;

namespace login_full.Models
{
	public class ReadingTestDetail
	{
		public string Id { get; set; }
		public string Title { get; set; }
		public string Content { get; set; }
		public int TimeLimit { get; set; }
		public List<ReadingTestQuestion> Questions { get; set; }
		public TestProgress Progress { get; set; }
	}

	public class ReadingTestQuestion : INotifyPropertyChanged
	{
		private bool _isExplanationVisible;
		private string _userInput;

		public string Id { get; set; }
		public QuestionType Type { get; set; }
		public string QuestionText { get; set; }

		// Danh sách các lựa chọn
		public List<string> Options { get; set; }
		public string CorrectAnswer { get; set; }
		public string UserAnswer { get; set; } // Câu trả lời của người dùng

		// Model cho từng option
		public ObservableCollection<QuestionOptionModel> OptionModels { get; set; }
		public string Explanation { get; set; }

		public bool IsAnswered => !string.IsNullOrEmpty(UserAnswer);
		public bool IsCorrectAnswer
		{
			get
			{
				if (Type == QuestionType.GapFilling)
				{
					return IsCorrectFillInTheBlank(UserAnswer, CorrectAnswer);
				}
				return UserAnswer == CorrectAnswer;
			}
		}

		private bool IsCorrectFillInTheBlank(string userAnswer, string correctAnswer)
		{
			return string.Equals(userAnswer?.Trim(), correctAnswer?.Trim(), StringComparison.OrdinalIgnoreCase);
		}


		public bool IsExplanationVisible
		{
			get => _isExplanationVisible;
			set
			{
				if (_isExplanationVisible != value)
				{
					_isExplanationVisible = value;
					OnPropertyChanged(nameof(IsExplanationVisible));
				}
			}
		}

		public string UserInput
		{
			get => _userInput;
			set
			{
				_userInput = value;
				OnPropertyChanged(nameof(UserInput));
				UserAnswer = value;
			}
		}

		// Khởi tạo danh sách OptionModels
		public void InitializeOptionModels()
		{
			if (Type == QuestionType.GapFilling)
			{
				OptionModels = null;
				return;
			}

			OptionModels = new ObservableCollection<QuestionOptionModel>();

			foreach (var option in Options)
			{
				var optionModel = new QuestionOptionModel
				{
					Text = option,
					IsSelected = option == UserAnswer,
					IsCorrect = option == CorrectAnswer,
					IsWrong = option == UserAnswer && option != CorrectAnswer
				};

				OptionModels.Add(optionModel);
			}
		}

		public void ToggleExplanation()
		{
			IsExplanationVisible = !IsExplanationVisible;
			System.Diagnostics.Debug.WriteLine($"ToggleExplanation called. New value: {IsExplanationVisible}");
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	public class QuestionOptionModel
	{
		public string Text { get; set; }
		public bool IsSelected { get; set; }
		public bool IsCorrect { get; set; }
		public bool IsWrong { get; set; }
	}

	public enum QuestionType
	{
		MultipleChoice,
		GapFilling,
		TrueFalseNotGiven,
		YesNoNotGiven
	}

	public class TestProgress
	{
		public int TotalQuestions { get; set; }
		public int AnsweredQuestions { get; set; }
		public int RemainingTime { get; set; }
		public bool IsCompleted { get; set; }
	}

	public class QuizDetailApiResponse
	{
		[JsonProperty("code")]
		public int Code { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("data")]
		public QuizData Data { get; set; }
	}

	public class QuizData
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("type")]
		public int Type { get; set; }

		[JsonProperty("mode")]
		public int Mode { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("status")]
		public string Status { get; set; }

		[JsonProperty("time")]
		public int Time { get; set; }

		[JsonProperty("content")]
		public string Content { get; set; }

		[JsonProperty("parts")]
		public List<QuizPart> Parts { get; set; }
	}

	public class QuizPart
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("quiz_id")]
		public int QuizId { get; set; }

		[JsonProperty("passage")]
		public int Passage { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("questions")]
		public List<Question> Questions { get; set; }
	}

	public class Question
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("quiz_id")]
		public int QuizId { get; set; }

		[JsonProperty("type")]
		public string Type { get; set; }

		[JsonProperty("question_type")]
		public string QuestionType { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("selection")]
		public List<SelectionOption> Selection { get; set; }

		[JsonProperty("explain")]
		public string Explain { get; set; }

		[JsonProperty("gap_fill_in_blank")]
		public string GapFillInBlankCorrectAnswer { get; set; }
	}

	public class SelectionOption
	{
		[JsonProperty("text")]
		public string Text { get; set; }

		[JsonProperty("correct")]
		public bool Correct { get; set; }
	}
}
