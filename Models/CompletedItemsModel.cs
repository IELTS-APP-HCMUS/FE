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

	/// <summary>
	/// Model quản lý danh sách các bài test đã hoàn thành
	/// </summary>
	/// <remarks>
	/// Cung cấp:
	/// - Danh sách các bài test
	/// - Trạng thái hiển thị (chỉ hiện bài đã hoàn thành hoặc tất cả)
	/// </remarks>
	public class CompletedItemsModel : INotifyPropertyChanged
    {
        private bool _showingCompletedOnly;
        private ObservableCollection<ReadingItemModels> _items;

		/// <summary>
		/// Trạng thái hiển thị chỉ các bài đã hoàn thành
		/// </summary>
		///
		public bool ShowingCompletedOnly
        {
            get => _showingCompletedOnly;
            set
            {
                _showingCompletedOnly = value;
                OnPropertyChanged();
            }
        }
		/// <summary>
		/// Danh sách các bài reading test
		/// </summary>
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
