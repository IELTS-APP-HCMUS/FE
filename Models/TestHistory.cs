using System;
using System.Windows.Input;
using CommunityToolkit.Mvvm.Input;
using login_full.ViewModels;

namespace login_full.Models
{
	/// <summary>
	/// Model lưu trữ lịch sử làm bài test của người dùng
	/// </summary>
	/// <remarks>
	/// Bao gồm:
	/// - Thông tin cơ bản về bài test
	/// - Thời gian làm bài
	/// - Kết quả chi tiết (số câu đúng/sai/bỏ qua)
	/// - Commands để làm lại hoặc xem kết quả
	/// </remarks>
	public class TestHistory
    {
        
        public string AnswerId { get; set; }
        public string TestId { get; set; }
        public string Title { get; set; }
        public DateTime SubmitTime { get; set; }
        public TimeSpan Duration { get; set; }
        public int TotalQuestions { get; set; }
        public int CorrectAnswers { get; set; }
        public int WrongAnswers { get; set; }
        public int SkippedAnswers { get; set; }
        public double CorrectPercentage => TotalQuestions > 0 
            ? Math.Round((double)CorrectAnswers / TotalQuestions * 100, 1) 
            : 0;

        public ICommand RetakeCommand { get; set; }
        public ICommand ViewResultCommand { get; set; }
    }
}