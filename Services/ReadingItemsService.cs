using login_full.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using login_full.Context;
using login_full.API_Services;

namespace login_full.Services
{
	public class ReadingItemsService : IReadingItemsService
	{
		public readonly ObservableCollection<ReadingItemModels> _items ;
		private bool _isInitialized;
		private readonly ClientCaller _clientCaller;

		// Public property to expose _items
		public ReadingItemsService()
		{
			_items = new ObservableCollection<ReadingItemModels> { };
            _clientCaller = new ClientCaller();
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
				
				await FetchItemsFromAPIAsync(1,25);

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
					string url = $"/v1/quizzes?{queryString}";
				
					HttpResponseMessage response = await _clientCaller.GetAsync(url);

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
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"Exception in FetchItemsFromAPIAsync: {ex.Message}");
			}
		}



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

		private string ExtractDifficultyFromTags(IEnumerable<login_full.Models.Tag> tags)
		{
			var passageTag = tags.FirstOrDefault(tag =>
				tag.Code.StartsWith("passage_", StringComparison.OrdinalIgnoreCase));
			return passageTag?.Title ?? "Unknown";
		}

		private string ExtractCategoryFromTags(IEnumerable<login_full.Models.Tag> tags)
		{
			// Look for tags related to reading question types
			var questionTypeTag = tags.FirstOrDefault(tag =>
				tag.Code.Equals("MATCHING_HEADING", StringComparison.OrdinalIgnoreCase) ||
				tag.Code.Equals("MATCHING_INFO", StringComparison.OrdinalIgnoreCase) ||
				tag.Code.Equals("MULTIPLE_CHOICE_MANY", StringComparison.OrdinalIgnoreCase) ||
				tag.Code.Equals("MAP_DIAGRAM_LABEL", StringComparison.OrdinalIgnoreCase) ||
				tag.Code.Equals("OTHERS", StringComparison.OrdinalIgnoreCase) ||
				tag.Code.Equals("FILL_BLANK", StringComparison.OrdinalIgnoreCase) ||
				tag.Code.Equals("TRUE_FALSE", StringComparison.OrdinalIgnoreCase) ||
				tag.Code.Equals("YES_NO", StringComparison.OrdinalIgnoreCase) ||
				tag.Code.Equals("MULTIPLE_CHOICE_ONE", StringComparison.OrdinalIgnoreCase));

			return questionTypeTag?.Title ?? "General";
		}
	}

}

