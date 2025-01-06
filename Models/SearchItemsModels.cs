using Microsoft.UI.Xaml.Controls;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Models
{
	/// <summary>
	/// Quản lý chức năng tìm kiếm cho reading items
	/// </summary>
	/// <remarks>
	/// Cung cấp các chức năng:
	/// - Tìm kiếm theo text
	/// - Gợi ý tìm kiếm
	/// - Reset tìm kiếm
	/// - Cập nhật kết quả tìm kiếm với phân trang
	/// </remarks>
	public class SearchManager 
    {
        private List<ReadingItemModels> _allItems;
        private PaginatedItemsModels _paginatedItems;
        private Action _updatePaginationNumbers;
        private Action _updateDisplayedItems;

        public SearchManager(List<ReadingItemModels> items, PaginatedItemsModels paginatedItems,
            Action updatePaginationNumbers, Action updateDisplayedItems)
        {
            _allItems = items;
            _paginatedItems = paginatedItems;
            _updatePaginationNumbers = updatePaginationNumbers;
            _updateDisplayedItems = updateDisplayedItems;
        }

        //public void HandleTextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        //{
        //    if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
        //    {
        //        var query = sender.Text.ToLower();

        //        if (string.IsNullOrEmpty(query))
        //        {
        //            sender.ItemsSource = null;
        //            ResetToOriginalList();
        //            return;
        //        }

        //        var suggestions = _allItems
        //            .Where(item =>
        //                item.Title.ToLower().Contains(query) ||
        //                item.Description.ToLower().Contains(query))
        //            .Take(5)
        //            .ToList();

        //        sender.ItemsSource = suggestions;
        //    }
        //}

        //public void HandleSuggestionChosen(AutoSuggestBox sender, AutoSuggestBoxSuggestionChosenEventArgs args)
        //{
        //    if (args.SelectedItem is ReadingItemModels selectedItem)
        //    {
        //        sender.Text = selectedItem.Title;
        //    }
        //}

        //public void HandleQuerySubmitted(AutoSuggestBox sender, AutoSuggestBoxQuerySubmittedEventArgs args)
        //{
        //    var query = sender.Text.ToLower();
        //    List<ReadingItemModels> searchResults;

        //    if (args.ChosenSuggestion != null && args.ChosenSuggestion is ReadingItemModels chosenItem)
        //    {
        //        searchResults = new List<ReadingItemModels> { chosenItem };
        //    }
        //    else
        //    {
        //        searchResults = _allItems
        //            .Where(item =>
        //                item.Title.ToLower().Contains(query) ||
        //                item.Description.ToLower().Contains(query))
        //            .ToList();
        //    }

        //    UpdateSearchResults(searchResults);
        //}

        //public void ResetSearch(AutoSuggestBox searchBox)
        //{
        //    searchBox.Text = string.Empty;
        //    ResetToOriginalList();
        //}

        //private void ResetToOriginalList()
        //{
        //    UpdateSearchResults(_allItems);
        //}

        //private void UpdateSearchResults(List<ReadingItemModels> searchResults)
        //{
        //    _paginatedItems = new PaginatedItemsModels(searchResults);
        //    _updatePaginationNumbers();
        //    _updateDisplayedItems();
        //}
    }
}
