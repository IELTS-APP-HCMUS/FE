using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public class CompletedItemsService : ICompletedItemsService
    {
        private readonly List<ReadingItemModels> _allItems;
        private readonly IPaginationService _paginationService;
        private bool _showingCompletedOnly;

        public bool IsShowingCompletedOnly => _showingCompletedOnly;

        public event EventHandler<List<ReadingItemModels>> DisplayListUpdated;

        public CompletedItemsService(
            List<ReadingItemModels> items,
            IPaginationService paginationService)
        {
            _allItems = items ?? new List<ReadingItemModels>();
            _paginationService = paginationService;
            _showingCompletedOnly = false;
        }

        public void ToggleCompletedItems()
        {
            _showingCompletedOnly = !_showingCompletedOnly;
            UpdateDisplayList();
        }

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
