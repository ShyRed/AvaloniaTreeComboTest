using System.Collections.ObjectModel;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;

namespace AvaloniaTreeComboTest.Core.ViewModels;

public partial class TreePageViewModel : ViewModelBase
{
    private const int MaxItems = 100000;
    
    [ObservableProperty] private ObservableCollection<TreeItem> _items = [];

    [RelayCommand]
    private async Task LoadItemsAsync()
    {
        Items = await Task.Run(() =>
        {
            var items = new List<TreeItem>(MaxItems);
            var parent = new TreeItem("First Entry", 0, []);
            for (var i = 1; i < MaxItems; i++)
            {
                if (i % 1000 == 0 || i == MaxItems - 1)
                {
                    items.Add(parent);
                    parent = new TreeItem($"Parent {i}", i, []);
                }
                else
                    parent.Children!.Add(new TreeItem($"Child {i}", i));
            }
            return new ObservableCollection<TreeItem>(items);
        });
    }
}

public partial class TreeItem : ObservableObject
{
    [ObservableProperty] private string _name;
    [ObservableProperty] private int _value;
    [ObservableProperty] private ObservableCollection<TreeItem>? _children;

    public TreeItem(string name, int value)
    {
        _name = name;
        _value = value;
    }

    public TreeItem(string name, int value, ObservableCollection<TreeItem>? children)
        : this(name, value)
    {
        _children = children;
    }
}