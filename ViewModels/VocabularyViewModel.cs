using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Windows.Input;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using login_full.Models;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml;
using login_full.Services;

namespace login_full.ViewModels
{
    public partial class VocabularyViewModel : ObservableObject
    {
        private ObservableCollection<VocabularyItem> _vocabularyItems;
        private readonly VocabularyService _vocabularyService;
        private int _currentPage = 1;
        private int _pageSize = 5;
        private int _totalItems = 0;
        private List<VocabularyItem> _sampleData;
        private string _pageInfo;
        private bool _canGoNext;
        private bool _canGoPrevious;
        private string _searchText;
        private List<VocabularyItem> _filteredData;

       // xử lý API here
        public ObservableCollection<VocabularyItem> VocabularyItems
        {
            get => _vocabularyItems;
            set => SetProperty(ref _vocabularyItems, value);
        }

        public string PageInfo
        {
            get => _pageInfo;
            set => SetProperty(ref _pageInfo, value);
        }

        public bool CanGoNext
        {
            get => _canGoNext;
            set => SetProperty(ref _canGoNext, value);
        }

        public bool CanGoPrevious
        {
            get => _canGoPrevious;
            set => SetProperty(ref _canGoPrevious, value);
        }

        public string SearchText
        {
            get => _searchText;
            set
            {
                if (SetProperty(ref _searchText, value))
                {
                    SearchVocabulary(value);
                }
            }
        }

        public IRelayCommand NextPageCommand { get; }
        public IRelayCommand PreviousPageCommand { get; }
        public IRelayCommand<int> ChangePageSizeCommand { get; }
        public IRelayCommand<VocabularyItem> DeleteItemCommand { get; }
        public IRelayCommand<VocabularyItem> ToggleStatusCommand { get; }
        public IRelayCommand AddVocabCommand { get; }

        public VocabularyViewModel()
        {
            _vocabularyService = new VocabularyService();
            VocabularyItems = new ObservableCollection<VocabularyItem>();
            _filteredData = new List<VocabularyItem>();
            NextPageCommand = new RelayCommand(NextPage, () => CanGoNext);
            PreviousPageCommand = new RelayCommand(PreviousPage, () => CanGoPrevious);
            ChangePageSizeCommand = new RelayCommand<int>(ChangePageSize);
            DeleteItemCommand = new RelayCommand<VocabularyItem>(DeleteItem);
            ToggleStatusCommand = new RelayCommand<VocabularyItem>(ToggleStatus);
            AddVocabCommand = new RelayCommand(ShowAddVocabDialog);

            InitializeData();
        }

