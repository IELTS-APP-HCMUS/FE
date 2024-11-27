﻿using login_full.Services;
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

namespace login_full.ViewModels
{
    public class ReadingItemsViewModel : ObservableObject
    {
        private readonly IReadingItemsService _readingItemsService;
        private readonly INavigationService _navigationService;
        private readonly ISearchService _searchService;
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

        public ObservableCollection<ReadingItemModels> Items { get; private set; }
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

        public ISearchService SearchService => _searchService;



        public ReadingItemsViewModel(
            IReadingItemsService readingItemsService,
            INavigationService navigationService,
            ISearchService searchService,
            IPaginationService paginationService,
            ICompletedItemsService completedItemsService)
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

            // Initialize collections
            Items = new ObservableCollection<ReadingItemModels>();

            // Subscribe to search service events
            _searchService.SearchResultsUpdated += OnSearchResultsUpdated;
        }

        private void OnSearchResultsUpdated(object sender, IEnumerable<ReadingItemModels> results)
        {
            var filteredResults = ShowingCompletedItems
                ? results.Where(i => i.IsCompleted)
                : results.Where(i => !i.IsCompleted);

            _paginationService.UpdateItems(filteredResults);
            OnPropertyChanged(nameof(DisplayedItems));
        }

        private async Task LoadItemsAsync()
        {
            var items = await _readingItemsService.GetReadingItemsAsync();
            Items = new ObservableCollection<ReadingItemModels>(items);
            InitializePagination();
        }

        private void InitializePagination()
        {
            var filteredItems = ShowingCompletedItems
                ? Items.Where(i => i.IsCompleted).ToList()
                : Items.Where(i => !i.IsCompleted).ToList();

            _paginationService.UpdateItems(filteredItems);
            _paginationService.UpdateItemsPerPage(IsFilterExpanded);
            OnPropertyChanged(nameof(DisplayedItems));
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
            OnPropertyChanged(nameof(DisplayedItems));
        }

        private void PreviousPage()
        {
            _paginationService.PreviousPage();
            OnPropertyChanged(nameof(DisplayedItems));
        }

        private void GoToPage(int pageNumber)
        {
            _paginationService.GoToPage(pageNumber);
            OnPropertyChanged(nameof(DisplayedItems));
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

        public ObservableCollection<int> PageNumbers { get; private set; }

        private void UpdatePageNumbers()
        {
            PageNumbers.Clear();
            for (int i = 1; i <= _paginationService.State.TotalPages; i++)
            {
                PageNumbers.Add(i);
            }
            OnPropertyChanged(nameof(PageNumbers));
        }

        
    }
}