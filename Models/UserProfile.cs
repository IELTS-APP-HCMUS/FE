using System.ComponentModel;

namespace login_full.Models
{
	/// <summary>
	/// Model quản lý thông tin cá nhân của người dùng
	/// </summary>
	/// <remarks>
	/// Lưu trữ thông tin cơ bản:
	/// - Tên người dùng
	/// - Email
	/// </remarks>
	public class UserProfile : INotifyPropertyChanged
	{
		private string _name;
		private string _email;

		public string Name { 
			get => _name;
			set
			{
				if (_name != value)
				{
					_name = value;
					OnPropertyChanged(nameof(Name));
				}
			}
		}
		public string Email
		{
			get => _email;
			set
			{
				if (_email != value)
				{
					_email = value;
					OnPropertyChanged(nameof(Email));
				}
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;

		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}