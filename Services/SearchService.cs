using Microsoft.UI.Xaml.Controls;
using login_full.Models;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System;
using System.Collections.ObjectModel;

namespace login_full.Services
{
    public class SearchService : ISearchService
    {
        private readonly List<ReadingItemModels> _allItems;
        private readonly IPaginationService _paginationService;
		private Task<ObservableCollection<ReadingItemModels>> readingItems;
		private PaginationService paginationService;

		public event EventHandler<IEnumerable<ReadingItemModels>> SearchResultsUpdated;

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

        public async Task HandleSearchQueryAsync(string query, bool isFromSuggestion = false)
        {
            var results = await SearchAsync(query);
            SearchResultsUpdated?.Invoke(this, results);
            await _paginationService.UpdateItemsAsync(results);
        }

        public void ResetSearch()
        {
            SearchResultsUpdated?.Invoke(this, _allItems);
            _paginationService.UpdateItemsAsync(_allItems);
        }
    }
}