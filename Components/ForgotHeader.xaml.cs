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

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components
{

	/// <summary>
	/// Component header cho trang quên m?t kh?u
	/// </summary>
	/// <remarks>
	/// Hi?n th?:
	/// - Nút quay l?i trang login
	/// - Tiêu ??
	/// - Mô t?
	/// </remarks>
	public sealed partial class ForgotHeader : UserControl
    {
        public ForgotHeader()
        {
            this.InitializeComponent();
        }
		// Dependency Property for UserName
		public static readonly DependencyProperty TitleProperty =
			DependencyProperty.Register("Title", typeof(string), typeof(ForgotHeader), new PropertyMetadata(string.Empty));

		public string Title
		{
			get => (string)GetValue(TitleProperty);
			set => SetValue(TitleProperty, value);
		}

		// Dependency Property for UserEmail
		public static readonly DependencyProperty DescribeProperty =
			DependencyProperty.Register("Describe", typeof(string), typeof(ForgotHeader), new PropertyMetadata(string.Empty));

		public string Describe
		{
			get => (string)GetValue(DescribeProperty);
			set => SetValue(DescribeProperty, value);
		}
	}
}
