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
	/// Định nghĩa giao diện dịch vụ quản lý các mục đọc.
	/// </summary>
	public interface IReadingItemsService
    {

		//public void InitializeItems();
		/// <summary>
		/// Lấy danh sách tất cả các mục đọc một cách bất đồng bộ.
		/// </summary>
		/// <returns>Danh sách mục đọc dưới dạng ObservableCollection.</returns>
		Task<ObservableCollection<ReadingItemModels>> GetReadingItemsAsync();

		/// <summary>
		/// Lấy danh sách các mục đọc đã hoàn thành một cách bất đồng bộ.
		/// </summary>
		/// <returns>Danh sách các mục đã hoàn thành dưới dạng ObservableCollection.</returns>
		Task<ObservableCollection<ReadingItemModels>> GetCompletedItemsAsync();

		/// <summary>
		/// Lấy danh sách các mục đọc đã hoàn thành một cách bất đồng bộ.
		/// </summary>
		/// <returns>Danh sách các mục đã hoàn thành dưới dạng ObservableCollection.</returns>
		Task<ObservableCollection<ReadingItemModels>> GetUncompletedItemsAsync();

		/// <summary>
		/// Tìm kiếm các mục đọc dựa trên từ khóa đầu vào.
		/// </summary>
		/// <param name="searchTerm">Từ khóa tìm kiếm.</param>
		/// <returns>Danh sách các mục khớp với từ khóa dưới dạng ObservableCollection.</returns>
		Task<ObservableCollection<ReadingItemModels>> SearchItemsAsync(string searchTerm);
    }
}
