using login_full.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace login_full.Services
{
    public class LocalStorageService
    {
        private const string TEST_HISTORY_KEY = "test_history";

        public void SaveTestHistory(List<TestHistory> history)
        {
            var json = JsonSerializer.Serialize(history);
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[TEST_HISTORY_KEY] = json;
        }

        public List<TestHistory> GetTestHistory()
        {
            var json = Windows.Storage.ApplicationData.Current.LocalSettings.Values[TEST_HISTORY_KEY] as string;
            if (string.IsNullOrEmpty(json))
                return new List<TestHistory>();
            return JsonSerializer.Deserialize<List<TestHistory>>(json) ?? new List<TestHistory>();
        }
    }
} 