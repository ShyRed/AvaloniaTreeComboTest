using AvaloniaTreeComboTest.Core.ViewModels;
using AvaloniaTreeComboTest.Views;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaTreeComboTest;

public static class DependencyInjection
{
    public static ServiceCollection AddFrontendServices(this ServiceCollection services)
    {
        services
            .AddTransient<MainWindow>()
            .AddTransient<HomePageView>()
            .AddTransient<TreePageView>()
            .AddTransient<ComboBoxPageView>()
            .AddTransient<ViewLocator>();
        return services;
    }
}