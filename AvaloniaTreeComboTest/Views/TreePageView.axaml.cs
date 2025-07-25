using System;
using System.Collections.ObjectModel;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Controls.Models.TreeDataGrid;
using Avalonia.Markup.Xaml;
using Avalonia.ReactiveUI;
using AvaloniaTreeComboTest.Core.ViewModels;
using DynamicData.Binding;
using ReactiveUI;

namespace AvaloniaTreeComboTest.Views;

public partial class TreePageView : UserControl
{
    private TreePageViewModel ViewModel => (TreePageViewModel)DataContext!;

    public TreePageView()
    {
        InitializeComponent();
    }

    protected override void OnInitialized()
    {
        base.OnInitialized();
        
        this.WhenAnyValue(x => x.ViewModel.Items)
            .Subscribe(x =>
            {
                HierarchicalTreeDataGridSource<TreeItem> source = new(ViewModel.Items);
                source.Columns.Add(new HierarchicalExpanderColumn<TreeItem>(
                    new TextColumn<TreeItem, string>(
                        "Name",
                        item => item.Name,
                        options: new TextColumnOptions<TreeItem>() { IsTextSearchEnabled = true}),
                    item => item.Children) );
                source.Columns.Add(new TextColumn<TreeItem,int>(
                    "Value",
                    item => item.Value));
                TreeCtrl.Source = source;
            });
    }
}