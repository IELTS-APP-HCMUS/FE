using login_full.ViewModels;
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
using login_full.Services;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.UI;


// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home
{
    public sealed partial class History : UserControl
    {
        public HistoryViewModel ViewModel { get; }
        public History()
        {
            this.InitializeComponent();

            ViewModel = ServiceLocator.GetService<HistoryViewModel>();
            this.DataContext = ViewModel;
            this.Loaded += OnLoaded;
        }
        private void OnLoaded(object sender, RoutedEventArgs e)
        {
            ViewModel.RefreshHistory();
        }
        private void ByNameTab_Click(object sender, RoutedEventArgs e)
		{
			ByTimeTab.Background = new SolidColorBrush(Colors.White);
			ByTimeTab.Foreground = new SolidColorBrush(Colors.DarkCyan);
			ByNameTab.Background = new SolidColorBrush(Colors.Beige);
            ByNameTab.Foreground = new SolidColorBrush(Colors.Orange);
		}
		private void ByTimeTab_Click(object sender, RoutedEventArgs e)
		{
			ByNameTab.Background = new SolidColorBrush(Colors.White);
			ByNameTab.Foreground = new SolidColorBrush(Colors.DarkCyan);
			ByTimeTab.Background = new SolidColorBrush(Colors.Beige);
			ByTimeTab.Foreground = new SolidColorBrush(Colors.Orange);
		}
	}
}
