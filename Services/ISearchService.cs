using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
	/// <summary>
	/// Định nghĩa giao diện dịch vụ tìm kiếm trong danh sách các mục đọc.
	/// </summary>
	public interface ISearchService
    {
		/// <summary>
		/// Đưa ra các gợi ý tìm kiếm dựa trên truy vấn đầu vào.
		/// </summary>
		/// <param name="query">Truy vấn tìm kiếm.</param>
		/// <returns>Danh sách các mục đọc phù hợp.</returns>
		Task<IEnumerable<ReadingItemModels>> GetSuggestionsAsync(string query);

		/// <summary>
		/// Tìm kiếm các mục đọc dựa trên truy vấn đầu vào.
		/// </summary>
		/// <param name="query">Truy vấn tìm kiếm.</param>
		/// <returns>Danh sách kết quả tìm kiếm.</returns>
		Task<IEnumerable<ReadingItemModels>> SearchAsync(string query);

		/// <summary>
		/// Xử lý truy vấn tìm kiếm, bao gồm cả gợi ý và kết quả tìm kiếm chính thức.
		/// </summary>
		/// <param name="query">Truy vấn tìm kiếm.</param>
		/// <param name="isFromSuggestion">Xác định truy vấn có đến từ gợi ý hay không.</param>
		Task HandleSearchQueryAsync(string query, bool isFromSuggestion = false);

		/// <summary>
		/// Đặt lại trạng thái tìm kiếm.
		/// </summary>
		void ResetSearch();

		/// <summary>
		/// Sự kiện thông báo khi kết quả tìm kiếm được cập nhật.
		/// </summary>
		event EventHandler<IEnumerable<ReadingItemModels>> SearchResultsUpdated;
    }
}
