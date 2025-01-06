using login_full.Models;
using System.Collections.Generic;
using System.Text.Json;

namespace login_full.Services
{/// <summary>
 /// Dịch vụ lưu trữ dữ liệu cục bộ, đặc biệt là lịch sử bài kiểm tra.
 /// </summary>
	public class LocalStorageService
    {
		/// <summary>
		/// Khóa lưu trữ lịch sử bài kiểm tra trong cài đặt cục bộ.
		/// </summary>
		private const string TEST_HISTORY_KEY = "test_history";
		
		/// 
		/// <summary>
		/// Lưu lịch sử bài kiểm tra vào bộ nhớ cục bộ.
		/// </summary>
		/// <param name="history">Danh sách lịch sử bài kiểm tra.</param>
		public void SaveTestHistory(List<TestHistory> history)
        {
            var json = JsonSerializer.Serialize(history);
            Windows.Storage.ApplicationData.Current.LocalSettings.Values[TEST_HISTORY_KEY] = json;
        }

		/// <summary>
		/// Lấy lịch sử bài kiểm tra từ bộ nhớ cục bộ.
		/// </summary>
		/// <returns>Danh sách lịch sử bài kiểm tra hoặc danh sách rỗng nếu không có dữ liệu.</returns>
		public List<TestHistory> GetTestHistory()
        {
            var json = Windows.Storage.ApplicationData.Current.LocalSettings.Values[TEST_HISTORY_KEY] as string;
            if (string.IsNullOrEmpty(json))
                return new List<TestHistory>();
            return JsonSerializer.Deserialize<List<TestHistory>>(json) ?? new List<TestHistory>();
        }
    }
} 