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

		//public TestResultPage()
		//{
		//    this.InitializeComponent();
		//    this.Loaded += TestResultPage_Loaded;
		//}

		//protected override void OnNavigatedTo(NavigationEventArgs e)
		//{
		//    base.OnNavigatedTo(e);
		//    if (e.Parameter is TestResultViewModel viewModel)
		//    {
		//        ViewModel = viewModel;
		//        DrawPieChart();
		//    }
		//}
		//private void BackButton_Click(object sender, RoutedEventArgs e)
		//{
		//    Frame.Navigate(typeof(reading_listening_Ui));
		//}

		//private void RetryButton_Click(object sender, RoutedEventArgs e)
		//{
		//    // Logic ?? làm l?i bài thi
		//}

		//private void HomeButton_Click(object sender, RoutedEventArgs e)
		//{
		//    Frame.Navigate(typeof(HomePage));
		//}

		//private void TestResultPage_Loaded(object sender, RoutedEventArgs e)
		//{
		//    DrawPieChart();
		//}

		//// ...

		//private void DrawPieChart()
		//{
		//    if (ViewModel == null) return;

		//    PieChart.Children.Clear();

		//    double centerX = 100;
		//    double centerY = 100;
		//    double radius = 80;
		//    double startAngle = 0;

		//    // V? ph?n câu ?úng (màu xanh)
		//    if (ViewModel.CorrectAnswers > 0)
		//    {
		//        DrawPieSlice(centerX, centerY, radius, startAngle,
		//                    (double)ViewModel.CorrectAnswers / ViewModel.TotalQuestions * 360,
		//                    new SolidColorBrush(Colors.Green));
		//    }
		//    startAngle += (double)ViewModel.CorrectAnswers / ViewModel.TotalQuestions * 360;

		//    // V? ph?n câu sai (màu ??)
		//    if (ViewModel.WrongAnswers > 0)
		//    {
		//        DrawPieSlice(centerX, centerY, radius, startAngle,
		//                    (double)ViewModel.WrongAnswers / ViewModel.TotalQuestions * 360,
		//                    new SolidColorBrush(Colors.Red));
		//    }
		//    startAngle += (double)ViewModel.WrongAnswers / ViewModel.TotalQuestions * 360;

		//    // V? ph?n ch?a làm (màu xám)
		//    if (ViewModel.UnansweredQuestions > 0)
		//    {
		//        DrawPieSlice(centerX, centerY, radius, startAngle,
		//                    (double)ViewModel.UnansweredQuestions / ViewModel.TotalQuestions * 360,
		//                    new SolidColorBrush(Colors.LightGray));
		//    }
		//}

		//private void DrawPieSlice(double centerX, double centerY, double radius,
		//                         double startAngle, double sweepAngle, Brush fill)
		//{
		//    double startRadians = startAngle * Math.PI / 180;
		//    double sweepRadians = sweepAngle * Math.PI / 180;
		//    double endRadians = startRadians + sweepRadians;

		//    bool isLargeArc = sweepAngle > 180;

		//    double startX = centerX + radius * Math.Cos(startRadians);
		//    double startY = centerY + radius * Math.Sin(startRadians);
		//    double endX = centerX + radius * Math.Cos(endRadians);
		//    double endY = centerY + radius * Math.Sin(endRadians);

		//    var figure = new PathFigure
		//    {
		//        StartPoint = new Point(centerX, centerY),
		//        IsClosed = true
		//    };

		//    figure.Segments.Add(new LineSegment { Point = new Point(startX, startY) });
		//    figure.Segments.Add(new ArcSegment
		//    {
		//        Point = new Point(endX, endY),
		//        Size = new Size(radius, radius),
		//        IsLargeArc = isLargeArc,
		//        SweepDirection = SweepDirection.Clockwise,
		//    });

		//    var geometry = new PathGeometry();
		//    geometry.Figures.Add(figure);

		//    var path = new Microsoft.UI.Xaml.Shapes.Path
		//    {
		//        Fill = fill,
		//        Data = geometry
		//    };

		//    PieChart.Children.Add(path);
		//}
		public TestResultPage()
		{
			this.InitializeComponent();
		}

		protected override async void OnNavigatedTo(NavigationEventArgs e)
		{
			base.OnNavigatedTo(e);

			// L?y ViewModel t? App
			ViewModel = (App.Current as App)?.CurrentTestResult;
			if (e.Parameter is string item)
			{
				// Load test dựa trên TestId của item được chọn
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
