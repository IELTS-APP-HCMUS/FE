using login_full.Services;
using login_full.Models;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.UI.Xaml.Controls;
using Windows.Storage;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using login_full.Helpers;

namespace login_full.ViewModels
{
    public class ReadingItemsViewModel : ObservableObject
    {
        private readonly IReadingItemsService _readingItemsService;
        private readonly INavigationService _navigationService;
        private ISearchService _searchService;
        private readonly IPaginationService _paginationService;
        private readonly ICompletedItemsService _completedItemsService;

        // Constants
        private const double COLLAPSED_SIDEBAR_WIDTH = 50;
        private const double EXPANDED_SIDEBAR_WIDTH = 250;
        private const double COLLAPSED_ITEM_WIDTH = 200;
        private const double EXPANDED_ITEM_WIDTH = 300;
        private const double MIN_ITEM_WIDTH = 250;

        // Private fields
        private bool _isFilterExpanded;
        private double _sidebarWidth = COLLAPSED_SIDEBAR_WIDTH;
        private double _itemWidth = COLLAPSED_ITEM_WIDTH;
        private Visibility _filterTextVisibility = Visibility.Collapsed;
        private Visibility _filterContentVisibility = Visibility.Collapsed;
        private bool _showingCompletedItems;
        private bool _showingUncompletedItems = true;
        private double _windowWidth;
        private double _windowHeight;
        private bool _isCompletedFilterActive;
        private bool _isLoading;
        private readonly FilteredSelection _filteredSelection;
        private readonly LoaderManager _loaderManager;

        public ObservableCollection<ReadingItemModels> _items { get; private set; }
		private ObservableCollection<ReadingItemModels> _allItems;
		public ObservableCollection<ReadingItemModels> Items
		{
			get => _items;
			private set
			{
				_items = value;
				OnPropertyChanged(nameof(Items)); // Notify UI when the collection changes
			}
		}
		public ObservableCollection<ReadingItemModels> DisplayedItems => _paginationService.State.CurrentPageItems;

        // Properties
        public bool IsFilterExpanded
        {
            get => _isFilterExpanded;
            set
            {
                SetProperty(ref _isFilterExpanded, value);
                UpdateLayout();
            }
        }

        public double SidebarWidth
        {
            get => _sidebarWidth;
            set => SetProperty(ref _sidebarWidth, value);
        }

        public double ItemWidth
        {
            get => _itemWidth;
            set => SetProperty(ref _itemWidth, value);
        }

        public Visibility FilterTextVisibility
        {
            get => _filterTextVisibility;
            set => SetProperty(ref _filterTextVisibility, value);
        }

        public Visibility FilterContentVisibility
        {
            get => _filterContentVisibility;
            set => SetProperty(ref _filterContentVisibility, value);
        }

        public bool ShowingCompletedItems
        {
            get => _showingCompletedItems;
            set => SetProperty(ref _showingCompletedItems, value);
        }

        public bool ShowingUncompletedItems
        {
            get => _showingUncompletedItems;
            set => SetProperty(ref _showingUncompletedItems, value);
        }

        public bool IsCompletedFilterActive
        {
            get => _isCompletedFilterActive;
            set => SetProperty(ref _isCompletedFilterActive, value);
        }

        public bool IsLoading
        {
            get => _isLoading;
            set => SetProperty(ref _isLoading, value);
        }

		private int? _selectedPassageTag;
		private int? _selectedQuestionTypeTag;

		public int? SelectedPassageTag
		{
			get => _selectedPassageTag;
			set => SetProperty(ref _selectedPassageTag, value);
		}

		public int? SelectedQuestionTypeTag
		{
			get => _selectedQuestionTypeTag;
			set => SetProperty(ref _selectedQuestionTypeTag, value);
		}


		// Commands
		public IAsyncRelayCommand LoadItemsCommand { get; }
        public IAsyncRelayCommand<AutoSuggestBox> SearchCommand { get; }
        public IRelayCommand<AutoSuggestBox> ClearSearchCommand { get; }
        public IRelayCommand ToggleFilterCommand { get; }
        public IRelayCommand<bool> FilterCommand { get; }
        public IAsyncRelayCommand<ReadingItemModels> StartTestCommand { get; }
        public IAsyncRelayCommand NavigateHomeCommand { get; }
        public IAsyncRelayCommand AboutUsCommand { get; }
        public IAsyncRelayCommand LogoutCommand { get; }
        public IRelayCommand NextPageCommand { get; }
        public IRelayCommand PreviousPageCommand { get; }
        public IRelayCommand<int> GoToPageCommand { get; }

