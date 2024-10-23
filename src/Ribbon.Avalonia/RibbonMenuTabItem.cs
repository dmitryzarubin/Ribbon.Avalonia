using System;
using Avalonia;
using Avalonia.Controls.Primitives;
using Avalonia.Controls.Templates;

namespace Ribbon.Avalonia;

public class MyClass
{
    public int ItemA { get; set; }
    public int ItemB { get; set; }
}

public class RibbonMenuTabItem : ToggleButton, IRibbonMenuItem
{
    public static readonly StyledProperty<IControlTemplate> IconProperty =
        AvaloniaProperty.Register<RibbonMenuTabItem, IControlTemplate>(nameof(Icon));

    public static readonly StyledProperty<RibbonMenuItemPlacement> PlacementProperty =
        AvaloniaProperty.Register<RibbonMenuTabItem, RibbonMenuItemPlacement>(nameof(Placement));

    public static readonly StyledProperty<IControlTemplate> ControlPaneProperty =
        AvaloniaProperty.Register<RibbonMenuTabItem, IControlTemplate>(nameof(ControlPane));

    public static readonly DirectProperty<RibbonMenuTabItem, object?> ControlPaneDataProperty =
        AvaloniaProperty.RegisterDirect<RibbonMenuTabItem, object?>(nameof(ControlPaneData), o => o.ControlPaneData,
            (item, o) => item.ControlPaneData = o);

    protected override Type StyleKeyOverride { get; } = typeof(RibbonMenuTabItem);

    public IControlTemplate Icon
    {
        get => GetValue(IconProperty);
        set => SetValue(IconProperty, value);
    }

    private object? _controlPaneData = new MyClass { ItemA = 1, ItemB = 2 };
    public object? ControlPaneData
    {
        get => _controlPaneData;
        set => SetAndRaise(ControlPaneDataProperty, ref _controlPaneData, value);
    }

    public IControlTemplate ControlPane
    {
        get => GetValue(ControlPaneProperty);
        set => SetValue(ControlPaneProperty, value);
    }

    public RibbonMenuItemPlacement Placement
    {
        get => GetValue(PlacementProperty);
        set => SetValue(PlacementProperty, value);
    }
}