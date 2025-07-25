using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaTreeComboTest.Core.ViewModels;

public partial class MainWindowViewModel : ViewModelBase
{
    [ObservableProperty] private bool _isPaneOpen = true;

    [ObservableProperty] private ViewModelBase _currentPage;
    
    [ObservableProperty] private NavigationItem _selectedNavigationItem;
    
    public ObservableCollection<NavigationItem> NavigationItems { get; } =
    [
        new(typeof(HomePageViewModel)),
        new(typeof(TreePageViewModel)),
        new(typeof(ComboBoxPageViewModel))
    ];
    
    private readonly IServiceProvider _serviceProvider;
    
    [RelayCommand]
    private void TogglePane() => IsPaneOpen = !IsPaneOpen;

    partial void OnSelectedNavigationItemChanged(NavigationItem value)
    {
        CurrentPage = (ViewModelBase)_serviceProvider.GetRequiredService(value.PageType);
    }

    public MainWindowViewModel(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
        _currentPage = serviceProvider.GetRequiredService<HomePageViewModel>();
        _selectedNavigationItem = NavigationItems[0];
    }
}

public sealed class NavigationItem(Type pageType)
{
    public string Name { get; } = MakeName(pageType);
    public Type PageType { get; } = pageType;

    private static string MakeName(Type pageType)
    {
        var name = pageType.Name.Replace("ViewModel", "", StringComparison.Ordinal);
        for (var i = 1; i < name.Length; i++)
        {
            if (!char.IsUpper(name[i])) continue;
            name = name.Insert(i, " ");
            i++;
        }
        return name;
    }
}