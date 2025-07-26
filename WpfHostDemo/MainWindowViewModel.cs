using Avalonia;
using Avalonia.Controls;
using Avalonia.Threading;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using AvaloniaTreeComboTest;
using Microsoft.Extensions.DependencyInjection;

namespace WpfHostDemo;

public partial class MainWindowViewModel : ObservableObject
{
    private static bool _isAvaloniaInitialized;
    private static Window? _avaloniaParentWindow;
    private static Application? _avaloniaApp;
    private AvaloniaTreeComboTest.Views.MainWindow? _currentAvaloniaWindow;
    private readonly System.Windows.Window _wpfWindow;

    public MainWindowViewModel(System.Windows.Window wpfWindow)
    {
        _wpfWindow = wpfWindow;
        _wpfWindow.Activated += WpfWindow_Activated;
    }

    private void WpfWindow_Activated(object? sender, EventArgs e)
    {
        if (_currentAvaloniaWindow != null && _currentAvaloniaWindow.IsVisible)
        {
            _currentAvaloniaWindow.Activate();

            // Get the Avalonia window handle and flash it
            if (_currentAvaloniaWindow.TryGetPlatformHandle()?.Handle is { } handle)
            {
                WindowsInteropUtils.FlashWindow(handle);
            }

        }
    }

    [RelayCommand]
    private async Task OpenAvaloniaWindow()
    {
        // Initialize Avalonia if not already initialized
        if (!_isAvaloniaInitialized)
        {
            var builder = Program.BuildAvaloniaApp();
            _avaloniaApp = builder.Instance;
            builder.SetupWithoutStarting();
            
            _avaloniaParentWindow = new Window()
            {
                Position = new PixelPoint(-9999, -9999),
                Width = 10,
                Height = 10,
                ShowInTaskbar = false,
            };
            _isAvaloniaInitialized = true;
        }

        // Disable the WPF window
        _wpfWindow.IsEnabled = false;

        try
        {
            await Dispatcher.UIThread.InvokeAsync(async () =>
            {
                if (_avaloniaParentWindow is null)
                    return;
                
                _currentAvaloniaWindow = new ()
                {
                    DataContext = AvaloniaTreeComboTest.App.Services.GetRequiredService<AvaloniaTreeComboTest.Core.ViewModels.MainWindowViewModel>(),
                    Position = new PixelPoint(
                        (int)_wpfWindow.Left + 50,
                        (int)_wpfWindow.Top + 50 )
                };

                _currentAvaloniaWindow.PositionChanged += (_, e) =>
                {
                    _avaloniaParentWindow.Position = e.Point;
                };

                // Handle window closing to re-enable the WPF window
                _currentAvaloniaWindow.Closed += (_, _) =>
                {
                    _wpfWindow.Dispatcher.Invoke(() =>
                    {
                        _wpfWindow.IsEnabled = true;
                        _currentAvaloniaWindow = null;
                    });
                };

                _avaloniaParentWindow.Show();
                await _currentAvaloniaWindow.ShowDialog(_avaloniaParentWindow!);
                _avaloniaParentWindow.Hide();
                
            });
        }
        finally
        {
            // Ensure the WPF window is re-enabled even if there's an error
            _wpfWindow.Dispatcher.Invoke(() => _wpfWindow.IsEnabled = true);
        }
    }
}