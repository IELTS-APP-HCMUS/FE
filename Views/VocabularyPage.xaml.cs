using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using System.Collections.ObjectModel;
using Microsoft.UI;
using Microsoft.UI.Xaml.Media;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using System.Linq;
using System.Collections.Generic;

namespace login_full.Views
{
    public sealed partial class VocabularyPage : Page, INotifyPropertyChanged
    {
        //private ObservableCollection<VocabularyItem> _vocabularyItems;
        //private int _currentPage = 1;
        //private int _pageSize = 5;
        //private int _totalItems = 0;
        //private List<VocabularyItem> _sampleData;

        //public ObservableCollection<VocabularyItem> VocabularyItems
        //{
        //    get => _vocabularyItems;
        //    set
        //    {
        //        _vocabularyItems = value;
        //        OnPropertyChanged();
        //    }
        //}

        public event PropertyChangedEventHandler PropertyChanged;

        public VocabularyPage()
        {
            this.InitializeComponent();
            //VocabularyItems = new ObservableCollection<VocabularyItem>();
            //InitializeData();
        }

        //    private void InitializeData()
        //    {
        //        // Tạo dữ liệu mẫu
        //        _sampleData = new List<VocabularyItem>
        //        {
        //            new VocabularyItem
        //            {
        //                Index = 1,
        //                Word = "abandon",
        //                Status = "Đã học",
        //                WordType = "verb",
        //                Meaning = "từ bỏ, bỏ rơi",
        //                Example = "He abandoned his car and continued on foot.",
        //                Note = "Thường dùng trong văn viết"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 2,
        //                Word = "ability",
        //                Status = "Đang học",
        //                WordType = "noun",
        //                Meaning = "khả năng, năng lực",
        //                Example = "She has the ability to speak six languages.",
        //                Note = "Từ thông dụng"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 3,
        //                Word = "abroad",
        //                Status = "Đã học",
        //                WordType = "adverb",
        //                Meaning = "ở nước ngoài",
        //                Example = "He's currently studying abroad.",
        //                Note = "Dùng trong du học"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 4,
        //                Word = "accomplish",
        //                Status = "Đang học",
        //                WordType = "verb",
        //                Meaning = "hoàn thành, đạt được",
        //                Example = "She accomplished all her goals for the year.",
        //                Note = "Từ formal"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 5,
        //                Word = "accurate",
        //                Status = "Đã học",
        //                WordType = "adjective",
        //                Meaning = "chính xác",
        //                Example = "The weather forecast was very accurate.",
        //                Note = "Dùng trong khoa học"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 6,
        //                Word = "achieve",
        //                Status = "Đang học",
        //                WordType = "verb",
        //                Meaning = "đạt được, giành được",
        //                Example = "He achieved his dream of becoming a doctor.",
        //                Note = "Từ động lực"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 7,
        //                Word = "adapt",
        //                Status = "Đã học",
        //                WordType = "verb",
        //                Meaning = "thích nghi, điều chỉnh",
        //                Example = "Animals must adapt to survive.",
        //                Note = "Sinh học"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 8,
        //                Word = "adequate",
        //                Status = "Đang học",
        //                WordType = "adjective",
        //                Meaning = "đầy đủ, thích đáng",
        //                Example = "The room provides adequate space for our needs.",
        //                Note = "Formal writing"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 9,
        //                Word = "adjust",
        //                Status = "Đã học",
        //                WordType = "verb",
        //                Meaning = "điều chỉnh",
        //                Example = "You need to adjust the temperature.",
        //                Note = "Kỹ thuật"
        //            },
        //            new VocabularyItem
        //            {
        //                Index = 10,
        //                Word = "admire",
        //                Status = "Đang học",
        //                WordType = "verb",
        //                Meaning = "ngưỡng mộ, khâm phục",
        //                Example = "I admire her courage and determination.",
        //                Note = "Cảm xúc"
        //            }
        //        };

        //        _totalItems = _sampleData.Count;
        //        LoadPagedData();
        //    }

        //    private void LoadPagedData()
        //    {
        //        if (VocabularyItems == null)
        //        {
        //            VocabularyItems = new ObservableCollection<VocabularyItem>();
        //        }

        //        VocabularyItems.Clear();
        //        var pagedData = _sampleData
        //            .Skip((_currentPage - 1) * _pageSize)
        //            .Take(_pageSize);

        //        foreach (var item in pagedData)
        //        {
        //            VocabularyItems.Add(item);
        //        }
        //        UpdatePageInfo();
        //    }

        //    private void UpdatePageInfo()
        //    {
        //        int totalPages = (_totalItems + _pageSize - 1) / _pageSize;
        //        PageInfoText.Text = $"{_currentPage}/{totalPages}";

        //        PreviousButton.IsEnabled = _currentPage > 1;
        //        NextButton.IsEnabled = _currentPage < totalPages;
        //    }

        //    private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        //    {
        //        var combo = sender as ComboBox;
        //        _pageSize = (combo.SelectedIndex + 1) * 5;
        //        _currentPage = 1;
        //        LoadPagedData();
        //    }

        //    private void PreviousButton_Click(object sender, RoutedEventArgs e)
        //    {
        //        if (_currentPage > 1)
        //        {
        //            _currentPage--;
        //            LoadPagedData();
        //        }
        //    }

        //    private void NextButton_Click(object sender, RoutedEventArgs e)
        //    {
        //        int totalPages = (_totalItems + _pageSize - 1) / _pageSize;
        //        if (_currentPage < totalPages)
        //        {
        //            _currentPage++;
        //            LoadPagedData();
        //        }
        //    }

        //    private void DeleteButton_Click(object sender, RoutedEventArgs e)
        //    {
        //        var button = sender as Button;
        //        var item = button.DataContext as VocabularyItem;
        //        VocabularyItems.Remove(item);
        //        // TODO: Implement actual delete logic
        //    }

        //    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}

        //public class VocabularyItem : INotifyPropertyChanged
        //{
        //    private int _index;
        //    private string _word;
        //    private string _status;
        //    private string _wordType;
        //    private string _meaning;
        //    private string _example;
        //    private string _note;

        //    public int Index
        //    {
        //        get => _index;
        //        set
        //        {
        //            _index = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public string Word
        //    {
        //        get => _word;
        //        set
        //        {
        //            _word = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public string Status
        //    {
        //        get => _status;
        //        set
        //        {
        //            _status = value;
        //            OnPropertyChanged();
        //            OnPropertyChanged(nameof(StatusColor));
        //        }
        //    }

        //    public SolidColorBrush StatusColor
        //    {
        //        get => Status == "Đã học" ?
        //            new SolidColorBrush(Colors.Green) :
        //            new SolidColorBrush(Colors.Red);
        //    }

        //    public string WordType
        //    {
        //        get => _wordType;
        //        set
        //        {
        //            _wordType = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public string Meaning
        //    {
        //        get => _meaning;
        //        set
        //        {
        //            _meaning = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public string Example
        //    {
        //        get => _example;
        //        set
        //        {
        //            _example = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public string Note
        //    {
        //        get => _note;
        //        set
        //        {
        //            _note = value;
        //            OnPropertyChanged();
        //        }
        //    }

        //    public event PropertyChangedEventHandler PropertyChanged;

        //    private void OnPropertyChanged([CallerMemberName] string propertyName = null)
        //    {
        //        PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        //    }
        //}
    }
}
