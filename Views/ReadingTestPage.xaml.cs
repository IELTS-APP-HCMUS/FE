﻿using login_full.Models;
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
using System.Globalization;


namespace login_full.Views
{
    public sealed partial class ReadingTestPage : Page
    {

        
        public ReadingTestViewModel ViewModel { get; }
        private readonly MockDictionaryService _dictionaryService;

        private TextHighlightService _highlightService;
        private string _selectedText;
        private int _selectedStartIndex;
        private int _selectedLength;
        private DispatcherTimer _popupTimer;

        public ReadingTestPage()
        {
            this.InitializeComponent();

            try
            {
                var readingTestService = ServiceLocator.GetService<IReadingTestService>();
                var navigationService = App.NavigationService;
                var pdfExportService = ServiceLocator.GetService<IPdfExportService>();
                _dictionaryService = ServiceLocator.GetService<MockDictionaryService>();

                _highlightService = ServiceLocator.GetService<TextHighlightService>();

                if (readingTestService == null || navigationService == null || pdfExportService == null)
                {
                    throw new InvalidOperationException("Required services are not initialized");
                }

                // Sử dụng NavigationService từ App
                ViewModel = new ReadingTestViewModel(
                    readingTestService,
                    navigationService,
                    pdfExportService
                );

                ViewModel.OnContentProcessingRequested += ViewModel_OnContentProcessingRequested;
                ViewModel.OnHighlightModeChanged += ViewModel_OnHighlightModeChanged;

                // Khởi tạo timer cho popup
                _popupTimer = new DispatcherTimer();
                _popupTimer.Interval = TimeSpan.FromMilliseconds(350); // Đợi 500ms
                _popupTimer.Tick += PopupTimer_Tick;
            }
            catch (Exception ex)
            {
                // Log lỗi hoặc hiển thị thông báo
                ShowErrorDialog("Initialization Error", ex.Message);
            }
        }

        private async void ShowErrorDialog(string title, string message)
        {
            ContentDialog errorDialog = new ContentDialog
            {
                Title = title,
                Content = message,
                CloseButtonText = "OK",
                XamlRoot = this.XamlRoot
            };

            await errorDialog.ShowAsync();
        }

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

        private void ContentRichTextBlock_SelectionChanged(object sender, RoutedEventArgs e)
        {
            var richTextBlock = sender as RichTextBlock;
            if (richTextBlock != null && ViewModel.IsHighlightMode)
            {
                var selectedText = richTextBlock.SelectedText;
                if (!string.IsNullOrEmpty(selectedText))
                {
                    _selectedText = selectedText;

                    // Tìm vị trí thực của text được chọn trong nội dung
                    var fullText = GetFullText(richTextBlock);
                    _selectedStartIndex = fullText.IndexOf(selectedText);
                    _selectedLength = selectedText.Length;

                    // Get the selection bounds
                    var start = richTextBlock.SelectionStart;
                    var bounds = start.GetCharacterRect(LogicalDirection.Forward);
                    HighlightPopup.HorizontalOffset = bounds.X;
                    HighlightPopup.VerticalOffset = bounds.Y + 240;
                    // HighlightPopup.IsOpen = true;
                    // Restart timer
                    _popupTimer.Stop();
                    _popupTimer.Start();
                }
            }
            else
            {
                HighlightPopup.IsOpen = false;
                _popupTimer.Stop();
            }
        }

        // Helper method để lấy toàn bộ text từ RichTextBlock
        private string GetFullText(RichTextBlock richTextBlock)
        {
            string text = string.Empty;
            foreach (var block in richTextBlock.Blocks)
            {
                if (block is Paragraph paragraph)
                {
                    foreach (var inline in paragraph.Inlines)
                    {
                        if (inline is Run run)
                        {
                            text += run.Text;
                        }
                    }
                }
            }
            return text;
        }

        private void HighlightButton_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(_selectedText)) return;

            _highlightService.AddHighlight(
                "current_document_id",
                _selectedText,
                _selectedStartIndex,
                _selectedLength
            );
            ApplyHighlights();
            HighlightPopup.IsOpen = false;
        }

        private void RemoveHighlightButton_Click(object sender, RoutedEventArgs e)
        {
            // Tìm tất cả highlights có overlap với vùng được chọn
            var highlights = _highlightService.GetHighlights("current_document_id");
            var overlappingHighlights = highlights.Where(h =>
                HasOverlap(_selectedStartIndex, _selectedLength, h.StartIndex, h.Length)).ToList();

            // Xóa tất cả highlights bị overlap
            foreach (var highlight in overlappingHighlights)
            {
                _highlightService.RemoveHighlight("current_document_id", highlight.StartIndex, highlight.Length);
            }
            ApplyHighlights();
            HighlightPopup.IsOpen = false;
        }
        // Helper method để kiểm tra overlap giữa hai đoạn text
        private bool HasOverlap(int start1, int length1, int start2, int length2)
        {
            int end1 = start1 + length1;
            int end2 = start2 + length2;
            return !(end1 <= start2 || start1 >= end2);
        }

        private void ApplyHighlights()
        {
            var highlights = _highlightService.GetHighlights("current_document_id");
            var text = ViewModel.TestDetail.Content;

            // Clear existing highlights
            ContentRichTextBlock.TextHighlighters.Clear();

            foreach (var highlight in highlights.OrderBy(h => h.StartIndex))
            {
                var textHighlighter = new TextHighlighter();
                textHighlighter.Background = new SolidColorBrush(ColorHelper.FromArgb(
                    255,
                    byte.Parse(highlight.Color.Substring(1, 2), NumberStyles.HexNumber),
                    byte.Parse(highlight.Color.Substring(3, 2), NumberStyles.HexNumber),
                    byte.Parse(highlight.Color.Substring(5, 2), NumberStyles.HexNumber)));
                textHighlighter.Ranges.Add(new TextRange
                {
                    StartIndex = highlight.StartIndex,
                    Length = highlight.Length
                });

                ContentRichTextBlock.TextHighlighters.Add(textHighlighter);
            }
        }

        private void ViewModel_OnHighlightModeChanged(object sender, bool isHighlightMode)
        {
            // Cập nhật visual state của nút highlight
            MainHighlightButton.IsChecked = isHighlightMode;

            // Nếu tắt highlight mode thì đóng popup
            if (!isHighlightMode)
            {
                HighlightPopup.IsOpen = false;
            }
        }
        private void MainHighlightButton_Click(object sender, RoutedEventArgs e)
        {
            var button = sender as AppBarToggleButton;
            if (button != null)
            {
                ViewModel.IsHighlightMode = button.IsChecked ?? false;
            }
        }

        private void PopupTimer_Tick(object sender, object e)
        {
            _popupTimer.Stop();
            if (!string.IsNullOrEmpty(_selectedText))
            {
                HighlightPopup.IsOpen = true;
            }
        }
    }
}