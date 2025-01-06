using Microsoft.UI.Xaml.Controls;
using login_full.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;

namespace login_full.Services
{

    /// <summary>
    // Service quản lý tìm kiếm các mục đọc.
    // Cung cấp các chức năng tìm kiếm và gợi ý từ danh sách các mục đọc.
    // </summary>
    public class SearchService : ISearchService
    {

        /// <summary>
        /// Danh sách tất cả các mục đọc.
        /// </summary>
        private readonly List<ReadingItemModels> _allItems;
        /// <summary>
        /// Service phân trang.
        /// </summary>
        private readonly IPaginationService _paginationService;
        /// <summary>
        /// Sự kiện được kích hoạt khi kết quả tìm kiếm được cập nhật.
        /// </summary>
        private Task<ObservableCollection<ReadingItemModels>> readingItems;
		private PaginationService paginationService;

		public event EventHandler<IEnumerable<ReadingItemModels>> SearchResultsUpdated;
        /// <summary>
        /// Khởi tạo một instance mới của <see cref="SearchService"/>.
        /// </summary>
        /// <param name="items">Danh sách các mục đọc</param>
        /// <param name="paginationService">Service phân trang</param>
        public SearchService(
            IEnumerable<ReadingItemModels> items,
            IPaginationService paginationService)
        {
            _allItems = items.ToList();
            _paginationService = paginationService;
        }

		public SearchService(Task<ObservableCollection<ReadingItemModels>> readingItems, PaginationService paginationService)
		{
			this.readingItems = readingItems;
			this.paginationService = paginationService;
		}
        /// <summary>
        /// Lấy danh sách gợi ý tìm kiếm dựa trên từ khóa.
        /// </summary>
        /// <param name="query">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách các mục đọc phù hợp với từ khóa</returns>
        public async Task<IEnumerable<ReadingItemModels>> GetSuggestionsAsync(string query)
        {
            await Task.Delay(50);
            if (string.IsNullOrEmpty(query))
                return Array.Empty<ReadingItemModels>();

            query = query.ToLower();
            var suggestions = _allItems
                .Where(item =>
                    item.Title.ToLower().Contains(query) ||
                    item.Description.ToLower().Contains(query) ||
                    item.Category.ToLower().Contains(query) ||
                    item.Difficulty.ToLower().Contains(query))
                .Take(15)
                .ToList();

            return suggestions;
        }
        /// <summary>
        /// Tìm kiếm các mục đọc dựa trên từ khóa.
        /// </summary>
        /// <param name="query">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách các mục đọc phù hợp với từ khóa</returns>
        public async Task<IEnumerable<ReadingItemModels>> SearchAsync(string query)
        {
            await Task.Delay(50);
            if (string.IsNullOrEmpty(query))
                return _allItems;

            return _allItems
                .Where(item =>
                    item.Title.Contains(query, StringComparison.OrdinalIgnoreCase) ||
                    item.Description.Contains(query, StringComparison.OrdinalIgnoreCase));
        }
        /// <summary>
        /// Xử lý truy vấn tìm kiếm và cập nhật kết quả.
        /// </summary>
        /// <param name="query">Từ khóa tìm kiếm</param>
        /// <param name="isFromSuggestion">Có phải từ gợi ý không</param>
        public async Task HandleSearchQueryAsync(string query, bool isFromSuggestion = false)
        {
            var results = await SearchAsync(query);
            SearchResultsUpdated?.Invoke(this, results);
            await _paginationService.UpdateItemsAsync(results);
        }
        /// <summary>
        /// Đặt lại kết quả tìm kiếm về danh sách ban đầu.
        /// </summary>
        public void ResetSearch()
        {
            SearchResultsUpdated?.Invoke(this, _allItems);
            _paginationService.UpdateItemsAsync(_allItems);
        }
    }
}