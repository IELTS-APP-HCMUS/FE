using login_full.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace login_full.Services
{
    /// <summary>
    ///Service quản lý phân trang cho danh sách các mục đọc.
    // Cung cấp các chức năng cập nhật và điều hướng giữa các trang.
    // </summary>
    public class PaginationService : IPaginationService
    {
        /// <summary>
        /// Trạng thái phân trang hiện tại.
        /// </summary>
        public PaginatedItemsModels State { get; }
        /// <summary>
        /// Danh sách tất cả các mục đọc.
        /// </summary>
        private List<ReadingItemModels> _allItems;
        // Thêm property để lưu danh sách số trang hiển thị
        public List<int> VisiblePageNumbers { get; private set; } = new List<int>();
        /// <summary>
        /// Khởi tạo một instance mới của <see cref="PaginationService"/>.
        /// </summary>
        public PaginationService()
        {
            State = new PaginatedItemsModels(new List<ReadingItemModels>())
            {
                CurrentPageItems = new ObservableCollection<ReadingItemModels>(),
                CurrentPage = 1,
                ItemsPerPage = 8
            };
        }
        /// <summary>
        /// Cập nhật danh sách các mục đọc.
        /// </summary>
        /// <param name="items">Danh sách các mục đọc</param>
        public void UpdateItems(IEnumerable<ReadingItemModels> items)
        {
            _allItems = items.ToList();
            UpdatePaginationState();
            UpdateCurrentPageItems();
        }
        /// <summary>
        /// Cập nhật danh sách các mục đọc bất đồng bộ.
        /// </summary>
        /// <param name="items">Danh sách các mục đọc</param>
        /// <returns>Task hoàn thành việc cập nhật</returns>
        public async Task UpdateItemsAsync(IEnumerable<ReadingItemModels> items)
        {
            try
            {
                _allItems = items?.ToList() ?? new List<ReadingItemModels>();
                UpdatePaginationState();
                UpdateCurrentPageItems();
                await Task.CompletedTask;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateItemsAsync: {ex.Message}");
                throw;
            }
        }
        /// <summary>
        /// Cập nhật số lượng mục đọc trên mỗi trang.
        /// </summary>
        /// <param name="isSidebarExpanded">Trạng thái mở rộng của sidebar</param>
        public void UpdateItemsPerPage(bool isSidebarExpanded)
        {
            State.ItemsPerPage = isSidebarExpanded ? 6 : 8;
            UpdatePaginationState();
            UpdateCurrentPageItems();
        }
        /// <summary>
        /// Cập nhật trạng thái phân trang.
        /// </summary>
        private void UpdatePaginationState()
        {
            if (_allItems == null || !_allItems.Any())
            {
                State.TotalPages = 1;
                State.CurrentPage = 1;
                return;
            }

            State.TotalPages = (int)Math.Ceiling((double)_allItems.Count / State.ItemsPerPage);
            State.CurrentPage = Math.Min(State.CurrentPage, State.TotalPages);

            // Tính toán các số trang hiển thị
            VisiblePageNumbers = CalculateVisiblePages(State.CurrentPage, State.TotalPages);
        }
        /// <summary>
        /// Tính toán các số trang hiển thị.
        /// </summary>
        /// <param name="currentPage">Trang hiện tại</param>
        /// <param name="totalPages">Tổng số trang</param>
        /// <returns>Danh sách số trang hiển thị</returns>
        private List<int> CalculateVisiblePages(int currentPage, int totalPages)
        {
            var pages = new List<int>();
            const int maxVisiblePages = 5; // Số lượng nút trang hiển thị

            int start = Math.Max(1, currentPage - 2);
            int end = Math.Min(totalPages, start + maxVisiblePages - 1);

            // Điều chỉnh start nếu end đã ở cuối
            if (end == totalPages)
            {
                start = Math.Max(1, end - maxVisiblePages + 1);
            }

            for (int i = start; i <= end; i++)
            {
                pages.Add(i);
            }

            return pages;
        }
        /// <summary>
        /// Cập nhật danh sách các mục đọc trên trang hiện tại.
        /// </summary>
        private void UpdateCurrentPageItems()
        {
            try
            {
                if (_allItems == null || !_allItems.Any())
                {
                    State.CurrentPageItems.Clear();
                    return;
                }

                var itemsToShow = _allItems
                    .Skip((State.CurrentPage - 1) * State.ItemsPerPage)
                    .Take(State.ItemsPerPage)
                    .ToList();

                State.CurrentPageItems.Clear();
                foreach (var item in itemsToShow)
                {
                    State.CurrentPageItems.Add(item);
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdateCurrentPageItems: {ex.Message}");
            }
        }
        /// <summary>
        /// Chuyển đến trang tiếp theo.
        /// </summary>
        public void NextPage()
        {
            if (State.CurrentPage < State.TotalPages)
            {
                State.CurrentPage++;
                UpdateCurrentPageItems();
            }
        }

        public void PreviousPage()
        {
            if (State.CurrentPage > 1)
            {
                State.CurrentPage--;
                UpdateCurrentPageItems();
            }
        }

        public void GoToPage(int page)
        {
            if (page >= 1 && page <= State.TotalPages)
            {
                State.CurrentPage = page;
                UpdateCurrentPageItems();
            }
        }
    }
}