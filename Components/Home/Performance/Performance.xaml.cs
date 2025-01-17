﻿using Microsoft.UI.Xaml;
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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home.Performance
{
	/// <summary>
	/// Component chính quản lý hiển thị hiệu suất học tập
	/// </summary>
	/// <remarks>
	/// Bao gồm:
	/// - Target component (mục tiêu điểm số)
	/// - ExamRemain component (thông tin ngày thi)
	/// - Responsive layout cho các màn hình khác nhau
	/// </remarks>
	public sealed partial class Performance : UserControl
	{
		// public the Target component
		public Target TargetComponentControl => TargetComponent;
		public Performance()
		{
			this.InitializeComponent();
		}
	}
}
