using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{

	/// <summary>
	/// Định nghĩa giao diện dịch vụ quản lý bài kiểm tra đọc.
	/// </summary>
	public interface IReadingTestService
    {
		/// <summary>
		/// Lấy chi tiết bài kiểm tra dựa trên ID bài kiểm tra.
		/// </summary>
		/// <param name="testId">ID của bài kiểm tra.</param>
		/// <returns>Chi tiết bài kiểm tra.</returns>
		Task<ReadingTestDetail> GetTestDetailAsync(string testId);

		/// <summary>
		/// Lưu câu trả lời cho một câu hỏi cụ thể.
		/// </summary>
		/// <param name="testId">ID của bài kiểm tra.</param>
		/// <param name="questionId">ID của câu hỏi.</param>
		/// <param name="answer">Câu trả lời được chọn.</param>
		/// <returns>Trả về một tác vụ bất đồng bộ.</returns>
		Task SaveAnswerAsync(string testId, string questionId, string answer);

		/// <summary>
		/// Nộp bài kiểm tra và trả về kết quả.
		/// </summary>
		/// <param name="testId">ID của bài kiểm tra.</param>
		/// <returns>Chuỗi kết quả của bài kiểm tra.</returns>
		Task<string> SubmitTestAsync(string testId);

		/// <summary>
		/// Cập nhật trạng thái hoàn thành của bài kiểm tra.
		/// </summary>
		/// <param name="testId">ID của bài kiểm tra.</param>
		/// <param name="isCompleted">Trạng thái hoàn thành.</param>
		/// <returns>Trả về true nếu cập nhật thành công.</returns>

		Task<bool> UpdateTestCompletionStatus(string testId, bool isCompleted);

		/// <summary>
		/// Lấy lịch sử các bài kiểm tra đã thực hiện.
		/// </summary>
		/// <returns>Danh sách lịch sử bài kiểm tra.</returns>
		Task<List<TestHistory>> GetTestHistoryAsync();

		/// <summary>
		/// Lấy chi tiết câu trả lời dựa trên ID câu trả lời.
		/// </summary>
		/// <param name="answerId">ID của câu trả lời.</param>
		/// <returns>Chi tiết kết quả câu trả lời.</returns>
		Task<AnswerResultModel> GetAnswerDetailAsync(string answerId);
	}
}
