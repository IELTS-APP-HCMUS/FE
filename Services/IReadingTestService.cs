using login_full.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace login_full.Services
{
    public interface IReadingTestService
    {
        Task<ReadingTestDetail> GetTestDetailAsync(string testId);
        Task SaveAnswerAsync(string testId, string questionId, string answer);
        Task<string> SubmitTestAsync(string testId);

        Task<bool> UpdateTestCompletionStatus(string testId, bool isCompleted);
        Task<List<TestHistory>> GetTestHistoryAsync();
    }
}
