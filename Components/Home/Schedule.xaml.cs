using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using login_full.Helpers;

// To learn more about WinUI, the WinUI project structure,
// and more about our project templates, see: http://aka.ms/winui-project-info.

namespace login_full.Components.Home
{
    public sealed partial class Schedule : UserControl
    {
		private readonly ScheduleManager scheduleManager = null;
		public Schedule()
        {
            this.InitializeComponent();
			scheduleManager = new ScheduleManager(ScheduleListView);
		}
		private void AddNewEvent_Click(object sender, RoutedEventArgs e)
		{
			scheduleManager.AddNewEvent(this.XamlRoot);
		}
	}
}
