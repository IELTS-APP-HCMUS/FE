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

    /// <summary>
    // ViewModel quản lý danh sách từ vựng và các chức năng liên quan.
    // Cung cấp các tính năng: thêm, xóa, sửa, tìm kiếm và phân trang từ vựng.
    // </summary>
    public partial class VocabularyViewModel : ObservableObject
    {
        /// <summary>
        /// Danh sách từ vựng có thể quan sát thay đổi
        /// </summary>
        private ObservableCollection<VocabularyItem> _vocabularyItems;
        /// <summary>
        /// Service xử lý các thao tác với từ vựng
        /// </summary>
        /// <remarks>
        /// Xử lý:
        /// - Thêm/xóa/sửa từ vựng
        /// - Đồng bộ với cơ sở dữ liệu
        /// - Quản lý trạng thái học tập
        /// </remarks>
        private readonly VocabularyService _vocabularyService;
        /// <summary>
        /// Trang hiện tại trong phân trang
        /// </summary>
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
        /// <summary>
        /// Danh sách từ vựng hiển thị trên UI
        /// </summary>
        /// <remarks>
        /// Binding hai chiều với ListView
        /// Tự động cập nhật UI khi có thay đổi
        /// </remarks>
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
        /// <summary>
        /// Khởi tạo một instance mới của <see cref="VocabularyViewModel"/>.
        /// </summary>
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
            //_sampleData = new List<VocabularyItem>
            //{
            //    new VocabularyItem
            //    {
            //        Word = "abandon",
            //        Status = "Đã học",
            //        WordType = "verb",
            //        Meaning = "từ bỏ, bỏ rơi",
            //        Example = "He abandoned his car and continued on foot.",
            //        Note = "Thường dùng trong văn viết"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "ability",
            //        Status = "Đang học",
            //        WordType = "noun",
            //        Meaning = "khả năng, năng lực",
            //        Example = "She has the ability to speak six languages.",
            //        Note = "Từ thông dụng"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "abroad",
            //        Status = "Đã học",
            //        WordType = "adverb",
            //        Meaning = "ở nước ngoài",
            //        Example = "He's currently studying abroad.",
            //        Note = "Dùng trong du học"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "accomplish",
            //        Status = "Đang học",
            //        WordType = "verb",
            //        Meaning = "hoàn thành, đạt được",
            //        Example = "She accomplished all her goals for the year.",
            //        Note = "Từ formal"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "accurate",
            //        Status = "Đã học",
            //        WordType = "adjective",
            //        Meaning = "chính xác",
            //        Example = "The weather forecast was very accurate.",
            //        Note = "Dùng trong khoa học"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "achieve",
            //        Status = "Đang học",
            //        WordType = "verb",
            //        Meaning = "đạt được, giành được",
            //        Example = "He achieved his dream of becoming a doctor.",
            //        Note = "Từ động lực"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "adapt",
            //        Status = "Đã học",
            //        WordType = "verb",
            //        Meaning = "thích nghi, điều chỉnh",
            //        Example = "Animals must adapt to survive.",
            //        Note = "Sinh học"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "adequate",
            //        Status = "Đang học",
            //        WordType = "adjective",
            //        Meaning = "đầy đủ, thích đáng",
            //        Example = "The room provides adequate space for our needs.",
            //        Note = "Formal writing"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "adjust",
            //        Status = "Đã học",
            //        WordType = "verb",
            //        Meaning = "điều chỉnh",
            //        Example = "You need to adjust the temperature.",
            //        Note = "Kỹ thuật"
            //    },
            //    new VocabularyItem
            //    {
            //        Word = "admire",
            //        Status = "Đang học",
            //        WordType = "verb",
            //        Meaning = "ngưỡng mộ, khâm phục",
            //        Example = "I admire her courage and determination.",
            //        Note = "Cảm xúc"
            //    }
            //};
            _sampleData = await _vocabularyService.GetVocabularyAsync();

            for (int i = 0; i < _sampleData.Count; i++)
            {
                _sampleData[i].Index = i + 1;
            }

            _filteredData = _sampleData;
            _totalItems = _sampleData.Count;

            LoadPagedData();
        }
        /// <summary>
        /// Tải dữ liệu phân trang.
        /// </summary>
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
        /// <summary>
        /// Chuyển đến trang tiếp theo.
        /// </summary>
        private void NextPage()
        {
            int totalPages = (_totalItems + _pageSize - 1) / _pageSize;
            if (_currentPage < totalPages)
            {
                _currentPage++;
                LoadPagedData();
            }
        }
        /// <summary>
        /// Quay lại trang trước đó.
        /// </summary>
        private void PreviousPage()
        {
            if (_currentPage > 1)
            {
                _currentPage--;
                LoadPagedData();
            }
        }
        /// <summary>
        /// Thay đổi kích thước trang.
        /// </summary>
        /// <param name="newSize">Kích thước trang mới</param>
        private void ChangePageSize(int newSize)
        {
            _pageSize = newSize;
            _currentPage = 1;
            LoadPagedData();
        }

        // xóa dựa vào VocabularyItem
        //private async void DeleteItem(VocabularyItem item)
        //{
        //    if (item != null)
        //    {
        //        await _vocabularyService.DeleteVocabularyAsync(item.WordKey);
        //        VocabularyItems.Remove(item);
        //        _sampleData.Remove(item);
        //        _totalItems--;

        //        for (int i = 0; i < _sampleData.Count; i++)
        //        {
        //            _sampleData[i].Index = i + 1;
        //        }

        //        LoadPagedData();

        //        if (VocabularyItems.Count == 0 && _currentPage > 1)
        //        {
        //            _currentPage--;
        //            LoadPagedData();
        //        }
        //    }
        //}
        /// <summary>
        /// Xóa từ vựng
        /// </summary>
        /// <param name="item">Từ vựng cần xóa</param>
        /// <remarks>
        /// 1. Hiển thị dialog xác nhận
        /// 2. Xóa từ database nếu người dùng đồng ý
        /// 3. Cập nhật UI và danh sách local
        /// </remarks>
        private async void DeleteItem(VocabularyItem item)
        {
            if (item != null)
            {
                // Tạo dialog xác nhận
                ContentDialog deleteDialog = new ContentDialog
                {
                    Title = "Xác nhận xóa",
                    Content = $"Bạn có chắc chắn muốn xóa từ \"{item.Word}\" khỏi sổ từ vựng?",
                    PrimaryButtonText = "Xóa",
                    CloseButtonText = "Hủy",
                    DefaultButton = ContentDialogButton.Close,
                    XamlRoot = App.MainWindow.Content.XamlRoot
                };

                // Hiển thị dialog và đợi phản hồi
                var result = await deleteDialog.ShowAsync();

                // Chỉ xóa nếu người dùng nhấn nút Xóa
                if (result == ContentDialogResult.Primary)
                {
                    await _vocabularyService.DeleteVocabularyAsync(item.WordKey);
                    VocabularyItems.Remove(item);
                    _sampleData.Remove(item);
                    _totalItems--;

                    // Cập nhật lại index cho tất cả items
                    for (int i = 0; i < _sampleData.Count; i++)
                    {
                        _sampleData[i].Index = i + 1;
                    }

                    LoadPagedData();

                    // Nếu trang hiện tại không còn item nào, quay lại trang trước
                    if (VocabularyItems.Count == 0 && _currentPage > 1)
                    {
                        _currentPage--;
                        LoadPagedData();
                    }
                }
            }
        }

        // thay đổi trạng thái đã học -> dang học và ngược lại
        /// <summary>
        /// Thay đổi trạng thái đã học -> đang học và ngược lại.
        /// </summary>
        /// <param name="item">Từ vựng cần thay đổi trạng thái</param>
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
        /// <summary>
        /// Hiển thị dialog thêm từ vựng mới.
        /// </summary>
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
        /// <summary>
        /// Tìm kiếm từ vựng theo từ khóa
        /// </summary>
        /// <param name="searchText">Từ khóa tìm kiếm</param>
        /// <remarks>
        /// Tìm kiếm theo các tiêu chí:
        /// - Từ vựng
        /// - Loại từ
        /// - Nghĩa
        /// - Ví dụ
        /// - Ghi chú
        /// Không phân biệt chữ hoa/thường
        /// </remarks>
        private void SearchVocabulary(string searchText)
        {
            if (string.IsNullOrWhiteSpace(searchText))
            {
                _filteredData = _sampleData;
            }
            else
            {
                searchText = searchText.ToLower();
                //_filteredData = _sampleData.Where(item =>
                //    item.Word?.ToLower().Contains(searchText) ||
                //    item.WordType?.ToLower().Contains(searchText) ||
                //    item.Meaning?.ToLower().Contains(searchText) ||
                //    item.Example?.ToLower().Contains(searchText) ||
                //    item.Note?.ToLower().Contains(searchText)
                //).ToList();
                var lowerSearchText = searchText.ToLower();
                _filteredData = _sampleData.Where(item =>
                    (item.Word?.ToLower().Contains(lowerSearchText) ?? false) ||
                    (item.WordType?.ToLower().Contains(lowerSearchText) ?? false) ||
                    (item.Meaning?.ToLower().Contains(lowerSearchText) ?? false) ||
                    (item.Example?.ToLower().Contains(lowerSearchText) ?? false) ||
                    (item.Note?.ToLower().Contains(lowerSearchText) ?? false)
                ).ToList();
            }

            _totalItems = _filteredData.Count;
            _currentPage = 1;
            LoadPagedData();
        }
    }
} 