		public IAsyncRelayCommand ApplyFilterCommand { get; }


		public ISearchService SearchService
		{
			get => _searchService;
			set
			{
				if (value != _searchService)
				{
					_searchService.SearchResultsUpdated -= OnSearchResultsUpdated; // Unsubscribe old service events
					_searchService = value;
					_searchService.SearchResultsUpdated += OnSearchResultsUpdated; // Subscribe new service events
				}
			}
		}

        public IPaginationService PaginationService => _paginationService;

        public bool CanGoToNextPage => _paginationService.State.CurrentPage < _paginationService.State.TotalPages;
        public bool CanGoToPreviousPage => _paginationService.State.CurrentPage > 1;

        private int _currentPage = 1;
        public int CurrentPage
        {
            get => _currentPage;
            set => SetProperty(ref _currentPage, value);
        }

        public ObservableCollection<int> PageNumbers { get; private set; }

		public ReadingItemsViewModel(
            IReadingItemsService readingItemsService,
            INavigationService navigationService,
            ISearchService searchService,
            IPaginationService paginationService,
            ICompletedItemsService completedItemsService,
            LoaderManager loaderManager
            )
        {
            _readingItemsService = readingItemsService;
            _navigationService = navigationService;
            _searchService = searchService;
            _paginationService = paginationService;
            _completedItemsService = completedItemsService;

            // Initialize Commands
            LoadItemsCommand = new AsyncRelayCommand(LoadItemsAsync);
            SearchCommand = new AsyncRelayCommand<AutoSuggestBox>(HandleSearchAsync);
            ClearSearchCommand = new RelayCommand<AutoSuggestBox>(ClearSearch);
            ToggleFilterCommand = new RelayCommand(ToggleFilter);
            FilterCommand = new RelayCommand<bool>(FilterItems);
            StartTestCommand = new AsyncRelayCommand<ReadingItemModels>(StartTestAsync);
            NavigateHomeCommand = new AsyncRelayCommand(NavigateHomeAsync);
            AboutUsCommand = new AsyncRelayCommand(NavigateToAboutUsAsync);
            LogoutCommand = new AsyncRelayCommand(LogoutAsync);
            NextPageCommand = new RelayCommand(NextPage);
            PreviousPageCommand = new RelayCommand(PreviousPage);
            GoToPageCommand = new RelayCommand<int>(GoToPage);
			ApplyFilterCommand = new AsyncRelayCommand<string>(ApplyFilterAsync);

			// Initialize collections
			Items = new ObservableCollection<ReadingItemModels>();
            PageNumbers = new ObservableCollection<int>();
            UpdatePageNumbers();

            _filteredSelection = new FilteredSelection
            {
                Passages = new Dictionary<int, bool>()
                {
                    {1, false},
                    {2, false},
                    {3, false}
                },
                QuestionTypes = new Dictionary<string, bool>()
                {
                    {"FILL_BLANK", false},
                    {"MATCHING_HEADING", false},
                    {"TRUE_FALSE", false},
                    {"YES_NO", false}
                }
            };
            _loaderManager = loaderManager;

            // Subscribe to search service events
            _searchService.SearchResultsUpdated += OnSearchResultsUpdated;

            // Khởi tạo trang đầu tiên là 1
            CurrentPage = 1;
        }

        private void OnSearchResultsUpdated(object sender, IEnumerable<ReadingItemModels> results)
        {
            var filteredResults = ShowingCompletedItems
                ? results.Where(i => i.IsSubmitted)
                : results.Where(i => !i.IsSubmitted);

            _paginationService.UpdateItems(filteredResults);
            OnPropertyChanged(nameof(DisplayedItems));
        }

