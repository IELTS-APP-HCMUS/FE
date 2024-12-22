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


        private bool _isVocabMode;
        public bool IsVocabMode
        {
            get => _isVocabMode;
            set
            {
                _isVocabMode = value;
                OnPropertyChanged();
                // Trigger content reprocessing when mode changes
                ProcessContentCommand.Execute(null);
            }
        }

        // Add ProcessContentCommand
        public IRelayCommand ProcessContentCommand { get; }


        private bool _isHighlightMode;
        public bool IsHighlightMode
        {
            get => _isHighlightMode;
            set
            {
                _isHighlightMode = value;
                OnPropertyChanged();
            }
        }

        private List<HighlightInfo> _highlights;
        public List<HighlightInfo> Highlights
        {
            get => _highlights;
            set
            {
                _highlights = value;
                OnPropertyChanged();
            }
        }

        private readonly IPdfExportService _pdfExportService;

        public ReadingTestViewModel(IReadingTestService testService, INavigationService navigationService, IPdfExportService pdfExportService)
        {
            _testService = testService ?? throw new ArgumentNullException(nameof(testService));
            _navigationService = navigationService ?? throw new ArgumentNullException(nameof(navigationService));
            _pdfExportService = pdfExportService ?? throw new ArgumentNullException(nameof(pdfExportService));

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

            // Initialize ProcessContentCommand
            ProcessContentCommand = new RelayCommand(() =>
            {
                OnContentProcessingRequested?.Invoke(this, EventArgs.Empty);
            });

            _highlights = new List<HighlightInfo>();
        }

        private void ZoomIn() { /* Implementation */ }
        private void ZoomOut() { /* Implementation */ }
        private void ToggleHighlight()
        {
            IsHighlightMode = !IsHighlightMode;
            OnHighlightModeChanged?.Invoke(this, IsHighlightMode);
        }

        private void AddNote() { /* Implementation */ }
        private async void SaveProgress()
        {
            if (_pdfExportService == null)
            {
                var errorDialog = new ContentDialog
                {
                    Title = "Lỗi",
                    Content = "Dịch vụ xuất PDF chưa được khởi tạo",
                    CloseButtonText = "OK",
                    XamlRoot = App.MainWindow.Content.XamlRoot
                };
                await errorDialog.ShowAsync();
                return;
            }

            var mainWindow = App.MainWindow;
            if (mainWindow == null) return;

            var picker = new Windows.Storage.Pickers.FileSavePicker();
            picker.SuggestedStartLocation = Windows.Storage.Pickers.PickerLocationId.DocumentsLibrary;
            picker.FileTypeChoices.Add("PDF files", new List<string>() { ".pdf" });
            picker.SuggestedFileName = $"Reading_Test_{TestDetail.Id}";

            // Initialize the picker with the window handle
            WinRT.Interop.InitializeWithWindow.Initialize(picker, WinRT.Interop.WindowNative.GetWindowHandle(mainWindow));

            var file = await picker.PickSaveFileAsync();
            if (file != null)
            {
                ContentDialog loadingDialog = null;
                try
                {
                    loadingDialog = new ContentDialog
                    {
                        Title = "Đang xuất PDF",
                        Content = "Vui lòng đợi trong giây lát...",
                        XamlRoot = mainWindow.Content.XamlRoot
                    };

                    // Hiển thị dialog loading
                    var loadingTask = loadingDialog.ShowAsync();

                    // Xuất PDF
                    await _pdfExportService.ExportReadingTestToPdfAsync(TestDetail, file.Path);

                    // Đóng dialog loading
                    loadingDialog.Hide();

                    // Đợi dialog loading đóng hoàn toàn
                    await Task.Delay(100);

                    // Hiển thị dialog thành công
                    var successDialog = new ContentDialog
                    {
                        Title = "Thành công",
                        Content = "Bài làm đã được lưu thành công!",
                        CloseButtonText = "OK",
                        XamlRoot = mainWindow.Content.XamlRoot
                    };

                    await successDialog.ShowAsync();
                }
                catch (Exception ex)
                {
                    // Đảm bảo dialog loading đã đóng
                    if (loadingDialog != null)
                    {
                        loadingDialog.Hide();
                        await Task.Delay(100);
                    }

                    var errorDialog = new ContentDialog
                    {
                        Title = "Lỗi",
                        Content = $"Không thể lưu file: {ex.Message}",
                        CloseButtonText = "OK",
                        XamlRoot = mainWindow.Content.XamlRoot
                    };

                    await errorDialog.ShowAsync();
                }
            }
        }



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
                string answerID = await _testService.SubmitTestAsync(TestDetail.Id);
                if (answerID != "")
                {
                    TestDetail.Progress.IsCompleted = true;

                    // Tính thời gian làm bài
                    var timeSpent = TimeSpan.FromSeconds(TestDetail.TimeLimit * 60 - TestDetail.Progress.RemainingTime);


                    // Tạo TestResultViewModel và chuyển hướng
                    var resultViewModel = new TestResultViewModel(TestDetail, timeSpent, (App.Current as App).ChartService, _navigationService, answerID);
                    await resultViewModel.LoadSummaryAsync(answerID);
					(App.Current as App).CurrentTestResult = resultViewModel;
                    await _navigationService.NavigateToAsync(typeof(TestResultPage), answerID);
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

        public event EventHandler OnContentProcessingRequested;

        // Event to notify view that content needs to be reprocessed
        //public event EventHandler OnContentProcessingRequested;

        public void AddHighlight(string text, int startIndex, int length)
        {
            if (IsHighlightMode)
            {
                var highlight = new HighlightInfo
                {
                    Text = text,
                    StartIndex = startIndex,
                    Length = length,
                    Color = Windows.UI.Color.FromArgb(255, 255, 255, 0) // Yellow highlight
                };

                Highlights.Add(highlight);
                OnPropertyChanged(nameof(Highlights));
                OnHighlightAdded?.Invoke(this, highlight);
            }
        }

        public void RemoveHighlight(HighlightInfo highlight)
        {
            if (Highlights.Remove(highlight))
            {
                OnPropertyChanged(nameof(Highlights));
                OnHighlightRemoved?.Invoke(this, highlight);
            }
        }

        // Events for highlight changes
        public event EventHandler<bool> OnHighlightModeChanged;
        public event EventHandler<HighlightInfo> OnHighlightAdded;
        public event EventHandler<HighlightInfo> OnHighlightRemoved;
    }


    public class HighlightInfo
    {
        public string Text { get; set; }
        public int StartIndex { get; set; }
        public int Length { get; set; }
        public Windows.UI.Color Color { get; set; }
    }
}
