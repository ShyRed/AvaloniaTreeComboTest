using AvaloniaTreeComboTest.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaTreeComboTest.Core;

public static class DependencyInjection
{
    public static ServiceCollection AddCoreServices(this ServiceCollection services)
    {
        services
            .AddTransient<MainWindowViewModel>()
            .AddTransient<HomePageViewModel>()
            .AddTransient<TreePageViewModel>()
            .AddTransient<ComboBoxPageViewModel>();
        return services;
    }
}