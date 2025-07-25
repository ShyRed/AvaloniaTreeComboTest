using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaTreeComboTest.Core.ViewModels;

public partial class ComboBoxPageViewModel : ViewModelBase
{
    private const int MaxItems = 100000;
    
    [ObservableProperty] private List<ComboBoxItem> _items = [];

    [ObservableProperty] private ComboBoxItem? _selectedItem;

    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        Items = await Task.Run(() =>
        {
            var items = new List<ComboBoxItem>(MaxItems);
            for (var i = 0; i < MaxItems; i++)
            {
                items.Add(new ComboBoxItem(i.ToString(), i));
            }
            return items;
        });
    }
}

public sealed class ComboBoxItem(string name, int value)
{
    public string Name { get; } = name;
    public int Value { get; } = value;
}