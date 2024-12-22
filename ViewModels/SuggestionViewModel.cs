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
    public class Card
    {
        public string CardId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string ImageSource { get; set; }
        public int ViewCount { get; set; }
    }
    public class SuggestionViewModel : ObservableObject
    {
        public ObservableCollection<Card> Cards { get; set; }
        private ObservableCollection<ReadingItemModels> _items { get; set; }
        private readonly ReadingItemsService _readingItemsService;
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