        private async void InitializeData()
        {
            _sampleData = new List<VocabularyItem>
            {
                new VocabularyItem
                {
                    Word = "abandon",
                    Status = "Đã học",
                    WordType = "verb",
                    Meaning = "từ bỏ, bỏ rơi",
                    Example = "He abandoned his car and continued on foot.",
                    Note = "Thường dùng trong văn viết"
                },
                new VocabularyItem
                {
                    Word = "ability",
                    Status = "Đang học",
                    WordType = "noun",
                    Meaning = "khả năng, năng lực",
                    Example = "She has the ability to speak six languages.",
                    Note = "Từ thông dụng"
                },
                new VocabularyItem
                {
                    Word = "abroad",
                    Status = "Đã học",
                    WordType = "adverb",
                    Meaning = "ở nước ngoài",
                    Example = "He's currently studying abroad.",
                    Note = "Dùng trong du học"
                },
                new VocabularyItem
                {
                    Word = "accomplish",
                    Status = "Đang học",
                    WordType = "verb",
                    Meaning = "hoàn thành, đạt được",
                    Example = "She accomplished all her goals for the year.",
                    Note = "Từ formal"
                },
                new VocabularyItem
                {
                    Word = "accurate",
                    Status = "Đã học",
                    WordType = "adjective",
                    Meaning = "chính xác",
                    Example = "The weather forecast was very accurate.",
                    Note = "Dùng trong khoa học"
                },
                new VocabularyItem
                {
                    Word = "achieve",
                    Status = "Đang học",
                    WordType = "verb",
                    Meaning = "đạt được, giành được",
                    Example = "He achieved his dream of becoming a doctor.",
                    Note = "Từ động lực"
                },
                new VocabularyItem
                {
                    Word = "adapt",
                    Status = "Đã học",
                    WordType = "verb",
                    Meaning = "thích nghi, điều chỉnh",
                    Example = "Animals must adapt to survive.",
                    Note = "Sinh học"
                },
                new VocabularyItem
                {
                    Word = "adequate",
                    Status = "Đang học",
                    WordType = "adjective",
                    Meaning = "đầy đủ, thích đáng",
                    Example = "The room provides adequate space for our needs.",
                    Note = "Formal writing"
                },
                new VocabularyItem
                {
                    Word = "adjust",
                    Status = "Đã học",
                    WordType = "verb",
                    Meaning = "điều chỉnh",
                    Example = "You need to adjust the temperature.",
                    Note = "Kỹ thuật"
                },
                new VocabularyItem
                {
                    Word = "admire",
                    Status = "Đang học",
                    WordType = "verb",
                    Meaning = "ngưỡng mộ, khâm phục",
                    Example = "I admire her courage and determination.",
                    Note = "Cảm xúc"
                }
            };
            _sampleData = await _vocabularyService.GetVocabularyAsync();

            for (int i = 0; i < _sampleData.Count; i++)
            {
                _sampleData[i].Index = i + 1;
            }

            _filteredData = _sampleData;
            _totalItems = _sampleData.Count;

            LoadPagedData();
        }

        private void LoadPagedData()
        {
            try 
            {
                VocabularyItems.Clear();
                var pagedData = _filteredData
                    .Skip((_currentPage - 1) * _pageSize)
                    .Take(_pageSize)
                    .Select((item, i) => {
                        item.Index = (_currentPage - 1) * _pageSize + i + 1;
                        return item;
                    });
                
                foreach (var item in pagedData)
                {
                    VocabularyItems.Add(item);
                }
                UpdatePageInfo();
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LoadPagedData: {ex.Message}");
            }
        }

        private void UpdatePageInfo()
        {
            try
            {
                int totalPages = (_totalItems + _pageSize - 1) / _pageSize;
                PageInfo = $"{_currentPage}/{totalPages}";
                CanGoPrevious = _currentPage > 1;
                CanGoNext = _currentPage < totalPages;
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in UpdatePageInfo: {ex.Message}");
            }
        }

        private void NextPage()
        {
            int totalPages = (_totalItems + _pageSize - 1) / _pageSize;
            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadPagedData();
            }
        }

