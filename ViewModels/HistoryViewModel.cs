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

        public ObservableCollection<TestHistory> TestHistories
        {
            get => _testHistories;
            set => SetProperty(ref _testHistories, value);
        }

        public IRelayCommand SortByNameCommand { get; }

        public HistoryViewModel(IReadingTestService readingTestService, INavigationService navigationService)
        {
            _readingTestService = readingTestService;
            _navigationService = navigationService;
            SortByNameCommand = new RelayCommand(SortByName);
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

        public async void RetakeTest(string testId)
        {
            await _navigationService.NavigateToAsync(typeof(ReadingTestPage), testId);
        }

        public async void ViewResult(string testId)
        {
            await _navigationService.NavigateToAsync(typeof(TestDetailResultPage), testId);
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
                h.ViewResultCommand = new RelayCommand(() => ViewResult(h.TestId));
                return h;
            });
            TestHistories = new ObservableCollection<TestHistory>(testHistories);
        }

    }
} 