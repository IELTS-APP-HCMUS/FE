using login_full.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using login_full.Context;
using System.Diagnostics;

namespace login_full.Services
{
	public class ReadingItemsService : IReadingItemsService
	{
		private readonly HttpClient _httpClient;
		public readonly ObservableCollection<ReadingItemModels> _items ;
		private bool _isInitialized;

		// Public property to expose _items
		public ReadingItemsService()
		{
			_httpClient = new HttpClient();
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
				IsSubmitted = true
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
				IsSubmitted = false
			},
            // ... Add more mock items as needed ...
        };

		}

		public async Task InitializeAsync()
		{
			if (_isInitialized)
				return;

			await LoadItemsFromAPIAsync();
			_isInitialized = true;
		}

		private async Task LoadItemsFromAPIAsync()
		{
			try
			{
				// Attempt to load items from the API
				await FetchItemsFromAPIAsync(1,25);

				// If no items were fetched, load mock data
				if (!_items.Any())
				{
					System.Diagnostics.Debug.WriteLine("No items fetched from API, loading mock data.");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Error loading items: {ex.Message}");
		
			}
		}

		private async Task FetchItemsFromAPIAsync(
			int ?pageNumber = null,
			int ?pageSize = null,
			int ?submittedStatus = null,
			string? searchTerm = null,
			int? type = null,
			int? tagPassage = null,
			int? tagQuestionType = null)
			{
				try
				{
					
				using (HttpClient client = new HttpClient())
					{
					string accessToken = GlobalState.Instance.AccessToken;

					// Add Bearer Token for Authorization
					client.DefaultRequestHeaders.Authorization =
						new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", accessToken);

					// Build Query Parameters
					var queryParams = new Dictionary<string, string>
					{
						{ "page", pageNumber.ToString() },
						{ "page_size", pageSize.ToString() },
						{ "submitted_status", submittedStatus.ToString() }
					};

					if (!string.IsNullOrEmpty(searchTerm)) queryParams.Add("search", searchTerm);
					if (type.HasValue) queryParams.Add("type", type.ToString());
					if (tagPassage.HasValue) queryParams.Add("tag_passage", tagPassage.ToString());
					if (tagQuestionType.HasValue) queryParams.Add("tag_question_type", tagQuestionType.ToString());

					string queryString = string.Join("&", queryParams.Select(kvp => $"{kvp.Key}={Uri.EscapeDataString(kvp.Value)}"));
					string url = $"https://ielts-app-api-4.onrender.com/v1/quizzes?{queryString}";
				
					HttpResponseMessage response = await client.GetAsync(url);

					if (response.IsSuccessStatusCode)
					{
						string stringResponse = await response.Content.ReadAsStringAsync();
						System.Diagnostics.Debug.WriteLine($"API url: {url}");
						// Deserialize API Response
						var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(stringResponse);

						// Clear the existing _items
						_items.Clear();

						// Map API Data into ReadingItemModels
						foreach (var item in apiResponse.Data.Items)
						{
							var mappedItem = new ReadingItemModels
							{
								TestId = item.Id.ToString(),
								Title = item.Title ?? "Default Title", // Fallback if null
								Description = item.ShortDescription ?? "Default Description",
								Duration = $"{item.Time} mins",
								Difficulty = ExtractDifficultyFromTags(item.Tags),
								Category = ExtractCategoryFromTags(item.Tags),
								ImagePath = "/Assets/reading_win.png", // Default Image
								IsSubmitted = item.IsSubmitted, // Map SubmittedStatus to IsCompleted
							};

							_items.Add(mappedItem);
							System.Diagnostics.Debug.WriteLine($"Mapped item: Id={mappedItem.TestId}, Title={mappedItem.Title}, Description={mappedItem.Description}, Duration={mappedItem.Duration}, Difficulty={mappedItem.Difficulty}, Category={mappedItem.Category}, IsSubmitted={mappedItem.IsSubmitted}");

						}

						System.Diagnostics.Debug.WriteLine($"Successfully fetched {apiResponse.Data.Items.Count} items from API.");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine($"Error fetching quizzes: {response.StatusCode} - {response.ReasonPhrase}");
					}
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Exception in FetchItemsFromAPIAsync: {ex.Message}");
			}
		}


		// Public methods that implement the interface
		//public Task<ObservableCollection<ReadingItemModels>> GetReadingItemsAsync()
		//{
		//	System.Diagnostics.Debug.WriteLine("Fetching items from the service...");
		//	System.Diagnostics.Debug.WriteLine($"Items in service: {_items?.Count ?? 0}");
		//	return Task.FromResult(_items);
		//}

		public async Task<ObservableCollection<ReadingItemModels>> GetReadingItemsAsync()
		{
			System.Diagnostics.Debug.WriteLine("Fetching items from the service...");
			if (!_isInitialized)
			{
				await InitializeAsync();
			}

			System.Diagnostics.Debug.WriteLine($"Items in service: {_items?.Count ?? 0}");
			return _items;
		}

		//public Task<ObservableCollection<ReadingItemModels>> GetCompletedItemsAsync()
		//{
		//	var completedItems = new ObservableCollection<ReadingItemModels>(
		//		_items.Where(item => item.IsCompleted)
		//	);
		//	return Task.FromResult(completedItems);
		//}
		public async Task<ObservableCollection<ReadingItemModels>> GetCompletedItemsAsync()
		{
			if (!_isInitialized)
			{
				await InitializeAsync();
			}

			var completedItems = new ObservableCollection<ReadingItemModels>(
				_items.Where(item => item.IsSubmitted)
			);
			return completedItems;
		}

		//public Task<ObservableCollection<ReadingItemModels>> GetUncompletedItemsAsync()
		//{
		//	var uncompletedItems = new ObservableCollection<ReadingItemModels>(
		//		_items.Where(item => !item.IsCompleted)
		//	);
		//	return Task.FromResult(uncompletedItems);
		//}

		public async Task<ObservableCollection<ReadingItemModels>> GetUncompletedItemsAsync()
		{
			if (!_isInitialized)
			{
				await InitializeAsync();
			}

			var uncompletedItems = new ObservableCollection<ReadingItemModels>(
				_items.Where(item => !item.IsSubmitted)
			);
			return uncompletedItems;
		}

		//public Task<ObservableCollection<ReadingItemModels>> SearchItemsAsync(string searchTerm)
		//{
		//	var filteredItems = new ObservableCollection<ReadingItemModels>(
		//		_items.Where(item =>
		//			item.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
		//			item.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
		//	);
		//	return Task.FromResult(filteredItems);
		//}

		public async Task<ObservableCollection<ReadingItemModels>> SearchItemsAsync(string searchTerm)
		{
			if (!_isInitialized)
			{
				await InitializeAsync();
			}

			var filteredItems = new ObservableCollection<ReadingItemModels>(
				_items.Where(item =>
					item.Title.Contains(searchTerm, StringComparison.OrdinalIgnoreCase) ||
					item.Description.Contains(searchTerm, StringComparison.OrdinalIgnoreCase))
			);
			return filteredItems;
		}


		// Helper methods to map additional data from tags
		private string ExtractDifficultyFromTags(IEnumerable<login_full.Models.Tag> tags)
		{
			var difficultyTag = tags.FirstOrDefault(tag => tag.Code.Equals("passage_1", StringComparison.OrdinalIgnoreCase));
			return difficultyTag?.Title ?? "Unknown";
		}

		private string ExtractCategoryFromTags(IEnumerable<login_full.Models.Tag> tags)
		{
			var categoryTag = tags.FirstOrDefault(tag => tag.Code.Equals("MATCHING_HEADING", StringComparison.OrdinalIgnoreCase));
			return categoryTag?.Title ?? "General";
		}
	}

}

