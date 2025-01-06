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
using login_full.Models;
using login_full.Views;
using login_full.Services;
using login_full.ViewModels;
using System.Threading.Tasks;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home
{
	/// <summary>
	/// Component hiển thị các bài học đề xuất
	/// </summary>
	/// <remarks>
	/// Hiển thị:
	/// - Grid 4 cards bài học đề xuất
	/// - Thông tin cơ bản về mỗi bài học
	/// - Start Now button
	/// </remarks>
	public sealed partial class Suggestion : UserControl
    {
        private SuggestionViewModel ViewModel { get; }
        public Suggestion()
        {
            this.InitializeComponent();
            var readingItemsService = new ReadingItemsService(); // Create or get the service instance
            ViewModel = new SuggestionViewModel(readingItemsService);

            // Set the DataContext to the ViewModel
            this.DataContext = ViewModel;

            // Load items asynchronously
            _ = LoadSuggestionsAsync(); // Fire and forget
            //var readingItemsService = new ReadingItemsService();
            //this.DataContext = new SuggestionViewModel(readingItemsService);
        }
        private async Task LoadSuggestionsAsync()
        {
            await ViewModel.LoadItemsAsync();
        }
        private async void StartTest_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as Button;
            var item = button.DataContext as ReadingItemModels;
            await App.NavigationService.NavigateToAsync(typeof(ReadingTestPage), item.TestId);
            //Frame.Navigate(typeof(Views.ReadingTestPage), item.TestId);
        }
    }
}