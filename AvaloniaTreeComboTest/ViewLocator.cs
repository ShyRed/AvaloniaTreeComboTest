using System;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using AvaloniaTreeComboTest.Core.ViewModels;
using Microsoft.Extensions.DependencyInjection;

namespace AvaloniaTreeComboTest;

public class ViewLocator : IDataTemplate
{
    private readonly IServiceProvider _serviceProvider;

    public ViewLocator(IServiceProvider serviceProvider)
    {
        _serviceProvider = serviceProvider;
    }
    
    public Control? Build(object? param)
    {
        if (param is null)
            return null;

        var name = param.GetType().FullName!
            .Replace("ViewModel", "View", StringComparison.Ordinal)
            .Replace(".Core.", ".", StringComparison.Ordinal);
        
        var type = Type.GetType(name);

        if (type != null)
        {
            return (Control)_serviceProvider.GetRequiredService(type);
        }

        return new TextBlock { Text = "Not Found: " + name };
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}