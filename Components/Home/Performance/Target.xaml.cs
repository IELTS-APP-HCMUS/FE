using login_full.Context;
using login_full.Models;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using YamlDotNet.Core.Tokens;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home.Performance
{
	public sealed partial class Target : UserControl
	{
		public TargetUpdatePopUp TargetUpdatePopUpCompControl => TargetUpdatePopUpComp;
		public Target()
		{
			this.InitializeComponent();
			// Lắng nghe thay đổi từ DataContext nếu cần
			this.DataContextChanged += (s, e) =>
			{

				if (DataContext is UserTarget data)
				{
					data.PropertyChanged += (sender, args) =>
					{
						if (args.PropertyName == nameof(UserTarget.TargetReading) ||
							args.PropertyName == nameof(UserTarget.TargetListening) ||
							args.PropertyName == nameof(UserTarget.TargetWriting) ||
							args.PropertyName == nameof(UserTarget.TargetSpeaking))
						{
							UpdateOverallScore();
						}
					};
				}
				UpdateOverallScore();
			};
			//double overallTarget = -1;
			//if (ReadingScoreTextBlock.Text != "" && ListeningScoreTextBlock.Text != ""
			//	&& WritingScoreTextBlock.Text != "" && SpeakingScoreTextBlock.Text != "")
			//{

			//	double readingTarget = double.Parse(ReadingScoreTextBlock.Text);
			//	double listeningTarget = double.Parse(ListeningScoreTextBlock.Text);
			//	double writingTarget = double.Parse(WritingScoreTextBlock.Text);
			//	double speakingTarget = double.Parse(SpeakingScoreTextBlock.Text);
			//	if (readingTarget != -1 && listeningTarget != -1 && writingTarget != -1 && speakingTarget != -1)
			//	{
			//		overallTarget = (readingTarget + listeningTarget + writingTarget + speakingTarget) / 4;
			//		overallTarget = Math.Round(overallTarget * 2) / 2;
			//	}
			//	OverallScoreTextBlock.Text = overallTarget == -1 ? "-" : overallTarget.ToString();
			//}
		}
		private string _overallScore = "------";
		public string OverallScore
		{
			get => _overallScore;
			set
			{
				if (_overallScore != value)
				{
					_overallScore = value;
					OnPropertyChanged(nameof(OverallScore));
				}
			}
		}
		private void UpdateOverallScore()
		{
			System.Diagnostics.Debug.WriteLine("changed");
			if (DataContext is UserTarget data)
			{
				// Tính trung bình các giá trị
				var average = (data.TargetReading + data.TargetListening + data.TargetWriting + data.TargetSpeaking) / 4.0;
				OverallScore = average.ToString("F1"); // Định dạng 1 chữ số thập phân
				OverallScoreTextBlock.Text = OverallScore;
			}
			else
			{
				OverallScore = "N/A";
			}
		}

		public event PropertyChangedEventHandler PropertyChanged;
		private void OnPropertyChanged(string propertyName)
		{
			PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
		}
		private void ScoreCategoryButton_Click(object sender, RoutedEventArgs e)
		{
			Button clickedButton = sender as Button;
			if (clickedButton != null)
			{
				//string category = clickedButton.Content.ToString();
				TargetUpdatePopUpComp.IeltsScorePopupControl.IsOpen = true;
			}
		}
	}
}
