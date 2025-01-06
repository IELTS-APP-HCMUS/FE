using Microsoft.UI.Xaml.Controls;
using login_full.ViewModels;

namespace login_full.Views
{
    /// <summary>
    ///Trang quản lý từ vựng.
    // Cho phép người dùng xem, tìm kiếm và quản lý danh sách từ vựng đã lưu.
    // </summary>
    public sealed partial class VocabularyPage : Page
    {
        /// <summary>
        /// ViewModel quản lý logic và dữ liệu cho trang Vocabulary
        /// </summary>
        private VocabularyViewModel _viewModel;

        /// <summary>
        /// Khởi tạo trang Vocabulary và thiết lập ViewModel
        /// </summary>
        public VocabularyViewModel ViewModel
        {
            get => _viewModel;
            private set => _viewModel = value;
        }

        public VocabularyPage()
        {
            InitializeComponent();
            ViewModel = new VocabularyViewModel();
            this.DataContext = ViewModel;
        }

        /// <summary>
        /// Xử lý sự kiện khi thay đổi kích thước trang trong ComboBox
        /// </summary>
        /// <param name="sender">ComboBox điều khiển kích thước trang</param>
        /// <param name="e">Tham số sự kiện chứa thông tin về sự thay đổi</param>
        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo && ViewModel != null)
            {
                int newSize = (combo.SelectedIndex + 1) * 5;
                ViewModel.ChangePageSizeCommand.Execute(newSize);
            }
        }

        /// <summary>
        /// Xử lý sự kiện khi người dùng nhập text tìm kiếm
        /// </summary>
        /// <param name="sender">AutoSuggestBox dùng để tìm kiếm</param>
        /// <param name="args">Tham số sự kiện chứa thông tin về text đã thay đổi</param>
        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.SearchText = sender.Text;
            }
        }
    }
}
