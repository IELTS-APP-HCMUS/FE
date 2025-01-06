using CommunityToolkit.Mvvm.ComponentModel;
using login_full.Models;
using login_full.Services;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.ViewModels
{
    /// <summary>
    // Model đại diện cho một thẻ gợi ý bài đọc
    // </summary>
    public class Card
    {
        /// <summary>
        /// ID định danh của thẻ
        /// </summary>
        public string CardId { get; set; }
        /// <summary>
        /// Tiêu đề của thẻ
        /// </summary>
        public string Title { get; set; }
        /// <summary>
        /// Mô tả ngắn về nội dung
        /// </summary>
        public string Description { get; set; }
        /// <summary>
        /// Đường dẫn đến hình ảnh minh họa
        /// </summary>
        public string ImageSource { get; set; }
        /// <summary>
        /// Số lượt xem của bài đọc
        /// </summary>
        public int ViewCount { get; set; }
    }
    /// <summary>
    // ViewModel quản lý và hiển thị các gợi ý bài đọc cho người dùng.
    // Cung cấp danh sách các bài đọc được đề xuất dựa trên lịch sử và sở thích người dùng.
    // </summary>
    public class SuggestionViewModel : ObservableObject
    {
        /// <summary>
        /// Danh sách các thẻ gợi ý
        /// </summary>
        public ObservableCollection<Card> Cards { get; set; }
        /// <summary>
        /// Service xử lý các thao tác với bài đọc
        /// </summary>
        private ObservableCollection<ReadingItemModels> _items { get; set; }
        private readonly ReadingItemsService _readingItemsService;
        /// <summary>
        /// Danh sách các bài đọc được gợi ý
        /// </summary>
        public ObservableCollection<ReadingItemModels> Items
        {
            get => _items;
            private set
            {
                _items = value;
                OnPropertyChanged(nameof(Items)); // Notify UI when the collection changes
            }
        }
        public SuggestionViewModel(ReadingItemsService readingItemsService)
        {
            _readingItemsService = readingItemsService;
            Items = new ObservableCollection<ReadingItemModels>();
            Cards = new ObservableCollection<Card>
            {
                new Card
                {
                    CardId = "1",
                    Title = "Card 1",
                    Description = "Simply dummy text...",
                    ImageSource = "/Assets/reading_win.png",
                    ViewCount = 22334
                },
                new Card
                {
                    CardId = "2",
                    Title = "Card 2",
                    Description = "Simply dummy text...",
                    ImageSource = "/Assets/reading_win.png",
                    ViewCount = 13456
                },
                new Card
                {
                    CardId = "3",
                    Title = "Card 3",
                    Description = "Simply dummy text...",
                    ImageSource = "/Assets/reading_win.png",
                    ViewCount = 9876
                },
                new Card
                {
                    CardId = "4",
                    Title = "Card 4",
                    Description = "Simply dummy text...",
                    ImageSource = "/Assets/reading_win.png",
                    ViewCount = 4321
                }
            };
        }

        /// <summary>
        /// Tải danh sách bài đọc gợi ý từ service
        /// </summary>
        /// <returns>Task hoàn thành việc tải dữ liệu</returns>
        /// <remarks>
        /// Lấy 4 bài đọc mới nhất
        /// Xử lý lỗi và ghi log nếu có vấn đề
        /// Tự động cập nhật UI sau khi tải xong
        /// </remarks>
        public async Task LoadItemsAsync()
        {
            try
            {
                var items = await _readingItemsService.GetReadingItemsAsync();
                if (items != null)
                {
                    // Switch back to the main thread if necessary (WPF/WinUI usually auto does this)
                    // get the last 4 items
                    var lastItems = items.TakeLast(4).ToList();
                    Items = new ObservableCollection<ReadingItemModels>(lastItems);
                    System.Diagnostics.Debug.WriteLine($"Count: {Items.Count}");
                }
                else
                {
                    System.Diagnostics.Debug.WriteLine("No items fetched from service.");
                }
            }
            catch (Exception ex)
            {
                System.Diagnostics.Debug.WriteLine($"Error in LoadItemsAsync: {ex.Message}");
            }
        }
    }
}