        private void PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPagedData();
            }
        }

        private void ChangePageSize(int newSize)
        {
            _pageSize = newSize;
            _currentPage = 1;
            LoadPagedData();
        }

        // xóa dựa vào VocabularyItem
        private async void DeleteItem(VocabularyItem item)
        {
            if (item != null)
            {
                await _vocabularyService.DeleteVocabularyAsync(item.Word);
                VocabularyItems.Remove(item);
                _sampleData.Remove(item);
                _totalItems--;
                
                for (int i = 0; i < _sampleData.Count; i++)
                {
                    _sampleData[i].Index = i + 1;
                }
                
                LoadPagedData();
                
                if (VocabularyItems.Count == 0 && _currentPage > 1)
                {
                    _currentPage--;
                    LoadPagedData();
                }
            }
        }

        // thay đổi trạng thái đã học -> dang học và ngược lại
        private async void ToggleStatus(VocabularyItem item)
        {
            if (item != null)
            {
                var originalItem = _sampleData.FirstOrDefault(x => x.Index == item.Index);
                if (originalItem != null)
                {
                    originalItem.Status = originalItem.Status == "Đã học" ? "Đang học" : "Đã học";
                    //LoadPagedData();
                }
                await _vocabularyService.UpdateVocabularyAsync(originalItem);
            }
        }


        // thêm từ vựng 
        private async void ShowAddVocabDialog()
        {
            var stackPanel = new StackPanel
            {
                Spacing = 10,
                Children =
                {
                    new TextBox 
                    { 
                        Header = "Từ vựng",
                        Name = "WordTextBox",
                        PlaceholderText = "Nhập từ vựng..."
                    },
                    new ComboBox
                    {
                        Header = "Loại từ",
                        Name = "WordTypeComboBox",
                        PlaceholderText = "Chọn loại từ",
                        Items = { "noun", "verb", "adjective", "adverb", "preposition" }
                    },
                    new TextBox 
                    { 
                        Header = "Nghĩa của từ",
                        Name = "MeaningTextBox",
                        PlaceholderText = "Nhập nghĩa của từ..."
                    },
                    new TextBox 
                    { 
                        Header = "Ví dụ",
                        Name = "ExampleTextBox",
                        PlaceholderText = "Nhập ví dụ...",
                        TextWrapping = TextWrapping.Wrap,
                        AcceptsReturn = true,
                        Height = 60
                    },
                    new TextBox 
                    { 
                        Header = "Ghi chú (tùy chọn)",
                        Name = "NoteTextBox",
                        PlaceholderText = "Nhập ghi chú..."
                    }
                }
            };

            var dialog = new ContentDialog()
            {
                Title = "Thêm từ vựng mới",
                PrimaryButtonText = "Thêm",
                CloseButtonText = "Hủy",
                DefaultButton = ContentDialogButton.Primary,
                XamlRoot = App.MainWindow.Content.XamlRoot,
                Content = stackPanel
            };

            var result = await dialog.ShowAsync();
            if (result == ContentDialogResult.Primary)
            {
                var wordTextBox = stackPanel.Children.FirstOrDefault(x => (x as FrameworkElement)?.Name == "WordTextBox") as TextBox;
                var wordTypeComboBox = stackPanel.Children.FirstOrDefault(x => (x as FrameworkElement)?.Name == "WordTypeComboBox") as ComboBox;
                var meaningTextBox = stackPanel.Children.FirstOrDefault(x => (x as FrameworkElement)?.Name == "MeaningTextBox") as TextBox;
                var exampleTextBox = stackPanel.Children.FirstOrDefault(x => (x as FrameworkElement)?.Name == "ExampleTextBox") as TextBox;
                var noteTextBox = stackPanel.Children.FirstOrDefault(x => (x as FrameworkElement)?.Name == "NoteTextBox") as TextBox;

                if (!string.IsNullOrWhiteSpace(wordTextBox?.Text) && 
                    wordTypeComboBox?.SelectedItem != null && 
                    !string.IsNullOrWhiteSpace(meaningTextBox?.Text))
                {
                    var newItem = new VocabularyItem
                    {
                        Word = wordTextBox.Text,
                        WordType = wordTypeComboBox.SelectedItem.ToString(),
                        Meaning = meaningTextBox.Text,
                        Example = exampleTextBox?.Text ?? string.Empty,
                        Note = noteTextBox?.Text ?? string.Empty,
                        Status = "Đang học"
                    };

                    // Thêm vào danh sách từ vựng
                    await _vocabularyService.AddVocabularyAsync(newItem);

                    _sampleData.Insert(0, newItem);
                    _totalItems++;

                    // Cập nhật lại index cho tất cả items
                    for (int i = 0; i < _sampleData.Count; i++)
                    {
                        _sampleData[i].Index = i + 1;
                    }

                    // Đảm bảo hiển thị trang đầu tiên để thấy item mới
                    _currentPage = 1;
                    LoadPagedData();
                }
            }
        }

        private void SearchVocabulary(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredData = _sampleData;
            }
            else
            {
                searchText = searchText.ToLower();
                _filteredData = _sampleData.Where(item =>
                    item.Word.ToLower().Contains(searchText) ||
                    item.WordType.ToLower().Contains(searchText) ||
                    item.Meaning.ToLower().Contains(searchText) ||
                    item.Example.ToLower().Contains(searchText) ||
                    item.Note.ToLower().Contains(searchText)
                ).ToList();
            }

            _totalItems = _filteredData.Count;
            _currentPage = 1;
            LoadPagedData();
        }
    }
} 