		private Task ApplyFilterAsync(string parameter)
        {
            try
			{
				IsLoading = true;

				var param = parameter.Split('_');
				var filterType = param[0];  
				var filterValue = string.Join("_", param.Skip(1)); 

				System.Diagnostics.Debug.WriteLine($"FilterType: {filterType}, FilterValue: {filterValue}");

                // Dùng cache gốc (_allItems) để lọc
                List<ReadingItemModels> filteredItems = new List<ReadingItemModels>();

				if (filterType.Equals("Passage", StringComparison.OrdinalIgnoreCase))
				{
                    //               if (_filteredSelection.Passages[int.Parse(param[2])])
                    //               {
                    //                   _filteredSelection.Passages[int.Parse(param[2])] = false;
                    //                   return Task.CompletedTask;
                    //               }
                    //filteredItems = _allItems.Where(item =>
                    //	item.Tags != null &&
                    //	item.Tags.Any(tag =>
                    //		tag.Code.Equals(filterValue, StringComparison.OrdinalIgnoreCase)
                    //	)
                    //).ToList();
                    filteredItems = _filteredSelection.Passages[int.Parse(param[2])] ? [.. _allItems] : _allItems.Where(item =>
                        item.Tags != null &&
                        item.Tags.Any(tag =>
                            tag.Code.Equals(filterValue, StringComparison.OrdinalIgnoreCase)
                        )
                    ).ToList();
                    bool temp = !_filteredSelection.Passages[int.Parse(param[2])];
                    _filteredSelection.Reset();
                    _filteredSelection.Passages[int.Parse(param[2])] = temp;
                }
				else if (filterType.Equals("QuestionType", StringComparison.OrdinalIgnoreCase))
				{
                    //if (_filteredSelection.QuestionTypes[filterValue])
                    //{
                    //    _filteredSelection.QuestionTypes[filterValue] = false;
                    //    filteredItems = [.. _allItems];
                    //}
                    //else
                    //{
                    //    _filteredSelection.QuestionTypes[filterValue] = true;
                    //    filteredItems = _allItems.Where(item =>
                    //        item.Tags != null &&
                    //        item.Tags.Any(tag =>
                    //            tag.Code.Equals(filterValue, StringComparison.OrdinalIgnoreCase)
                    //        )
                    //    ).ToList();
                    //}
                    filteredItems = _filteredSelection.QuestionTypes[filterValue] ? [.. _allItems] : _allItems.Where(item =>
                        item.Tags != null &&
                        item.Tags.Any(tag =>
                            tag.Code.Equals(filterValue, StringComparison.OrdinalIgnoreCase)
                        )
                    ).ToList();
                    bool temp = !_filteredSelection.QuestionTypes[filterValue];
                    _filteredSelection.Reset();
                    _filteredSelection.QuestionTypes[filterValue] = temp;
				}
				ApplyLocalFilter(filterType, filterValue, filteredItems);
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Filter Error: {ex.Message}");
			}
			finally
			{
				IsLoading = false;
			}

            return Task.CompletedTask;
        }

        private void ApplyLocalFilter(string filterType, string filterValue, List<ReadingItemModels> filteredItems)
		{
			Items = new ObservableCollection<ReadingItemModels>(filteredItems);

			InitializePagination();

			System.Diagnostics.Debug.WriteLine($"Filter applied locally: {filterType} - {filterValue} | Total: {filteredItems.Count} items.");
		}


		public async Task LoadItemsAsync()
        {
			try
			{
                _loaderManager.ShowLoader();
                var items = await _readingItemsService.GetReadingItemsAsync();
                _loaderManager.HideLoader();
                Items = new ObservableCollection<ReadingItemModels>(items);
				_allItems = new ObservableCollection<ReadingItemModels>(items);
				InitializePagination();
			}
            finally
            {
                IsLoading = false;
            }
        }

        private void InitializePagination()
        {
            var filteredItems = ShowingCompletedItems
                ? Items.Where(i => i.IsSubmitted).ToList()
                : Items.Where(i => !i.IsSubmitted).ToList();

            _paginationService.UpdateItems(filteredItems);
            _paginationService.UpdateItemsPerPage(IsFilterExpanded);
            
            // Reset về trang 1 khi khởi tạo lại pagination
            CurrentPage = 1;
            _paginationService.GoToPage(1);
            
            OnPropertyChanged(nameof(DisplayedItems));
            UpdatePageNumbers();
        }

        private async Task HandleSearchAsync(AutoSuggestBox searchBox)
        {
            if (searchBox == null) return;
            await _searchService.HandleSearchQueryAsync(searchBox.Text);
        }

        private void ClearSearch(AutoSuggestBox searchBox)
        {
            if (searchBox == null) return;
            searchBox.Text = string.Empty;
            _searchService.ResetSearch();
            
            // Reset về trang 1 khi clear tìm kiếm
            CurrentPage = 1;
            InitializePagination();
        }

