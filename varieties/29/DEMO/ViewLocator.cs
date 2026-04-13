// Вариант 29
using System;
using System.Diagnostics.CodeAnalysis;
using Avalonia.Controls;
using Avalonia.Controls.Templates;
using DEMO.ViewModels;

namespace DEMO;

/// <summary>
/// По модели представления возвращает соответствующее окно, если тип найден.
/// </summary>
[RequiresUnreferencedCode(
    "Default implementation of ViewLocator involves reflection which may be trimmed away.",
    Url = "https://docs.avaloniaui.net/docs/concepts/view-locator")]
public class ViewLocator : IDataTemplate
{
    public Control? Build(object? param)
    {
        if (param is not object instance)
        {
            return null;
        }

        var name = instance.GetType().FullName!.Replace("ViewModel", "View", StringComparison.Ordinal);
        var viewType = Type.GetType(name);

        if (viewType is null)
            return new TextBlock { Text = "Not Found: " + name };

        return (Control)Activator.CreateInstance(viewType)!;
    }

    public bool Match(object? data)
    {
        return data is ViewModelBase;
    }
}
