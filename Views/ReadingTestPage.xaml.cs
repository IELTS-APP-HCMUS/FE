using login_full.Models;
using login_full.Services;
using login_full.ViewModels;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using Windows.Foundation;
using Windows.UI.Text;
using Microsoft.UI.Text;
using Microsoft.UI;
using System.Text.RegularExpressions;
using System;
using System.Linq;
using System.Collections.Generic;
using Microsoft.UI.Xaml.Documents;
using Microsoft.UI.Xaml.Media;



namespace login_full.Views
{
	public sealed partial class ReadingTestPage : Page
	{
		public ReadingTestViewModel ViewModel { get; }

		public ReadingTestPage()
		{
			this.InitializeComponent();
            var readingTestService = ServiceLocator.GetService<IReadingTestService>();
            var navigationService = App.NavigationService;


            // Sử dụng NavigationService từ App
            ViewModel = new					(
                readingTestService,
                navigationService
            );
			//  Loaded += ReadingTestPage_Loaded;
		}

		//private async void ReadingTestPage_Loaded(object sender, RoutedEventArgs e)
		//{
		//    await ViewModel.LoadTestAsync("test1");
		//}
		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			if (e.Parameter is string item)
			{
				// Load test dựa trên TestId của item được chọn
				await ViewModel.LoadTestAsync(item);
                               ProcessContent();
            }
		}



        private void ProcessContent()
        {
            if (ViewModel.TestDetail?.Content == null) return;

            // Xóa nội dung cũ trong paragraph
            ContentParagraph.Inlines.Clear();

            // Tách văn bản thành các đoạn theo ký tự xuống dòng
            string[] paragraphs = ViewModel.TestDetail.Content.Split('\n');

            for (int p = 0; p < paragraphs.Length; p++)
            {
                string paragraph = paragraphs[p].Trim();
                if (string.IsNullOrEmpty(paragraph)) continue;

                // Tách đoạn thành các từ
                string[] words = paragraph.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i];

                    // Xử lý dấu câu
                    if (word.EndsWith(".") || word.EndsWith(",") ||
                        word.EndsWith("!") || word.EndsWith("?") ||
                        word.EndsWith(";") || word.EndsWith(":"))
                    {
                        string punctuation = word[^1].ToString();
                        string mainWord = word[..^1];

                        if (!string.IsNullOrEmpty(mainWord))
                        {
                            AddWordButton(mainWord);
                        }
                        ContentParagraph.Inlines.Add(new Run { Text = punctuation });
                    }
                    else
                    {
                        AddWordButton(word);
                    }

                    // Thêm khoảng trắng sau mỗi từ (trừ từ cuối cùng)
                    if (i < words.Length - 1)
                    {
                        ContentParagraph.Inlines.Add(new Run { Text = " " });
                    }
                }

                // Thêm xuống dòng sau mỗi đoạn (trừ đoạn cuối cùng)
                if (p < paragraphs.Length - 1)
                {
                    ContentParagraph.Inlines.Add(new LineBreak());
                    ContentParagraph.Inlines.Add(new LineBreak()); // Thêm dòng trống giữa các đoạn
                }
            }
        }

        private void AddWordButton(string word)
        {
            var button = new Button
            {
                Content = new TextBlock
                {
                    Text = word,
                    FontSize = 16,
                    TextWrapping = TextWrapping.NoWrap
                },
                Padding = new Thickness(0, 0, 0, 0),
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0)
            };

            button.Click += WordButton_Click;

            var container = new InlineUIContainer
            {
                Child = button
            };

            ContentParagraph.Inlines.Add(container);
        }

        private async void WordButton_Click(object sender, RoutedEventArgs e)
        {
            if (sender is Button button && button.Content is TextBlock textBlock)
            {
                string word = textBlock.Text;

                ContentDialog dialog = new ContentDialog
                {
                    Title = "Word Information",
                    Content = $"You clicked: {word}\nHere you can add dictionary definition, translation, or any other information.",
                    CloseButtonText = "Close",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }
    }
}