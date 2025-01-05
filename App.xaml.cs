using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using login_full.ViewModels;
using login_full.Services;
using login_full.API_Services;
using Microsoft.Extensions.DependencyInjection;
using System;

namespace login_full
{

    public partial class App : Application
    {
        public static Window MainWindow { get; set; }
		public static Frame MainFrame { get; set; }
		public static bool IsLoggedInWithGoogle { get; set; } = false;

		private NavigationService _navigationService;

		public TestResultViewModel CurrentTestResult { get; set; }
		//  public TestResultService TestResultService { get; } = new TestResultService();
		public IChartService ChartService { get; } = new ChartService();

		private IServiceProvider _serviceProvider;

		public App()
        {
            this.InitializeComponent();
            
            // Thêm cấu hình QuestPDF license
            QuestPDF.Settings.License = QuestPDF.Infrastructure.LicenseType.Community;
			Windows.Web.Http.Filters.HttpBaseProtocolFilter filter = new Windows.Web.Http.Filters.HttpBaseProtocolFilter();
			filter.IgnorableServerCertificateErrors.Add(Windows.Security.Cryptography.Certificates.ChainValidationResult.Untrusted);
			ConfigureServices();
		}

        private void ConfigureServices()
        {
            var services = new ServiceCollection();
            
            // Register services
            services.AddSingleton<INavigationService, NavigationService>();
            services.AddSingleton<IReadingTestService, ReadingTestService>();
            services.AddSingleton<IPdfExportService, PdfExportService>();
            services.AddSingleton<MockDictionaryService>();
            services.AddSingleton<TextHighlightService>();

            // Register ViewModels
            services.AddTransient<HistoryViewModel>();
            
            _serviceProvider = services.BuildServiceProvider();
            
            // Initialize ServiceLocator
            //ServiceLocator.Initialize(_serviceProvider);
        }

        public T GetService<T>() where T : class
        {
            return _serviceProvider.GetService<T>();
        }

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
			MainWindow = new MainWindow();

			// Initialize the global Frame and attach it to the MainWindow's content
			var frame = new Frame();
			App.MainFrame = frame;
			MainWindow.Content = frame;


			// Initialize NavigationService with the frame
			_navigationService = new NavigationService();
			_navigationService.Initialize(frame);

			// Perform navigation to LoginPage using NavigationService
			_navigationService.NavigateToAsync(typeof(LoginPage));

			MainWindow.Activate();
		}

		// Singleton pattern để truy cập NavigationService
		public static NavigationService NavigationService =>
			(Current as App)?._navigationService;


    }
}
