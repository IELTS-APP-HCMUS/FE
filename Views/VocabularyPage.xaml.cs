using Microsoft.UI.Xaml.Controls;
using login_full.ViewModels;

namespace login_full.Views
{
    public sealed partial class VocabularyPage : Page
    {
        private VocabularyViewModel _viewModel;

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

        private void PageSizeComboBox_SelectionChanged(object sender, SelectionChangedEventArgs e)
        {
            if (sender is ComboBox combo && ViewModel != null)
            {
                int newSize = (combo.SelectedIndex + 1) * 5;
                ViewModel.ChangePageSizeCommand.Execute(newSize);
            }
        }

        private void SearchBox_TextChanged(AutoSuggestBox sender, AutoSuggestBoxTextChangedEventArgs args)
        {
            if (args.Reason == AutoSuggestionBoxTextChangeReason.UserInput)
            {
                ViewModel.SearchText = sender.Text;
            }
        }
    }
}
