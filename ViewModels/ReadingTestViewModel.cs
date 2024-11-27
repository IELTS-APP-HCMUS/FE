using login_full.Models;
using login_full.Services;
using Microsoft.UI.Xaml;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Xaml.Controls;
using login_full.Views;
using System.Collections.ObjectModel;



namespace login_full.ViewModels
{
    public class ReadingTestViewModel : INotifyPropertyChanged
    {

        //public HighlightViewModel HighlightVM { get; }
        //public ObservableCollection<HighlightModel> Highlights { get; } = new();

        //public IRelayCommand ToggleHighlightCommand { get; }
        //public IRelayCommand HighlightSelectedTextCommand { get; }
        //public IRelayCommand RemoveHighlightCommand { get; }


        private readonly IReadingTestService _testService;
        private ReadingTestDetail _testDetail;
        private DispatcherTimer _timer;

        // Xử lý navigation
        private readonly INavigationService _navigationService;

        public IRelayCommand SubmitCommand { get; }
        public IAsyncRelayCommand ExitCommand { get; }

        

        public IRelayCommand ZoomInCommand { get; }
        public IRelayCommand ZoomOutCommand { get; }
        public IRelayCommand HighlightCommand { get; }
        public IRelayCommand AddNoteCommand { get; }
        public IRelayCommand SaveProgressCommand { get; }

        public string FormattedTimeRemaining
        {
            get
            {
                if (TestDetail?.Progress == null) return "00:00";
                var timeSpan = TimeSpan.FromSeconds(TestDetail.Progress.RemainingTime);
                return $"{timeSpan.Minutes:D2}:{timeSpan.Seconds:D2}";
            }
        }

        public ReadingTestDetail TestDetail
        {
            get => _testDetail;
            private set
            {
                _testDetail = value;
                OnPropertyChanged();
            }
        }


        public ReadingTestViewModel(IReadingTestService testService, INavigationService navigationService/*, IHighlightService highlightService*/)
        {
            _testService = testService;
            _navigationService = navigationService;

            SubmitCommand = new RelayCommand(async () => await SubmitTest());

            _timer = new DispatcherTimer
            {
                Interval = TimeSpan.FromSeconds(1)
            };
            _timer.Tick += Timer_Tick;


       
            //HighlightVM = new HighlightViewModel(highlightService);

            ZoomInCommand = new RelayCommand(ZoomIn);
            ZoomOutCommand = new RelayCommand(ZoomOut);
            HighlightCommand = new RelayCommand(ToggleHighlight);
            AddNoteCommand = new RelayCommand(AddNote);
            SaveProgressCommand = new RelayCommand(SaveProgress);
            ExitCommand = new AsyncRelayCommand(ShowExitDialog);
        }

        private void ZoomIn() { /* Implementation */ }
        private void ZoomOut() { /* Implementation */ }
        private void ToggleHighlight() { /* Implementation */ }
        private void AddNote() { /* Implementation */ }
        private void SaveProgress() { /* Implementation */ }



        public async Task LoadTestAsync(string testId)
        {
            TestDetail = await _testService.GetTestDetailAsync(testId);

            TestDetail.Progress.RemainingTime = TestDetail.TimeLimit * 60;
            StartTimer();
        }

        private void StartTimer()
        {
            _timer.Start();
        }

        private void Timer_Tick(object sender, object e)
        {
            if (TestDetail.Progress.RemainingTime > 0)
            {
                TestDetail.Progress.RemainingTime--;
                OnPropertyChanged(nameof(FormattedTimeRemaining));
            }
            else
            {
                _timer.Stop();
                _= SubmitTest();
            }
        }

       

        private async Task SubmitTest()
        {
            _timer.Stop();

            var mainWindow = App.MainWindow;
            if (mainWindow == null) return;

            ContentDialog submitDialog = new ContentDialog
            {
                Title = "Xác nhận nộp bài",
                Content = "Bạn có chắc chắn muốn nộp bài không?",
                PrimaryButtonText = "Nộp bài",
                SecondaryButtonText = "Tiếp tục làm bài",
                DefaultButton = ContentDialogButton.Secondary,
                XamlRoot = mainWindow.Content.XamlRoot
            };

            var result = await submitDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                var success = await _testService.SubmitTestAsync(TestDetail.Id);
                if (success)
                {
                    TestDetail.Progress.IsCompleted = true;

                    // Tính thời gian làm bài
                    var timeSpent = TimeSpan.FromSeconds(TestDetail.TimeLimit * 60 - TestDetail.Progress.RemainingTime);


                    // Tạo TestResultViewModel và chuyển hướng
                    var resultViewModel = new TestResultViewModel(TestDetail, timeSpent, (App.Current as App).ChartService, _navigationService);
                    (App.Current as App).CurrentTestResult = resultViewModel;
                    await _navigationService.NavigateToAsync(typeof(TestResultPage));
                }
            }
            else
            {
                _timer.Start();
            }
        }
        private async Task ShowExitDialog()
        {
            var mainWindow = App.MainWindow;
            if (mainWindow == null)
            {
                throw new InvalidOperationException("MainWindow is not available.");
            }

            ContentDialog exitDialog = new ContentDialog
            {
                Title = "Xác nhận thoát",
                Content = "Bạn có muốn thoát khỏi trang làm bài hay không?",
                PrimaryButtonText = "Thoát",
                SecondaryButtonText = "Tiếp tục làm bài",
                DefaultButton = ContentDialogButton.Secondary,
                XamlRoot = mainWindow.Content.XamlRoot
            };

            var result = await exitDialog.ShowAsync();

            if (result == ContentDialogResult.Primary)
            {
                await _navigationService.NavigateToAsync(typeof(Views.reading_Item_UI));
            }
        }


        public event PropertyChangedEventHandler PropertyChanged;
        private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }
    }
}
