using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public interface ICompletedItemsService
    {
        bool IsShowingCompletedOnly { get; }
        void ToggleCompletedItems();
        void UpdateDisplayList();
        event EventHandler<List<ReadingItemModels>> DisplayListUpdated;
    }
}
