using System;
using System.Collections.Generic;
using Avalonia.Controls;
using Avalonia.VisualTree;

namespace AvaloniaUI.Ribbon;

public static class RibbonControlExtensions
{
    public static void ForEach<T>(this IEnumerable<T> source, Action<T> action)
    {
        foreach (var item in source) action(item);
    }
}