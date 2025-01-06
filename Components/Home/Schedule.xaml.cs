using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using login_full.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home
{

	/// <summary>
	/// Component quản lý lịch học trong ngày
	/// </summary>
	/// <remarks>
	/// Chức năng:
	/// - Hiển thị danh sách sự kiện
	/// - Thêm sự kiện mới
	/// - Quản lý thời gian
	/// </remarks>
	public sealed partial class Schedule : UserControl
    {
		private readonly ScheduleManager scheduleManager = null;
		public Schedule()
        {
            this.InitializeComponent();
			scheduleManager = new ScheduleManager(ScheduleListView);
            this.Loaded += ScheduleUserControl_Loaded;
        }
		private void AddNewEvent_Click(object sender, RoutedEventArgs e)
		{
			scheduleManager.AddNewEvent(this.XamlRoot);
		}
        private async void ScheduleUserControl_Loaded(object sender, RoutedEventArgs e)
        {
            // Lấy lịch từ API và cập nhật giao diện
            await scheduleManager.UpdateSchedulesAsync();
        }
    }
}
