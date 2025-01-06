using login_full.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
	/// <summary>
	/// Định nghĩa giao diện dịch vụ phân trang cho danh sách mục đọc.
	/// </summary>
	public interface IPaginationService
    {
		/// <summary>
		/// Trạng thái hiện tại của phân trang.
		/// </summary>
		PaginatedItemsModels State { get; }

		/// <summary>
		/// Danh sách các số trang đang hiển thị.
		/// </summary>
		List<int> VisiblePageNumbers { get; }

		/// <summary>
		/// Cập nhật danh sách các mục hiển thị.
		/// </summary>
		/// <param name="items">Danh sách mục cần cập nhật.</param>
		void UpdateItems(IEnumerable<ReadingItemModels> items);

		/// <summary>
		/// Cập nhật số lượng mục hiển thị trên mỗi trang tùy thuộc vào trạng thái mở rộng của sidebar.
		/// </summary>
		/// <param name="isSidebarExpanded">Trạng thái mở rộng của sidebar.</param>
		void UpdateItemsPerPage(bool isSidebarExpanded);

		/// <summary>
		/// Chuyển đến trang kế tiếp.
		/// </summary>
		void NextPage();

		/// <summary>
		/// Quay về trang trước đó.
		/// </summary>
		void PreviousPage();

		/// <summary>
		/// Đi đến trang đó.
		/// </summary>
		void GoToPage(int page);

		/// <summary>
		/// Cập nhật danh sách các mục hiển thị một cách bất đồng bộ.
		/// </summary>
		/// <param name="items">Danh sách mục cần cập nhật.</param>
		/// <returns>Trả về một tác vụ bất đồng bộ (Task).</returns>
		Task UpdateItemsAsync(IEnumerable<ReadingItemModels> items);
    }
}
