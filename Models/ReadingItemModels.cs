using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace login_full.Models
{
    public class ReadingItemModels : INotifyPropertyChanged
    {
        public string TestId { get; set; }
        public string Title { get; set; }
        public string Description { get; set; }
        public string Duration { get; set; }
        public string Difficulty { get; set; }
        public string Category { get; set; }
        public string ImagePath { get; set; }
        public bool IsSubmitted { get; set; }

        public event PropertyChangedEventHandler PropertyChanged;

        protected void OnPropertyChanged([CallerMemberName] string name = null)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
        }
    }

	public class Tag : INotifyPropertyChanged
	{
		public string Code { get; set; }
		public string Title { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	public class ApiResponse : INotifyPropertyChanged
	{
		public int Code { get; set; }
		public string Message { get; set; }
		public ApiResponseData Data { get; set; }

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}

	public class ApiResponseData : INotifyPropertyChanged
	{
		
		public int Total { get; set; }
		public int Page { get; set; }
		public int PageSize { get; set; }
		public List<ApiQuizItem> Items { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}

	}

	public class ApiQuizItem : INotifyPropertyChanged
	{
		public int Id { get; set; }
		public string Title { get; set; }
		public string ShortDescription { get; set; }
		public int Time { get; set; }

		[JsonProperty("is_submitted")] 
		public bool IsSubmitted { get; set; }
		public string Thumbnail { get; set; }
		public List<Tag> Tags { get; set; }
		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged([CallerMemberName] string name = null)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(name));
		}
	}
}
