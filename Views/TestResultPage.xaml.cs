using ABI.System;
using login_full.Services;
using login_full.ViewModels;
using Microsoft.UI;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;



namespace login_full.Views
{
    /// <summary>
    /// Tang hiển thị kết quả tổng quan của bài kiểm tra.
    // Hiển thị biểu đồ và thống kê về kết quả làm bài.
    // </summary>
    public sealed partial class TestResultPage : Page
	{
        /// <summary>
        /// ViewModel quản lý logic và dữ liệu cho trang Test Result
        /// </summary>
        public TestResultViewModel ViewModel { get; private set; }
        /// <summary>
        /// Khởi tạo trang Test Result
        /// </summary>
        public TestResultPage()
		{
			this.InitializeComponent();
		}
        /// <summary>
        /// Xử lý sự kiện khi điều hướng đến trang
        /// Tải dữ liệu kết quả và vẽ biểu đồ thống kê
        /// </summary>
        /// <param name="e">Tham số điều hướng chứa thông tin để tải kết quả bài kiểm tra</param>
        protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			
			ViewModel = (App.Current as App)?.CurrentTestResult;
			if (e.Parameter is string item)
			{
				await ViewModel.LoadSummaryAsync(item);
			}
			if (ViewModel != null)
			{
				this.DataContext = ViewModel;
				ViewModel.DrawChart(PieChart, 100, 100, 80);
			}
		}

	}
}
