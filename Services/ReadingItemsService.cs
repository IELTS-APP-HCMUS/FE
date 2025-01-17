﻿using login_full.Models;
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
    /// <summary>
    // Service quản lý các mục đọc từ API.
    // Cung cấp các chức năng tải, tìm kiếm, và phân loại các mục đọc.
    // </summary>
    public class ReadingItemsService : IReadingItemsService
	{
        /// <summary>
        /// Danh sách các mục đọc có thể quan sát thay đổi.
        /// </summary>
        public readonly ObservableCollection<ReadingItemModels> _items ;
        /// <summary>
        /// Trạng thái khởi tạo của service.
        /// </summary>
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
        /// <summary>
        /// Lấy các mục đọc từ API với các tham số tùy chọn.
        /// </summary>
        /// <param name="pageNumber">Số trang</param>
        /// <param name="pageSize">Kích thước trang</param>
        /// <param name="submittedStatus">Trạng thái đã nộp</param>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <param name="type">Loại mục đọc</param>
        /// <param name="tagPassage">Thẻ đoạn văn</param>
        /// <param name="tagQuestionType">Thẻ loại câu hỏi</param>
        private async Task FetchItemsFromAPIAsync(
	int? pageNumber = null,
	int? pageSize = null,
	int? submittedStatus = null,
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
					var apiResponse = JsonConvert.DeserializeObject<ApiResponse>(stringResponse);

					_items.Clear();

					foreach (var item in apiResponse.Data.Items)
					{
						var mappedItem = new ReadingItemModels
						{
							TestId = item.Id.ToString(),
							Title = item.Title ?? "Default Title",
							Description = item.ShortDescription ?? "Default Description",
							Duration = $"{item.Time} mins",
							Tags = item.Tags,
							Difficulty = ExtractDifficultyFromTags(item.Tags),
							Category = ExtractCategoryFromTags(item.Tags),
							ImagePath = !string.IsNullOrEmpty(item.Thumbnail)
										? item.Thumbnail 
										: "/Assets/reading_win.png",
							IsSubmitted = item.IsSubmitted,
						};
						
						_items.Add(mappedItem);
						System.Diagnostics.Debug.WriteLine($"Image URL: {mappedItem.ImagePath}");
						System.Diagnostics.Debug.WriteLine($"Mapped item: Id={mappedItem.TestId}, Title={mappedItem.Title}, Description={mappedItem.Description}, Duration={mappedItem.Duration}, Difficulty={mappedItem.Difficulty}, Category={mappedItem.Category}, IsSubmitted={mappedItem.IsSubmitted}, ImagePath= {mappedItem.ImagePath}");
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



        /// <summary>
        /// Lấy danh sách các mục đọc.
        /// </summary>
        /// <returns>Danh sách các mục đọc</returns>
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
        /// <summary>
        /// Lấy danh sách các mục đọc đã hoàn thành.
        /// </summary>
        /// <returns>Danh sách các mục đọc đã hoàn thành</returns>
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
        /// <summary>
        /// Lấy danh sách các mục đọc chưa hoàn thành.
        /// </summary>
        /// <returns>Danh sách các mục đọc chưa hoàn thành</returns>
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

        /// <summary>
        /// Tìm kiếm các mục đọc theo từ khóa.
        /// </summary>
        /// <param name="searchTerm">Từ khóa tìm kiếm</param>
        /// <returns>Danh sách các mục đọc phù hợp</returns>
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
        /// <summary>
        /// Trích xuất độ khó từ các thẻ.
        /// </summary>
        /// <param name="tags">Danh sách thẻ</param>
        /// <returns>Độ khó</returns>
        private string ExtractDifficultyFromTags(IEnumerable<Tag> tags)
		{
			var passageTag = tags?.FirstOrDefault(tag =>
				tag.TagPositions != null &&
				tag.TagPositions.Any(pos => pos.Position == "reading_passage_search"));

			return passageTag?.Title ?? "Unknown";
		}
        /// <summary>
        /// Trích xuất thể loại từ các thẻ.
        /// </summary>
        /// <param name="tags">Danh sách thẻ</param>
        /// <returns>Thể loại</returns>
        private string ExtractCategoryFromTags(IEnumerable<Tag> tags)
		{
			var questionTypeTag = tags?.FirstOrDefault(tag =>
				tag.TagPositions != null &&
				tag.TagPositions.Any(pos => pos.Position == "reading_question_types_search"));

			return questionTypeTag?.Title ?? "General";
		}
	}

}

