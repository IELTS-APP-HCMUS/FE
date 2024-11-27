using login_full.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public interface IReadingItemsService
    {

        //public void InitializeItems();

        Task<ObservableCollection<ReadingItemModels>> GetReadingItemsAsync();
        Task<ObservableCollection<ReadingItemModels>> GetCompletedItemsAsync();
        Task<ObservableCollection<ReadingItemModels>> GetUncompletedItemsAsync();
        Task<ObservableCollection<ReadingItemModels>> SearchItemsAsync(string searchTerm);
    }
}
