using login_full.Models;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public interface IPaginationService
    {
        PaginatedItemsModels State { get; }

      //  List<int> VisiblePageNumbers { get; }
        void UpdateItems(IEnumerable<ReadingItemModels> items);
        void UpdateItemsPerPage(bool isSidebarExpanded);
        void NextPage();
        void PreviousPage();
        void GoToPage(int page);
        Task UpdateItemsAsync(IEnumerable<ReadingItemModels> items);

    }
}
