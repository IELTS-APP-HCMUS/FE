using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public interface ISearchService
    {
        Task<IEnumerable<ReadingItemModels>> GetSuggestionsAsync(string query);
        Task<IEnumerable<ReadingItemModels>> SearchAsync(string query);
        Task HandleSearchQueryAsync(string query, bool isFromSuggestion = false);
        void ResetSearch();
        event EventHandler<IEnumerable<ReadingItemModels>> SearchResultsUpdated;
    }
}
