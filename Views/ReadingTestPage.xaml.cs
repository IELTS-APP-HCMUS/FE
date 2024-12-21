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
        private readonly MockDictionaryService _dictionaryService;

        public ReadingTestPage()
        {
            this.InitializeComponent();
            var readingTestService = ServiceLocator.GetService<IReadingTestService>();
            var navigationService = App.NavigationService;
            _dictionaryService = new MockDictionaryService();


            // Sử dụng NavigationService từ App
            ViewModel = new(
                readingTestService,
                navigationService
            );
            //  Loaded += ReadingTestPage_Loaded;
            ViewModel.OnContentProcessingRequested += ViewModel_OnContentProcessingRequested;
          
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

        private void ViewModel_OnContentProcessingRequested(object sender, EventArgs e)
        {
            ProcessContent();
        }

        private void ProcessContent()
        {
            if (ViewModel.TestDetail?.Content == null) return;

            // Xóa nội dung cũ trong paragraph
            ContentParagraph.Inlines.Clear();

            if (ViewModel.IsVocabMode)
            {
                ProcessVocabMode();
            }
            else
            {
                ProcessNormalMode();
            }
        }

        private void ProcessNormalMode()
        {
            // Hiển thị văn bản bình thường không có button
            string[] paragraphs = ViewModel.TestDetail.Content.Split('\n');

            for (int p = 0; p < paragraphs.Length; p++)
            {
                string paragraph = paragraphs[p].Trim();
                if (string.IsNullOrEmpty(paragraph)) continue;

                ContentParagraph.Inlines.Add(new Run { Text = paragraph });

                // Thêm xuống dòng sau mỗi đoạn (trừ đoạn cuối)
                if (p < paragraphs.Length - 1)
                {
                    ContentParagraph.Inlines.Add(new LineBreak());
                    ContentParagraph.Inlines.Add(new LineBreak());
                }
            }
        }

        private void ProcessVocabMode()
        {
            // Code xử lý vocab mode (code cũ của ProcessContent)
            string[] paragraphs = ViewModel.TestDetail.Content.Split('\n');

            for (int p = 0; p < paragraphs.Length; p++)
            {
                string paragraph = paragraphs[p].Trim();
                if (string.IsNullOrEmpty(paragraph)) continue;

                string[] words = paragraph.Split(' ');

                for (int i = 0; i < words.Length; i++)
                {
                    string word = words[i];

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

                    if (i < words.Length - 1)
                    {
                        ContentParagraph.Inlines.Add(new Run { Text = " " });
                    }
                }

                if (p < paragraphs.Length - 1)
                {
                    ContentParagraph.Inlines.Add(new LineBreak());
                    ContentParagraph.Inlines.Add(new LineBreak());
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
                    FontSize = 14,
                    TextWrapping = TextWrapping.NoWrap
                },
                Padding = new Thickness(0, 0, 0, 0),
                Background = new SolidColorBrush(Colors.Transparent),
                BorderThickness = new Thickness(0),
                Margin = new Thickness(0,0,0,-4.5),
                
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
                var entry = _dictionaryService.GetWord(word);

                if (entry == null)
                {
                    return;
                }

                ContentDialog dialog = new ContentDialog
                {
                    Title = "Dictionary",
                    Content = entry,
                    ContentTemplate = (DataTemplate)Resources["DictionaryDialogTemplate"],
                    Style = (Style)Resources["DictionaryDialogStyle"],
                    CloseButtonText = "Close",
                    XamlRoot = this.XamlRoot
                };

                await dialog.ShowAsync();
            }
        }
        //private async void WordButton_Click(object sender, RoutedEventArgs e)
        //{
        //    if (sender is Button button && button.Content is TextBlock textBlock)
        //    {
        //        string word = textBlock.Text;
        //        var entry = _dictionaryService.GetWord(word);

        //        if (entry == null)
        //        {
        //            //ContentDialog dialog = new ContentDialog
        //            //{
        //            //    Title = "Word Not Found",
        //            //    Content = $"The word '{word}' was not found in the dictionary.",
        //            //    CloseButtonText = "Close",
        //            //    XamlRoot = this.XamlRoot
        //            //};
        //            //await dialog.ShowAsync();
        //            return;
        //        }

        //        var content = new StackPanel { Spacing = 10 };

        //        // Pronunciation
        //        var pronunciationPanel = new StackPanel { Orientation = Orientation.Horizontal, Spacing = 10 };
        //        pronunciationPanel.Children.Add(new TextBlock 
        //        { 
        //            Text = entry.Word,
        //            FontSize = 20,
        //            FontWeight = FontWeights.Bold
        //        });
        //        pronunciationPanel.Children.Add(new TextBlock 
        //        { 
        //            Text = entry.Pronunciation,
        //            FontStyle = FontStyle.Italic
        //        });
        //        content.Children.Add(pronunciationPanel);

        //        // Part of Speech
        //        content.Children.Add(new TextBlock 
        //        { 
        //            Text = entry.PartOfSpeech,
        //            FontStyle = FontStyle.Italic,
        //            Foreground = new SolidColorBrush(Colors.Gray)
        //        });

        //        // English Meaning
        //        content.Children.Add(new TextBlock 
        //        { 
        //            Text = "Definition:",
        //            FontWeight = FontWeights.SemiBold
        //        });
        //        content.Children.Add(new TextBlock 
        //        { 
        //            Text = entry.Meaning,
        //            TextWrapping = TextWrapping.Wrap
        //        });

        //        // Vietnamese Meaning
        //        content.Children.Add(new TextBlock 
        //        { 
        //            Text = "Nghĩa:",
        //            FontWeight = FontWeights.SemiBold,
        //            Margin = new Thickness(0,10,0,0)
        //        });
        //        content.Children.Add(new TextBlock 
        //        { 
        //            Text = entry.VietnameseMeaning,
        //            TextWrapping = TextWrapping.Wrap
        //        });

        //        // Related Words
        //        content.Children.Add(new TextBlock 
        //        { 
        //            Text = "Related Words:",
        //            FontWeight = FontWeights.SemiBold,
        //            Margin = new Thickness(0,10,0,0)
        //        });
        //        var relatedWordsPanel = new ItemsControl
        //        {
        //            ItemsSource = entry.RelatedWords,
        //            Margin = new Thickness(10,0,0,0)
        //        };
        //        content.Children.Add(relatedWordsPanel);

        //        // Examples
        //        content.Children.Add(new TextBlock 
        //        { 
        //            Text = "Examples:",
        //            FontWeight = FontWeights.SemiBold,
        //            Margin = new Thickness(0,10,0,0)
        //        });
        //        var examplesPanel = new ItemsControl
        //        {
        //            ItemsSource = entry.Examples,
        //            Margin = new Thickness(10,0,0,0)
        //        };
        //        content.Children.Add(examplesPanel);

        //        ContentDialog dialog = new ContentDialog
        //        {
        //            Title = "Dictionary",
        //            Content = content,
        //            CloseButtonText = "Close",
        //            XamlRoot = this.XamlRoot
        //        };

        //        await dialog.ShowAsync();
        //    }
        //}
    }
}