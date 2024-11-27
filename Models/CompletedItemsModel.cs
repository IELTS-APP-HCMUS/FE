using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Models
{


    public class CompletedItemsModel : INotifyPropertyChanged
    {
        private bool _showingCompletedOnly;
        private ObservableCollection<ReadingItemModels> _items;

        public bool ShowingCompletedOnly
        {
            get => _showingCompletedOnly;
            set
            {
                _showingCompletedOnly = value;
                OnPropertyChanged();
            }
        }

        public ObservableCollection<ReadingItemModels> Items
        {
            get => _items;
            set
            {
                _items = value;
                OnPropertyChanged();
            }
        }

        public CompletedItemsModel()
        {
            Items = new ObservableCollection<ReadingItemModels>();
            ShowingCompletedOnly = false;
        }

        public event PropertyChangedEventHandler PropertyChanged;
        protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
