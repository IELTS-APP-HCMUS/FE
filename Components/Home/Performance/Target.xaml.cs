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
using System.IO;
using System.Linq;
using System.Net.Http;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;

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
			double overallTarget = -1;
			if (ReadingScoreTextBlock.Text != "" && ListeningScoreTextBlock.Text != ""
				&& WritingScoreTextBlock.Text != "" && SpeakingScoreTextBlock.Text != "")
			{

				double readingTarget = double.Parse(ReadingScoreTextBlock.Text);
				double listeningTarget = double.Parse(ListeningScoreTextBlock.Text);
				double writingTarget = double.Parse(WritingScoreTextBlock.Text);
				double speakingTarget = double.Parse(SpeakingScoreTextBlock.Text);
				if (readingTarget != -1 && listeningTarget != -1 && writingTarget != -1 && speakingTarget != -1)
				{
					overallTarget = (readingTarget + listeningTarget + writingTarget + speakingTarget) / 4;
					overallTarget = Math.Round(overallTarget * 2) / 2;
				}
				OverallScoreTextBlock.Text = overallTarget == -1 ? "-" : overallTarget.ToString();
			}
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
