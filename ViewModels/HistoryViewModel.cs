using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using login_full.Models;
using login_full.Services;
using login_full.Views;
using System.Windows.Input;

namespace login_full.ViewModels
{
    public class HistoryViewModel : ObservableObject
    {
        private readonly IReadingTestService _readingTestService;
        private readonly INavigationService _navigationService;
        private ObservableCollection<TestHistory> _testHistories;
        private ObservableCollection<TestHistory> _displayedHistories;
        private int _itemsPerPage = 10;
        private int _currentPage = 1;
        private int _totalPages;

        public ObservableCollection<TestHistory> TestHistories
        {
            get => _testHistories;
            set => SetProperty(ref _testHistories, value);
        }

        public ObservableCollection<TestHistory> DisplayedHistories
        {
            get => _displayedHistories;
            set => SetProperty(ref _displayedHistories, value);
        }

        public int ItemsPerPage
        {
            get => _itemsPerPage;
            set
            {
                if (SetProperty(ref _itemsPerPage, value))
                {
                    UpdatePagination();
                }
            }
        }

        public int CurrentPage
        {
            get => _currentPage;
            set
            {
                if (SetProperty(ref _currentPage, value))
                {
                    UpdatePagination();
                }
            }
        }

        public int TotalPages
        {
            get => _totalPages;
            set => SetProperty(ref _totalPages, value);
        }

        public string PageInfo => $"{CurrentPage}/{TotalPages}";

        public List<int> AvailableItemsPerPage => new List<int> { 5, 10, 15 };

        public IRelayCommand SortByNameCommand { get; }
        public IRelayCommand SortByTimeCommand { get; }
        public IRelayCommand NextPageCommand { get; }
        public IRelayCommand PreviousPageCommand { get; }

        public HistoryViewModel(IReadingTestService readingTestService, INavigationService navigationService)
        {
            _readingTestService = readingTestService;
            _navigationService = navigationService;
            SortByNameCommand = new RelayCommand(SortByName);
            SortByTimeCommand = new RelayCommand(SortByTime);
            NextPageCommand = new RelayCommand(NextPage, CanGoToNextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage, CanGoToPreviousPage);
            LoadTestHistories();
        }

        private async void LoadTestHistories()
        {
            await LoadTestHistoriesAsync();
        }

        private void SortByName()
        {
            var sorted = TestHistories.OrderBy(x => x.Title).ToList();
            TestHistories = new ObservableCollection<TestHistory>(sorted);
        }

        private void SortByTime()
        {
            var sorted = TestHistories.OrderBy(x => x.SubmitTime).ToList();
            TestHistories = new ObservableCollection<TestHistory>(sorted);
        }

        private void UpdatePagination()
        {
            if (_testHistories == null) return;

            TotalPages = (int)Math.Ceiling(_testHistories.Count / (double)ItemsPerPage);

            var skip = (CurrentPage - 1) * ItemsPerPage;
            var items = _testHistories.Skip(skip).Take(ItemsPerPage);
            DisplayedHistories = new ObservableCollection<TestHistory>(items);

            OnPropertyChanged(nameof(PageInfo));
            (NextPageCommand as RelayCommand)?.NotifyCanExecuteChanged();
            (PreviousPageCommand as RelayCommand)?.NotifyCanExecuteChanged();
        }

        private bool CanGoToNextPage() => CurrentPage < TotalPages;
        private bool CanGoToPreviousPage() => CurrentPage > 1;

        private void NextPage()
        {
            if (CanGoToNextPage())
            {
                CurrentPage++;
            }
        }

        private void PreviousPage()
        {
            if (CanGoToPreviousPage())
            {
                CurrentPage--;
            }
        }

        public async void RetakeTest(string testId)
        {
            await _navigationService.NavigateToAsync(typeof(ReadingTestPage), testId);
        }

		public async void ViewResult(string testId, string answerId)
		{
			// Truyền cả testId và answerId vào trang Xem lại
			await _navigationService.NavigateToAsync(typeof(TestDetailResultPage),
				new Dictionary<string, string>
				{
			{ "testId", testId },
			{ "answerId", answerId }
				});
		}

		public async void RefreshHistory()
        {
            await LoadTestHistoriesAsync();
        }

        private async Task LoadTestHistoriesAsync()
        {
            var histories = await _readingTestService.GetTestHistoryAsync();
            var testHistories = histories.Select(h =>
            {
                h.RetakeCommand = new RelayCommand(() => RetakeTest(h.TestId));
                h.ViewResultCommand = new RelayCommand(() => ViewResult(h.TestId, h.AnswerId));
                return h;
            });
            _testHistories = new ObservableCollection<TestHistory>(testHistories);
            UpdatePagination();
        }

    }
}