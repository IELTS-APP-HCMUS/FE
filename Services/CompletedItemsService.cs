using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    /// <summary>
    // Service quản lý các mục đã hoàn thành.
    // Cung cấp các chức năng hiển thị và lọc các mục đọc đã hoàn thành.
    // </summary>
    public class CompletedItemsService : ICompletedItemsService
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
        /// Trạng thái hiển thị chỉ các mục đã hoàn thành.
        /// </summary>
        private bool _showingCompletedOnly;

        public bool IsShowingCompletedOnly => _showingCompletedOnly;

        public event EventHandler<List<ReadingItemModels>> DisplayListUpdated;
        /// <summary>
        /// Khởi tạo một instance mới của <see cref="CompletedItemsService"/>.
        /// </summary>
        /// <param name="items">Danh sách các mục đọc</param>
        /// <param name="paginationService">Service phân trang</param>
        public CompletedItemsService(
            List<ReadingItemModels> items,
            IPaginationService paginationService)
        {
            _allItems = items ?? new List<ReadingItemModels>();
            _paginationService = paginationService;
            _showingCompletedOnly = false;
        }
        /// <summary>
        /// Chuyển đổi trạng thái hiển thị chỉ các mục đã hoàn thành.
        /// </summary>
        public void ToggleCompletedItems()
        {
            _showingCompletedOnly = !_showingCompletedOnly;
            UpdateDisplayList();
        }
        /// <summary>
        /// Cập nhật danh sách hiển thị dựa trên trạng thái hiện tại.
        /// </summary>
        public async void UpdateDisplayList()
        {
            try
            {
                if (_allItems == null || !_allItems.Any())
                {
                    await _paginationService.UpdateItemsAsync(new List<ReadingItemModels>());
                    return;
                }

                var filteredItems = _showingCompletedOnly
                    ? _allItems.Where(item => item.IsSubmitted).ToList()
                    : _allItems.Where(item => !item.IsSubmitted).ToList();

                DisplayListUpdated?.Invoke(this, filteredItems);
                await _paginationService.UpdateItemsAsync(filteredItems);
            }
            catch (Exception ex)
            {
                // Log error
                System.Diagnostics.Debug.WriteLine($"Error in UpdateDisplayList: {ex.Message}");
            }
        }
    }
}
