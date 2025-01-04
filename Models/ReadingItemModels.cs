using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using System.Threading;

namespace login_full.Models
{
	// Reading Item Model
	public class ReadingItemModels : INotifyPropertyChanged
	{
		[JsonProperty("id")]
		public string TestId { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string Description { get; set; }

		[JsonProperty("time")]
		public string Duration { get; set; }

		public string Difficulty { get; set; } // Extracted from Tags
		public string Category { get; set; }   // Extracted from Tags

		[JsonProperty("thumbnail")]
		public string ImagePath { get; set; }

		[JsonProperty("is_submitted")]
		public bool IsSubmitted { get; set; }

		[JsonProperty("tags")]
		public List<Tag> Tags { get; set; }

		// New property for Image Binding
		private BitmapImage _imageBitmap;
		public BitmapImage ImageBitmap
		{
			get => _imageBitmap;
			set
			{
				_imageBitmap = value;
				OnPropertyChanged();
			}
		}

		
		private static readonly SemaphoreSlim Semaphore = new SemaphoreSlim(1);

		public async void SetImageBitmap()
		{
			System.Diagnostics.Debug.WriteLine($"Setting ImageBitmap: {ImagePath}");
			try
			{
				if (!string.IsNullOrEmpty(ImagePath))
				{
					await Semaphore.WaitAsync(); 

					using (var httpClient = new HttpClient())
					{
						httpClient.Timeout = TimeSpan.FromSeconds(10); 

						var response = await httpClient.GetAsync(new Uri(ImagePath));

						if (response.IsSuccessStatusCode)
						{
							var stream = await response.Content.ReadAsStreamAsync();
							var bitmap = new BitmapImage();
							await bitmap.SetSourceAsync(stream.AsRandomAccessStream());
							ImageBitmap = bitmap;

							System.Diagnostics.Debug.WriteLine($"[SUCCESS] Loaded image from URL: {ImagePath}");
						}
						else if (response.StatusCode == System.Net.HttpStatusCode.TooManyRequests)
						{
							System.Diagnostics.Debug.WriteLine($"[ERROR] Too Many Requests. Retrying after delay.");
							await Task.Delay(2000); 
							SetImageBitmap();      
						}
						else
						{
							ImageBitmap = new BitmapImage(new Uri("ms-appx:///Assets/reading_win.png"));
							System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to load image. HTTP Status: {response.StatusCode}");
						}
					}
				}
				else
				{
					ImageBitmap = new BitmapImage(new Uri("ms-appx:///Assets/reading_win.png"));
					System.Diagnostics.Debug.WriteLine("[WARNING] ImagePath was empty. Loaded default image.");
				}
			}
			catch (Exception ex)
			{
				System.Diagnostics.Debug.WriteLine($"[ERROR] Failed to load image: {ex.Message}");
			}
			finally
			{
				Semaphore.Release(); // Giải phóng lượt tải sau khi hoàn thành.
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	// API Response Model
	public class ApiResponse : INotifyPropertyChanged
	{
		[JsonProperty("code")]
		public int Code { get; set; }

		[JsonProperty("message")]
		public string Message { get; set; }

		[JsonProperty("data")]
		public ApiResponseData Data { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	// API Response Data Model
	public class ApiResponseData : INotifyPropertyChanged
	{
		[JsonProperty("total")]
		public int Total { get; set; }

		[JsonProperty("page")]
		public int Page { get; set; }

		[JsonProperty("page_size")]
		public int PageSize { get; set; }

		[JsonProperty("items")]
		public List<ApiQuizItem> Items { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	// API Quiz Item Model
	public class ApiQuizItem : INotifyPropertyChanged
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("description")]
		public string ShortDescription { get; set; }

		[JsonProperty("time")]
		public int Time { get; set; }

		[JsonProperty("is_submitted")]
		public bool IsSubmitted { get; set; }

		[JsonProperty("thumbnail")]
		public string Thumbnail { get; set; }

		[JsonProperty("tags")]
		public List<Tag> Tags { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	// Tag Model
	public class Tag : INotifyPropertyChanged
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("title")]
		public string Title { get; set; }

		[JsonProperty("code")]
		public string Code { get; set; }

		[JsonProperty("tag_positions")]
		public List<TagPosition> TagPositions { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	// Tag Position Model
	public class TagPosition : INotifyPropertyChanged
	{
		[JsonProperty("id")]
		public int Id { get; set; }

		[JsonProperty("position")]
		public string Position { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
