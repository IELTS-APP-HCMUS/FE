using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Windows.Storage;

namespace login_full.Services
{
	public class CacheService
	{
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
