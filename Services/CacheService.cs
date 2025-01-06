using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace login_full.Services
{

    /// <summary>
    // Service quản lý cache cho ứng dụng.
    // Cung cấp các chức năng xóa cache cho câu trả lời bài kiểm tra.
    // </summary>
    public class CacheService
	{

        /// <summary>
        /// Xóa cache câu trả lời cho bài kiểm tra cụ thể.
        /// </summary>
        /// <param name="testId">ID của bài kiểm tra</param>
        /// <returns>Task hoàn thành việc xóa cache</returns>
        public async Task ClearTestAnswersAsync(string testId)
		{
			// Clear cached answers for the given test ID
			ApplicationDataContainer localSettings = ApplicationData.Current.LocalSettings;
			localSettings.Values[$"Answers_{testId}"] = null;

			// Clear any other related cached data if needed
			localSettings.Values[$"Progress_{testId}"] = null;

			await Task.CompletedTask;
		}
	}

}
