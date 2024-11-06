﻿using login_full.Models;

namespace login_full
{
    internal class GlobalState
	{
		// Instance duy nhất của class
		private static GlobalState _instance;

		// Thuộc tính lưu Access Token hoặc các dữ liệu khác
		public string AccessToken { get; set; }

		public UserProfile UserProfile { get; set; }
		public UserTarget UserTarget { get; set; }

		// Private constructor để ngăn chặn khởi tạo từ bên ngoài
		private GlobalState() { }

		// Hàm để lấy instance duy nhất
		public static GlobalState Instance
		{
			get
			{
				if (_instance == null)
				{
					_instance = new GlobalState();
				}
				return _instance;
			}
		}
	}
}