using login_full.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace login_full.Services
{
    public class PaginationService : IPaginationService
    {
        public PaginatedItemsModels State { get; }
        private List<ReadingItemModels> _allItems;
        // Thêm property để lưu danh sách số trang hiển thị
        public List<int> VisiblePageNumbers { get; private set; } = new List<int>();

        public PaginationService()
        {
            State = new PaginatedItemsModels(new List<ReadingItemModels>())
            {
                CurrentPageItems = new ObservableCollection<ReadingItemModels>(),
                CurrentPage = 1,
                ItemsPerPage = 8
            };
        }

        public void UpdateItems(IEnumerable<ReadingItemModels> items)
        {
            _allItems = items.ToList();
            UpdatePaginationState();
            UpdateCurrentPageItems();
        }

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

        public void UpdateItemsPerPage(bool isSidebarExpanded)
        {
            State.ItemsPerPage = isSidebarExpanded ? 6 : 8;
            UpdatePaginationState();
            UpdateCurrentPageItems();
        }

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
           // VisiblePageNumbers = CalculateVisiblePages(State.CurrentPage, State.TotalPages);
        }

        //private List<int> CalculateVisiblePages(int currentPage, int totalPages)
        //{
        //    var pages = new List<int>();
        //    const int maxVisiblePages = 5;

        //    int start = Math.Max(1, currentPage - 2);
        //    int end = Math.Min(totalPages, start + maxVisiblePages - 1);

        //    // Điều chỉnh start nếu end đã ở cuối
        //    if (end == totalPages)
        //    {
        //        start = Math.Max(1, end - maxVisiblePages + 1);
        //    }

        //    for (int i = start; i <= end; i++)
        //    {
        //        pages.Add(i);
        //    }

        //    return pages;
        //}

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