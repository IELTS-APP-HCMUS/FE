using login_full.Converters;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;

// API Response Model tổng quát

/// <summary>
/// Mô hình phản hồi từ API tổng quát.
/// </summary>
/// <typeparam name="T">Kiểu dữ liệu cho trường dữ liệu (data).</typeparam>
public class ApiResponseModel<T>
{
	[JsonProperty("code")]
	public int Code { get; set; }

	[JsonProperty("message")]
	public string Message { get; set; }

	[JsonProperty("data")]
	public T Data { get; set; }
}

// Dữ liệu chính từ API

/// <summary>
/// Mô hình dữ liệu chính từ API.
/// </summary>
public class AnswerResultModel
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("quiz")]
	public int QuizId { get; set; }

	[JsonProperty("detail")]
	//[JsonConverter(typeof(DetailConverter))] // Chuyển đổi đặc biệt cho Dictionary
	public Dictionary<string, List<AnswerDetailModel>> Detail { get; set; }

	[JsonProperty("summary")]
	public AnswerSummaryModel Summary { get; set; }

	[JsonProperty("type")]
	public int Type { get; set; }

	[JsonProperty("status")]
	public string Status { get; set; }

	[JsonProperty("note")]
	public string Note { get; set; }

	[JsonProperty("quiz_type")]
	public int QuizType { get; set; }

	[JsonProperty("completed_duration")]
	public int CompletedDuration { get; set; }

	[JsonProperty("questions")]
	public object Questions { get; set; } // Dữ liệu này là null trong response

	[JsonProperty("student")]
	public StudentModel Student { get; set; }

	[JsonProperty("quiz_detail")]
	public QuizDetailModel QuizDetail { get; set; }
}

// Chi tiết câu hỏi

/// <summary>
/// Chi tiết câu hỏi.
/// </summary>
public class AnswerDetailModel
{
	[JsonProperty("answer")]
	public AnswerTitleModel Answer { get; set; }

	[JsonProperty("type")]
	public string Type { get; set; }

	[JsonProperty("correct")]
	public bool Correct { get; set; }

	[JsonProperty("question")]
	public int Question { get; set; }

	[JsonProperty("id_question")]
	public int IdQuestion { get; set; }
}

// Câu trả lời
public class AnswerTitleModel
{
	[JsonProperty("title")]
	public List<string> Title { get; set; }
}

// Tóm tắt kết quả
public class AnswerSummaryModel
{
	[JsonProperty("correct")]
	public int Correct { get; set; }

	[JsonProperty("total")]
	public int Total { get; set; }

	[JsonProperty("left_time")]
	public string LeftTime { get; set; }

	[JsonProperty("mocktest_time")]
	public int MockTestTime { get; set; }

	[JsonProperty("type")]
	public string Type { get; set; }
}

// Thông tin sinh viên
public class StudentModel
{
	[JsonProperty("id")]
	public string Id { get; set; }

	[JsonProperty("first_name")]
	public string FirstName { get; set; }

	[JsonProperty("last_name")]
	public string LastName { get; set; }
}

// Thông tin bài kiểm tra
public class QuizDetailModel
{
	[JsonProperty("id")]
	public int Id { get; set; }

	[JsonProperty("title")]
	public string Title { get; set; }
}
