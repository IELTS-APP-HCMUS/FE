using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Windowing;
using Windows.Graphics;
using System;

namespace login_full
{
	public sealed partial class MainWindow : Window
	{
		private const int MinWindowWidth = 850;
		private const int MinWindowHeight = 600;
		/// <summary>
		/// Khởi tạo lớp `MainWindow`
		/// </summary>
		public MainWindow()
		{
			this.InitializeComponent();
			// Set the global MainFrame
			App.MainWindow = this;

			this.SizeChanged += MainWindow_SizeChanged;
		}
		/// <summary>
		/// Khởi tạo lớp `MainWindow_SizeChanged` để xử lý sự kiện thay đổi kích thước cửa sổ.
		/// </summary>
		private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
		{
			IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
			var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
			var appWindow = AppWindow.GetFromWindowId(windowId);

			var currentSize = appWindow.Size;
			int newWidth = Math.Max(currentSize.Width, MinWindowWidth);
			int newHeight = Math.Max(currentSize.Height, MinWindowHeight);

			appWindow.Resize(new SizeInt32(newWidth, newHeight));
		}
		/// <summary>
		/// Khởi tạo lớp `MainFrame_NavigationFailed` để xử lý sự kiện thất bại khi điều hướng.
		/// </summary>
		private void MainFrame_NavigationFailed(object sender, NavigationFailedEventArgs e)
		{
			System.Diagnostics.Debug.WriteLine($"Navigation failed: {e.SourcePageType.FullName}");
		}
	}
}
