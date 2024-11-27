using login_full.Models;
using System;
using System.ComponentModel;

namespace login_full.Context
{
    public class GlobalState : INotifyPropertyChanged
	{
		private static readonly Lazy<GlobalState> _instance = new(() => new GlobalState());
		// Instance duy nhất của class
		//private static GlobalState _instance;

        // Thuộc tính lưu Access Token hoặc các dữ liệu khác
        public string AccessToken { get; set; }
		private UserProfile _userProfile;
		private UserTarget _userTarget;
		//public UserProfile UserProfile { get; set; }
		public UserProfile UserProfile
		{
			get => _userProfile;
			set
			{
				if (_userProfile != value)
				{
					_userProfile = value;
					OnPropertyChanged(nameof(UserProfile));
				}
			}
		}
		public UserTarget UserTarget
		{
			get => _userTarget;
			set
			{
				if (_userTarget != value)
				{
					_userTarget = value;
					OnPropertyChanged(nameof(UserTarget));
				}
			}
		}
		// Private constructor để ngăn chặn khởi tạo từ bên ngoài
		private GlobalState() { }

        // Hàm để lấy instance duy nhất
        public static GlobalState Instance
        {
            get
            {
				//_instance ??= new GlobalState();
				/*
				 if (_instance == null)
                {
                    _instance = new GlobalState();
				}
				 */
				return _instance.Value;
            }
        }
		public event PropertyChangedEventHandler PropertyChanged;
		protected void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
	}
}