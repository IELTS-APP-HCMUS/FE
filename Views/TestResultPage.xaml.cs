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

	public sealed partial class TestResultPage : Page
	{
		public TestResultViewModel ViewModel { get; private set; }

		public TestResultPage()
		{
			this.InitializeComponent();
		}

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
