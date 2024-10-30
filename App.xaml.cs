﻿using Microsoft.UI.Xaml;
using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Controls.Primitives;
using Microsoft.UI.Xaml.Data;
using Microsoft.UI.Xaml.Input;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml.Navigation;
using Microsoft.UI.Xaml.Shapes;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices.WindowsRuntime;
using Windows.ApplicationModel;
using Windows.ApplicationModel.Activation;
using Windows.Foundation;
using Windows.Foundation.Collections;
using System.IO;
using login_full.API_Services;


namespace login_full
{

    public partial class App : Application
    {
        public Window MainWindow { get; set; }
		public static bool IsLoggedInWithGoogle { get; set; } = false;
		public App()
        {
            this.InitializeComponent();
			var configService = new ConfigService(); 
			var dbService = new DatabaseService(configService);
			dbService.ConnectToDatabase(); 
		}

        protected override void OnLaunched(Microsoft.UI.Xaml.LaunchActivatedEventArgs args)
        {
            m_window = new MainWindow();
            m_window.Activate();
        }

        private Window m_window;

    }
}
