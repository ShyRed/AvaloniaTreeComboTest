using System;
using System.Globalization;
using Avalonia;
using Avalonia.Controls;
using Avalonia.Data.Converters;
using Avalonia.Media;
using AvaloniaTreeComboTest.Core.ViewModels;

namespace AvaloniaTreeComboTest.ValueConverters;

public sealed class NavigationItemToIconConverter : IValueConverter
{
    public static NavigationItemToIconConverter Instance { get; } = new();
    
    public object? Convert(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        if (value is not NavigationItem navigationItem || Application.Current is null)
            return null;

        if (!Application.Current.TryFindResource(navigationItem.PageType.Name.Replace("ViewModel", ""), out var icon))
            return null;

        return icon as StreamGeometry;
    }

    public object ConvertBack(object? value, Type targetType, object? parameter, CultureInfo culture)
    {
        throw new NotSupportedException();
    }
}