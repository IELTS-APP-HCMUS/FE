using Microsoft.UI.Xaml.Controls;
using Microsoft.UI.Xaml.Media;
using Microsoft.UI.Xaml;
using Microsoft.UI;
using Windows.UI;

namespace login_full.Helpers
{
    public class LoaderManager
    {
        private readonly Window RootWindow;
        private Grid OverlayGrid;

        public LoaderManager(Window rootWindow)
        {
            RootWindow = rootWindow;
        }

        public void ShowLoader()
        {
            if (OverlayGrid != null)
                return;

            ProgressRing progressRing = new ProgressRing
            {
                IsActive = true,
                Width = 80,
                Height = 80,
                Foreground = new SolidColorBrush(Colors.White),
                HorizontalAlignment = HorizontalAlignment.Center,
                VerticalAlignment = VerticalAlignment.Center
            };

            OverlayGrid = new Grid
            {
                Background = new SolidColorBrush(Color.FromArgb(150, 0, 0, 0)), // Màu nền mờ
                HorizontalAlignment = HorizontalAlignment.Stretch,
                VerticalAlignment = VerticalAlignment.Stretch,
                Width = RootWindow.Bounds.Width,  // Sử dụng kích thước của cửa sổ hiện tại
                Height = RootWindow.Bounds.Height
            };

            OverlayGrid.Children.Add(progressRing);

            var rootFrame = RootWindow.Content as FrameworkElement;
            if (rootFrame is Grid rootGrid)
            {
                rootGrid.Children.Add(OverlayGrid);
            }
            else
            {
                Grid newRootGrid = new Grid();
                if (rootFrame != null)
                {
                    RootWindow.Content = null;
                    newRootGrid.Children.Add(rootFrame);
                }
                newRootGrid.Children.Add(OverlayGrid);
                RootWindow.Content = newRootGrid;
            }
        }

        public void HideLoader()
        {
            if (OverlayGrid != null)
            {
                var rootFrame = RootWindow.Content as FrameworkElement;
                if (rootFrame is Grid rootGrid)
                {
                    rootGrid.Children.Remove(OverlayGrid);
                }
                OverlayGrid = null;
            }
        }
    }
}
