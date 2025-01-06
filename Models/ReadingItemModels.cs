using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.CompilerServices;
using Microsoft.UI.Xaml.Media.Imaging;
using Newtonsoft.Json;
using System.Net.Http;
using System.IO;
using System.Threading.Tasks;
using Windows.Storage.Streams;
namespace login_full.Models
{
	// Reading Item Model
	/// <summary>
	/// Model cho một bài reading test
	/// </summary>
	/// <remarks>
	/// Chứa thông tin cơ bản về bài test:
	/// - Tiêu đề, mô tả
	/// - Thời gian làm bài
	/// - Độ khó
	/// - Trạng thái hoàn thành
	/// </remarks>
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

		public string Difficulty { get; set; } 
		public string Category { get; set; }  

		[JsonProperty("thumbnail")]
		public string ImagePath { get; set; }

		[JsonProperty("is_submitted")]
		public bool IsSubmitted { get; set; }

		[JsonProperty("tags")]
		public List<Tag> Tags { get; set; }


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

	/// <summary>
	/// Model cho tag của bài test
	/// </summary>
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
