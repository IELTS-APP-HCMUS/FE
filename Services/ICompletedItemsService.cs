using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
	/// <summary>
	/// Định nghĩa giao diện dịch vụ cho các mục đã hoàn thành.
	/// </summary>
	public interface ICompletedItemsService
    {
		/// <summary>
		/// Xác định trạng thái hiển thị chỉ các mục đã hoàn thành.
		/// </summary>
		bool IsShowingCompletedOnly { get; }

		/// <summary>
		/// Chuyển đổi giữa chế độ hiển thị tất cả và chỉ các mục đã hoàn thành.
		/// </summary>
		void ToggleCompletedItems();

		/// <summary>
		/// Cập nhật danh sách hiển thị dựa trên trạng thái hiện tại.
		/// </summary>
		void UpdateDisplayList();

		/// <summary>
		/// Sự kiện kích hoạt khi danh sách hiển thị được cập nhật.
		/// </summary>
		event EventHandler<List<ReadingItemModels>> DisplayListUpdated;
    }
}
