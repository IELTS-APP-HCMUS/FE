using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Navigation;
using login_full.ViewModels;
using login_full.Models;

namespace login_full.Views
{
    public sealed partial class TestDetailResultPage : Page
    {
        public TestDetailResultViewModel ViewModel { get; private set; }

        public TestDetailResultPage()
        {
            this.InitializeComponent();
        }

        protected override void OnNavigatedTo(NavigationEventArgs e)
        {
            base.OnNavigatedTo(e);

            if (e.Parameter is ReadingTestDetail testDetail)
            {
                // Khởi tạo ViewModel với testDetail được truyền vào
                ViewModel = new TestDetailResultViewModel(
                    App.NavigationService,
                    testDetail
                );

                // Khởi tạo OptionModels cho mỗi câu hỏi
                foreach (var question in testDetail.Questions)
                {
                    question.InitializeOptionModels();
                }

                this.DataContext = ViewModel;
            }
        }
    }
}
