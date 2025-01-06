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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components
{
	/// <summary>
	/// Component hiển thị footer của ứng dụng
	/// </summary>
	/// <remarks>
	/// Hiển thị:
	/// - Logo và slogan
	/// - Social media links
	/// - Thông tin liên hệ
	/// - Copyright
	/// </remarks>
	public sealed partial class Footer : UserControl
	{
		public Footer()
		{
			this.InitializeComponent();
		}
	}
}
