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
	/// Model quản lý phân trang cho danh sách bài test
	/// </summary>
	/// <remarks>
	/// Quản lý:
	/// - Danh sách items trên trang hiện tại
	/// - Thông tin về trang hiện tại và tổng số trang
	/// - Số lượng items trên mỗi trang
	/// </remarks>
	public class PaginatedItemsModels : INotifyPropertyChanged
	{
		private ObservableCollection<ReadingItemModels> _currentPageItems;
		private int _currentPage;
		private int _totalPages;
		private int _itemsPerPage;

		public ObservableCollection<ReadingItemModels> CurrentPageItems
		{
			get => _currentPageItems;
			set
			{
				_currentPageItems = value;
				OnPropertyChanged();
			}
		}

		public int CurrentPage
		{
			get => _currentPage;
			set
			{
				_currentPage = value;
				OnPropertyChanged();
			}
		}

		public int TotalPages
		{
			get => _totalPages;
			set
			{
				_totalPages = value;
				OnPropertyChanged();
			}
		}

		public int ItemsPerPage
		{
			get => _itemsPerPage;
			set
			{
				_itemsPerPage = value;
				OnPropertyChanged();
			}
		}

		// Use ReadingItemModels directly for API items
		public List<ReadingItemModels> Items { get; set; }

		public PaginatedItemsModels(List<ReadingItemModels> readingItemModels)
		{
			CurrentPageItems = new ObservableCollection<ReadingItemModels>();
			CurrentPage = 1;
		}

		public PaginatedItemsModels(IEnumerable<ReadingItemModels> items, int currentPage, int totalPages, int itemsPerPage)
		{
			CurrentPageItems = new ObservableCollection<ReadingItemModels>(items);
			Items = new List<ReadingItemModels>(items);
			CurrentPage = currentPage;
			TotalPages = totalPages;
			ItemsPerPage = itemsPerPage;
		}

		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged([CallerMemberName] string propertyName = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}

}