        private void ToggleFilter()
        {
            IsFilterExpanded = !IsFilterExpanded;
            SidebarWidth = IsFilterExpanded ? EXPANDED_SIDEBAR_WIDTH : COLLAPSED_SIDEBAR_WIDTH;
            FilterTextVisibility = IsFilterExpanded ? Visibility.Visible : Visibility.Collapsed;
            FilterContentVisibility = IsFilterExpanded ? Visibility.Visible : Visibility.Collapsed;
            UpdateLayout();
        }

        private void FilterItems(bool showCompleted)
        {
            ShowingCompletedItems = showCompleted;
            ShowingUncompletedItems = !showCompleted;
            _completedItemsService.ToggleCompletedItems();
            
            // Reset về trang 1 khi lọc items
            CurrentPage = 1;
            InitializePagination();
        }

        private void UpdateLayout()
        {
            double availableWidth = _windowWidth - SidebarWidth - 60;
            int desiredColumns = IsFilterExpanded ? 3 : 4;
            ItemWidth = Math.Min((availableWidth / desiredColumns) - 20,
                               IsFilterExpanded ? EXPANDED_ITEM_WIDTH : COLLAPSED_ITEM_WIDTH);
            ItemWidth = Math.Max(MIN_ITEM_WIDTH, ItemWidth);

            _paginationService.UpdateItemsPerPage(IsFilterExpanded);
            OnPropertyChanged(nameof(DisplayedItems));
        }

        private async Task StartTestAsync(ReadingItemModels item)
        {
            if (item != null)
            {
               // await _navigationService.NavigateToAsync(typeof(ReadingTestPage), item);
            }
        }

        private async Task NavigateHomeAsync()
        {
            await _navigationService.NavigateToAsync(typeof(HomePage));
        }

        private async Task NavigateToAboutUsAsync()
        {
            await _navigationService.NavigateToAsync(typeof(AboutUsPage));
        }

        private async Task LogoutAsync()
        {
            var localSettings = ApplicationData.Current.LocalSettings;
            localSettings.Values.Remove("Username");
            localSettings.Values.Remove("PasswordInBase64");
            localSettings.Values.Remove("EntropyInBase64");

            await _navigationService.NavigateToAsync(typeof(MainWindow));
        }

        private void NextPage()
        {
            _paginationService.NextPage();
            UpdatePageNumbers();
            OnPropertyChanged(nameof(CanGoToNextPage));
            OnPropertyChanged(nameof(CanGoToPreviousPage));
            OnPropertyChanged(nameof(DisplayedItems));
        }

        private void PreviousPage()
        {
            _paginationService.PreviousPage();
            UpdatePageNumbers();
            OnPropertyChanged(nameof(CanGoToNextPage));
            OnPropertyChanged(nameof(CanGoToPreviousPage));
            OnPropertyChanged(nameof(DisplayedItems));
        }

        private void GoToPage(int pageNumber)
        {
            if (pageNumber >= 1 && pageNumber <= _paginationService.State.TotalPages)
            {
                _paginationService.GoToPage(pageNumber);
                CurrentPage = pageNumber;
                UpdatePageNumbers();
                OnPropertyChanged(nameof(CanGoToNextPage));
                OnPropertyChanged(nameof(CanGoToPreviousPage));
                OnPropertyChanged(nameof(DisplayedItems));
            }
        }

        public void UpdateWindowSize(double width, double height)
        {
            _windowWidth = width;
            _windowHeight = height;
            UpdateLayout();
        }

        public void Cleanup()
        {
            // Unsubscribe from events
            _searchService.SearchResultsUpdated -= OnSearchResultsUpdated;
        }

        private void UpdatePageNumbers()
        {
            PageNumbers.Clear();
            foreach (var pageNum in _paginationService.VisiblePageNumbers)
            {
                PageNumbers.Add(pageNum);
            }
            OnPropertyChanged(nameof(PageNumbers));
        }

        
    }
}
class FilteredSelection
{
    public Dictionary<int, bool> Passages { get; set; }
    public Dictionary<string, bool> QuestionTypes { get; set; }
    public void Reset()
    {
        Passages = new Dictionary<int, bool>()
                {
                    {1, false},
                    {2, false},
                    {3, false}
                };
        QuestionTypes = new Dictionary<string, bool>()
                {
                    {"FILL_BLANK", false},
                    {"MATCHING_HEADING", false},
                    {"TRUE_FALSE", false},
                    {"YES_NO", false}
                };
    }
}