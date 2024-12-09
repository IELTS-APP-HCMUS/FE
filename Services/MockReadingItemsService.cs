using login_full.Models;
using System;
using System.Collections.ObjectModel;
using System.Linq;
using System.Threading.Tasks;

namespace login_full.Services
{
    public class MockReadingItemsService : IReadingItemsService
    {
        private readonly ObservableCollection<ReadingItemModels> _items;

        public MockReadingItemsService()
        {
            _items = new ObservableCollection<ReadingItemModels>
            {
                new ReadingItemModels
                {
                    TestId = "test1",
                    Title = "Gap Filling - Easy",
                    Description = "Practice your gap-filling skills with easy passages",
                    Duration = "10 mins",
                    Difficulty = "Easy",
                    Category = "Gap Filling",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = true  // Đã hoàn thành
                },
                new ReadingItemModels
                {
                    TestId ="test2",
                    Title = "Matching Headings - Intermediate",
                    Description = "Match the correct headings to the passages",
                    Duration = "15 mins",
                    Difficulty = "Intermediate",
                    Category = "Matching",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false // Chưa hoàn thành
                },
                new ReadingItemModels
                {
                    TestId = "test3",
                    Title = "Gap Filling - Easy",
                    Description = "Practice your gap-filling skills with easy passages",
                    Duration = "10 mins",
                    Difficulty = "Easy",
                    Category = "Gap Filling",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = true
                },
                new ReadingItemModels
                {
                    TestId = "test4",
                    Title = "Matching Headings - Intermediate",
                    Description = "Match the correct headings to the passages.",
                    Duration = "15 mins",
                    Difficulty = "Intermediate",
                    Category = "Matching",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test5",
                    Title = "True-False-Not Given - Hard",
                    Description = "Identify true, false, or not given statements.",
                    Duration = "20 mins",
                    Difficulty = "Hard",
                    Category = "True/False",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = true
                },
                new ReadingItemModels
                {
                    TestId = "test6",
                    Title = "Reading Test 4",
                    Description = "Test your reading comprehension.",
                    Duration = "25 mins",
                    Difficulty = "Medium",
                    Category = "Reading",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test7",
                    Title = "Listening Practice",
                    Description = "Improve your listening skills.",
                    Duration = "30 mins",
                    Difficulty = "Easy",
                    Category = "Listening",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test8",
                    Title = "Advanced Passage",
                    Description = "Challenge yourself with an advanced passage.",
                    Duration = "35 mins",
                    Difficulty = "Hard",
                    Category = "Reading",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = true
                },
                new ReadingItemModels
                {
                    TestId = "test9",
                    Title = "General Reading",
                    Description = "Casual reading exercise.",
                    Duration = "15 mins",
                    Difficulty = "Easy",
                    Category = "Reading",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test10",
                    Title = "Listening Quiz",
                    Description = "A quick listening test.",
                    Duration = "20 mins",
                    Difficulty = "Medium",
                    Category = "Listening",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test11",
                    Title = "General Reading",
                    Description = "Casual reading exercise.",
                    Duration = "15 mins",
                    Difficulty = "Easy",
                    Category = "Reading",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test12",
                    Title = "Listening Quiz",
                    Description = "A quick listening test.",
                    Duration = "20 mins",
                    Difficulty = "Medium",
                    Category = "Listening",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test13",
                    Title = "Gap Filling - Easy",
                    Description = "Practice your gap-filling skills with easy passages",
                    Duration = "10 mins",
                    Difficulty = "Easy",
                    Category = "Gap Filling",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test14",
                    Title = "Matching Headings - Intermediate",
                    Description = "Match the correct headings to the passages.",
                    Duration = "15 mins",
                    Difficulty = "Intermediate",
                    Category = "Matching",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test15",
                    Title = "True-False-Not Given - Hard",
                    Description = "Identify true, false, or not given statements.",
                    Duration = "20 mins",
                    Difficulty = "Hard",
                    Category = "True/False",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                },
                new ReadingItemModels
                {
                    TestId = "test16",
                    Title = "Reading Test 4",
                    Description = "Test your reading comprehension.",
                    Duration = "25 mins",
                    Difficulty = "Medium",
                    Category = "Reading",
                    ImagePath = "/Assets/reading_win.png",
                    IsSubmitted = false
                }
            };
        }

        public Task<ObservableCollection<ReadingItemModels>> GetReadingItemsAsync()
        {
            return Task.FromResult(_items);
        }

        public Task<ObservableCollection<ReadingItemModels>> GetCompletedItemsAsync()
        {
            var completedItems = new ObservableCollection<ReadingItemModels>(
                _items.Where(item => item.IsSubmitted)
            );
            return Task.FromResult(completedItems);
        }

        public Task<ObservableCollection<ReadingItemModels>> GetUncompletedItemsAsync()
        {
            var uncompletedItems = new ObservableCollection<ReadingItemModels>(
                _items.Where(item => !item.IsSubmitted)
            );
            return Task.FromResult(uncompletedItems);
        }

        public Task<ObservableCollection<ReadingItemModels>> SearchItemsAsync(string searchTerm)
        {
            var filteredItems = new ObservableCollection<ReadingItemModels>(
                _items.Where(item =>
                    item.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
                    item.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
            );
            return Task.FromResult(filteredItems);
        }
    }
}
