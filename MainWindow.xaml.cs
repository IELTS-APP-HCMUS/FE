using Google.Apis.Util;
using login_full;
using Microsoft.Extensions.Configuration;
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
using System.Threading;
using System.Threading.Tasks;
using Windows.Foundation;
using Windows.Foundation.Collections;
using Google.Apis.Auth;
using Google.Apis.Auth.OAuth2.Responses;
using Microsoft.UI;
using Windows.Storage;
using System.Security.Cryptography;
using System.Text;
using Windows.Services.Maps;
using Microsoft.UI.Dispatching;
using Microsoft.UI.Windowing;
using Windows.Graphics;



namespace login_full
{

    public sealed partial class MainWindow : Window
    {
        private readonly UserAuthenticationService _authService;
        //private readonly ApiService _apiService;


        // size of the window
        private const int MinWindowWidth = 850;
        private const int MinWindowHeight = 0;

        public MainWindow()
        {
            this.InitializeComponent();
            (Application.Current as App).MainWindow = this;
            _authService = new UserAuthenticationService();
         //   _apiService = new ApiService();
            CheckSavedCredentials();
            this.SizeChanged += MainWindow_SizeChanged;

        }

        // resize the window
        private void MainWindow_SizeChanged(object sender, WindowSizeChangedEventArgs e)
        {
            // Ensure the window does not shrink below the minimum size
            IntPtr hWnd = WinRT.Interop.WindowNative.GetWindowHandle(this);
            var windowId = Microsoft.UI.Win32Interop.GetWindowIdFromWindow(hWnd);
            var appWindow = AppWindow.GetFromWindowId(windowId);

            // Get the current size
            var currentSize = appWindow.Size;

            // Check and enforce minimum width and height
            int newWidth = Math.Max(currentSize.Width, MinWindowWidth);
            int newHeight = Math.Max(currentSize.Height, MinWindowHeight);

            // Resize the window to maintain the minimum size
            appWindow.Resize(new SizeInt32(newWidth, newHeight));
        }
        private async void GoogleSignInButton_Click(object sender, RoutedEventArgs e)
        {
            try
            {
                var configuration = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .Build();

                var googleAuthService = new GoogleAuthService(configuration);
                var credential = await googleAuthService.AuthenticateAsync(CancellationToken.None);

                if (credential == null)
                {
                    await ShowErrorDialogAsync("Authentication failed. No credential returned.");
                    return;
                }

                if (credential.Token.IsExpired(SystemClock.Default))
                {
                    try
                    {
                        await credential.RefreshTokenAsync(CancellationToken.None);
                    }
                    catch (Exception refreshEx)
                    {
                        await ShowErrorDialogAsync($"Failed to refresh token: {refreshEx.Message}");
                        return;
                    }
                }

                await ShowSuccessDialogAsync($"Signed in successfully. User ID: {credential.UserId}");

                // TODO: Implement your post-login logic here
            }
        
            catch (Google.Apis.Auth.OAuth2.Responses.TokenResponseException trEx)
            {
                await ShowErrorDialogAsync($"Token response error: {trEx.Message}");
            }
            catch (Exception ex)
            {
                await ShowErrorDialogAsync($"Unexpected error: {ex.Message}");
            }
        }

        private async Task ShowErrorDialogAsync(string message)
        {
            if (Dispatcher == null)
            {
                throw new InvalidOperationException("Dispatcher is not initialized.");
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Sign In Error",
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
            });
        }

        private async Task ShowSuccessDialogAsync(string message)
        {
            if (Dispatcher == null)
            {
                throw new InvalidOperationException("Dispatcher is not initialized.");
            }

            await Dispatcher.RunAsync(Windows.UI.Core.CoreDispatcherPriority.Normal, async () =>
            {
                ContentDialog dialog = new ContentDialog
                {
                    Title = "Sign In Successful",
                    Content = message,
                    CloseButtonText = "OK"
                };
                await dialog.ShowAsync();
            });
        }

        private void LoginButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.Background = new SolidColorBrush(Colors.White);
            RegisterButton.Background = new SolidColorBrush(Colors.Transparent);
            LoginPanel.Visibility = Visibility.Visible;
            RegisterPanel.Visibility = Visibility.Collapsed;
        }

        private void RegisterButton_Click(object sender, RoutedEventArgs e)
        {
            LoginButton.Background = new SolidColorBrush(Colors.Transparent);
            RegisterButton.Background = new SolidColorBrush(Colors.White);
            LoginPanel.Visibility = Visibility.Collapsed;
            RegisterPanel.Visibility = Visibility.Visible;
        }

        private void CheckSavedCredentials()
        {
            if (_authService.HasSavedCredentials())
            {
                NavigateToHomePage();
            }
        }

        private void Login1Button_Click(object sender, RoutedEventArgs e)
        {
            string username = UsernameTextBox.Text;
            string password = PasswordBox.Password;

            if (_authService.ValidateCredentials(username,password))
            {
                if (RememberMeCheckbox.IsChecked == true)
                {
                   _authService.SaveCredentials(username, password);
                }
                NavigateToHomePage();
            }
            else
            {
                ErrorMessageTextBlock.Text = "Invalid username or password.";
                ErrorMessageTextBlock.Visibility = Visibility.Visible;
            }
        }

    
        private void NavigateToHomePage()
        {

            LoginGrid.Visibility = Visibility.Collapsed;
            MainFrame.Visibility = Visibility.Visible;
            MainFrame.Navigate(typeof(HomePage));
        }



        // API login

        //private async void RegisterButton1_Click(object sender, RoutedEventArgs e)
        //{
        //    LoginButton.Background = new SolidColorBrush(Colors.Transparent);
        //    RegisterButton.Background = new SolidColorBrush(Colors.White);
        //    LoginPanel.Visibility = Visibility.Collapsed;
        //    RegisterPanel.Visibility = Visibility.Visible;

        //    string email = RegisterEmailTextBox.Text;
        //    string fullName = FullNameTextBox.Text;
        //    string password = RegisterPasswordBox.Password;
        //    string confirmPassword = ConfirmPasswordBox.Password;

        //    if (string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(fullName) ||
        //        string.IsNullOrWhiteSpace(password) || string.IsNullOrWhiteSpace(confirmPassword))
        //    {
        //        await ShowErrorDialogAsync("Please fill in all fields.");
        //        return;
        //    }

        //    if (password != confirmPassword)
        //    {
        //        await ShowErrorDialogAsync("Passwords do not match.");
        //        return;
        //    }

        //    var (firstName, lastName) = SplitFullName(fullName);

        //    var user = new UserRegistrationModel
        //    {
        //        Email = email,
        //        Password = password,
        //        FirstName = firstName,
        //        LastName = lastName
        //    };

        //    try
        //    {
        //        bool isRegistered = await _apiService.RegisterUser(user);
        //        if (isRegistered)
        //        {
        //            await ShowSuccessDialogAsync("Registration successful!");
        //            // Optionally, you can automatically log the user in or navigate to the login page
        //        }
        //        else
        //        {
        //            await ShowErrorDialogAsync("Registration failed. Please try again.");
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        await ShowErrorDialogAsync($"An error occurred: {ex.Message}");
        //    }
        //}

        //private (string FirstName, string LastName) SplitFullName(string fullName)
        //{
        //    var nameParts = fullName.Trim().Split(' ');
        //    if (nameParts.Length == 1)
        //    {
        //        return (nameParts[0], "");
        //    }
        //    else
        //    {
        //        return (nameParts[0], string.Join(" ", nameParts.Skip(1)));
        //    }
        //}
    }

}

